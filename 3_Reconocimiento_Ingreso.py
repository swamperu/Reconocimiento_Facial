import psycopg2
import cv2
import os
import time
import numpy as np
from datetime import datetime, timedelta
import face_recognition
import configparser

# Función para obtener todos los parámetros en una consulta
def obtener_parametros(cursor, codigos):
    placeholders = ','.join(['%s'] * len(codigos))
    cursor.execute(f"SELECT codigo, valor FROM maestro.parametro WHERE estado = 1 AND codigo IN ({placeholders});", codigos)
    resultados = cursor.fetchall()
    return {codigo: valor for codigo, valor in resultados}

# Función para registrar los logs
def registrar_log(cursor, comentario, numero_dni, es_finalizado):
    fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    query_log = """
        INSERT INTO transaccional.seguimiento_log (usuario, fecha_registro, comentario, numero_dni, es_finalizado) 
        VALUES (%s, %s, %s, %s, %s);
    """
    cursor.execute(query_log, ('python', fecha_hora_actual, comentario, numero_dni, es_finalizado))

# Leer configuración desde el archivo config.ini
config = configparser.ConfigParser()
config.read('config.ini')

# Conexión a la base de datos PostgreSQL usando los valores del archivo de configuración
def conectar_bd():
    try:
        cnxn = psycopg2.connect(
            host=config['database']['host'],
            port=config['database']['port'],
            database=config['database']['database'],
            user=config['database']['user'],
            password=config['database']['password']
        )
        return cnxn
    except Exception as e:
        print(f"Error conectando a la base de datos: {e}")
        exit()

# Conexión a la base de datos
with conectar_bd() as cnxn:
    with cnxn.cursor() as cursor:
        # Obtener los parámetros
        parametros = obtener_parametros(cursor, ["RUTA_CAPTURA", "TOTAL_FOTO", "SEGUNDOS_INTERVALO_RECONOCIMIENTO","TIEMPO_MENSAJE_CONFIRMACION","CAMARA_INGRESO", "CAMARA_SALIDA", "PORCENTAJE_CONFIANZA"])
        ruta_imagen = parametros.get("RUTA_CAPTURA")
        total_foto = parametros.get("TOTAL_FOTO")
        intervalo_reconocimiento = parametros.get("SEGUNDOS_INTERVALO_RECONOCIMIENTO")
        tiempo_mensaje_confirmacion = parametros.get("TIEMPO_MENSAJE_CONFIRMACION")
        camara_ingreso = parametros.get("CAMARA_INGRESO", '0')
        camara_salida = parametros.get("CAMARA_SALIDA", '1')
        porcentaje_confianza = float(parametros.get("PORCENTAJE_CONFIANZA"))

        # Ruta de datos
        dataPath = ruta_imagen
        
        # Cargar los encodings y nombres previamente entrenados
        known_encodings = np.load('encodings.npy')
        known_names = np.load('names.npy')

        # Iniciar captura de cámara
        #camara_id = int(camara_salida)
        camara_id = int(parametros.get("CAMARA_INGRESO", '0'))
        cap = cv2.VideoCapture(camara_id, cv2.CAP_DSHOW)
        cv2.namedWindow("Reconocimiento Facial Ingreso", cv2.WINDOW_NORMAL)  # Permite maximizar la ventana
        if not cap.isOpened():
            print("Error al abrir la cámara")
            exit()

        faceClassif = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

        # Variables para el mensaje de "Usuario Registrado"
        registro_exitoso = False
        tiempo_registro = None
        
        try:
            while True:
                ret, frame = cap.read()
                if not ret:
                    break

                # Convertir el frame de BGR a RGB para usarlo con face_recognition
                rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

                # Si el registro fue exitoso, mostrar mensaje durante unos segundos
                if registro_exitoso and tiempo_registro is not None and (datetime.now() - tiempo_registro).total_seconds() < int(tiempo_mensaje_confirmacion):
                    mensaje_registro = f"Se registro al Usuario"
                    mensaje_registro2 = f"{nombre_completo}"
                    cv2.putText(frame, mensaje_registro, (50, 50), 2, 1.2, (0, 0, 255), 2, cv2.LINE_AA)
                    cv2.putText(frame, mensaje_registro2, (50, 90), 2, 1.2, (0, 255, 0), 1, cv2.LINE_AA)
                    
                    cv2.imshow("Reconocimiento Facial Ingreso", frame)
                    if cv2.waitKey(1) == 27:  # Esc para salir
                        break
                    continue  # Saltar el proceso de reconocimiento hasta que pasen los segundos configurados

                registro_exitoso = False

                # Encontrar las ubicaciones de los rostros
                face_locations = face_recognition.face_locations(rgb_frame)
                face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)

                for face_encoding, face_location in zip(face_encodings, face_locations):
                    matches = face_recognition.compare_faces(known_encodings, face_encoding, tolerance=0.6)
                    face_distances = face_recognition.face_distance(known_encodings, face_encoding)
                    mejor_match = np.argmin(face_distances)

                    if matches[mejor_match]:
                        codigo_dni = known_names[mejor_match]

                        # Buscar trabajador y fecha del último registro en una sola consulta
                        cursor.execute("""
                            SELECT t.numero_dni, t.nombres, t.apellido_paterno, m.fecha_creacion 
                            FROM maestro.trabajador t
                            LEFT JOIN transaccional.marcacion m ON t.numero_dni = m.numero_dni
                            WHERE t.numero_dni = %s
                            ORDER BY m.fecha_creacion DESC LIMIT 1;
                        """, (codigo_dni,))
                        trabajador = cursor.fetchone()

                        if trabajador:
                            nombres, apellido_paterno, ultima_fecha_creacion = trabajador[1], trabajador[2], trabajador[3]
                            nombre_completo = f"{nombres} {apellido_paterno}"

                            # Verificar si el trabajador tiene un último registro de marcación
                            if ultima_fecha_creacion is not None:
                                 puede_registrar = datetime.now() >= ultima_fecha_creacion + timedelta(seconds=int(intervalo_reconocimiento))
                            else:
                                # Si no tiene registro previo, permitir el registro
                                puede_registrar = True
        
                            confianza_requerida = porcentaje_confianza / 100  # Convertir 80 a 0.8

                            if (1 - face_distances[mejor_match]) >= confianza_requerida and puede_registrar:
                                fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
                                ingreso_salida = 'I'
                                
                                # Registrar en la base de datos
                                cursor.execute("""
                                    INSERT INTO transaccional.marcacion 
                                    (numero_dni, confianza, fecha_hora, es_manual, camara, ingreso_salida, estado, usuario_creacion, fecha_creacion) 
                                    VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s);
                                """, (codigo_dni, str(1 - face_distances[mejor_match]), fecha_hora_actual, '0', 'webcam', ingreso_salida, 0, 'Python', fecha_hora_actual))
                                cnxn.commit()

                                # Mensaje de éxito y marcación en la pantalla
                                registro_exitoso = True
                                tiempo_registro = datetime.now()

                        # Dibujar el cuadro y el nombre en la imagen
                        top, right, bottom, left = face_location
                        cv2.rectangle(frame, (left, top), (right, bottom), (0, 255, 0), 2)
                        #cv2.putText(frame, nombre_completo, (left, top - 25), 2, 1.1, (0, 255, 0), 1, cv2.LINE_AA) No mostramos el nombre mientras sea menor el % de confianza
                        cv2.putText(frame, f'{1 - face_distances[mejor_match]:.2f}', (left, top), 2, 1.1, (0, 165, 255), 1, cv2.LINE_AA)
                    else:
                        top, right, bottom, left = face_location
                        cv2.rectangle(frame, (left, top), (right, bottom), (0, 0, 255), 2)
                        cv2.putText(frame, 'Desconocido', (left, top - 20), 2, 0.8, (0, 0, 255), 1, cv2.LINE_AA)

                cv2.imshow("Reconocimiento Facial Ingreso", frame)
                cv2.namedWindow("Reconocimiento Facial Ingreso", cv2.WINDOW_NORMAL)

                if cv2.waitKey(1) == 27:  # Esc para salir
                    break

        finally:
            cap.release()
            cv2.destroyAllWindows()
