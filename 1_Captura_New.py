import psycopg2
import cv2
import os
import face_recognition
from datetime import datetime
import configparser  # Importamos el módulo configparser

# Función para obtener el valor de un parámetro específico
def obtener_parametro(cursor, codigo):
    cursor.execute("SELECT valor FROM maestro.parametro WHERE estado = 1 AND codigo = %s;", (codigo,))
    resultado = cursor.fetchone()
    return resultado[0] if resultado else None

# Función para registrar los logs
def registrar_log(cursor, comentario, numero_dni, es_finalizado):
    fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    query_log = """
        INSERT INTO transaccional.seguimiento_log (usuario, fecha_registro, comentario,numero_dni,es_finalizado) 
        VALUES (%s, %s, %s, %s, %s);
    """
    cursor.execute(query_log, ('python', fecha_hora_actual, comentario, numero_dni, es_finalizado))
    cnxn.commit()

# Función para capturar y guardar rostros
def capturar_rostros(personPath, cap, total_foto):
    count = 0
    #lado = 250

    while True:
        ret, frame = cap.read()
        if not ret:
            print("Error en la captura de video.")
            break
        
        # Redimensionar el frame para mejorar la velocidad de procesamiento
        pequeño_frame = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)
        rgb_pequeño_frame = cv2.cvtColor(pequeño_frame, cv2.COLOR_BGR2RGB)

        # Detectar ubicaciones de los rostros
        ubicaciones_rostros = face_recognition.face_locations(rgb_pequeño_frame)

        for (top, right, bottom, left) in ubicaciones_rostros:
            top, right, bottom, left = top * 4, right * 4, bottom * 4, left * 4

            # Extraer y guardar el rostro
            #rostro = cv2.resize(frame[top:bottom, left:right], (lado, lado), interpolation=cv2.INTER_CUBIC)
            rostro = cv2.resize(frame[top:bottom, left:right], (int(tamaño_imagenes), int(tamaño_imagenes)), interpolation=cv2.INTER_CUBIC)
            
            #cv2.imwrite(f"{personPath}/rostro_{count}.jpg", rostro)
            # Guardar la imagen con el formato de dos dígitos en el nombre del archivo
            cv2.imwrite(f"{personPath}/rostro_{str(count).zfill(3)}.jpg", rostro)
            
            registrar_log(cursor, f"Capturando fotos para : {personPath}/rostro_{str(count).zfill(3)}", dni, '0')
            
            count += 1

            # Dibujar un rectángulo alrededor del rostro
            cv2.rectangle(frame, (left, top), (right, bottom), (0, 255, 0), 2)

        cv2.imshow('Rostros detectados', frame)

        # Salir con 'Esc' o si se alcanza el total de fotos
        if cv2.waitKey(1) == 27 or count >= total_foto:
            break

    return count

dni='';

# Leer configuración desde el archivo config.ini
config = configparser.ConfigParser()
config.read('config.ini')

# Conexión a la base de datos PostgreSQL usando los valores del archivo de configuración
try:
    cnxn = psycopg2.connect(
        host=config['database']['host'],
        port=config['database']['port'],
        database=config['database']['database'],
        user=config['database']['user'],
        password=config['database']['password']
    )
    cursor = cnxn.cursor()
except Exception as e:
    print(f"Error conectando a la base de datos: {e}")
    exit()


try:
    # Obtener los valores de los parámetros
    ruta_imagen = obtener_parametro(cursor, "RUTA_CAPTURA")
    total_foto = obtener_parametro(cursor, "TOTAL_FOTO")
    porcentaje_confianza = obtener_parametro(cursor, "PORCENTAJE_CONFIANZA")
    tamaño_imagenes = obtener_parametro(cursor, "TAMAÑO_IMAGENES")

    if not ruta_imagen or not total_foto or not porcentaje_confianza:
        raise ValueError("No se encontraron todos los parámetros necesarios en la tabla.")

    total_foto = int(total_foto)
    porcentaje_confianza = float(porcentaje_confianza)

    # Consulta para verificar si hay un trabajador con inicia_captura = '1'
    cursor.execute("""
        SELECT numero_dni, nombres, apellido_paterno 
        FROM maestro.trabajador WHERE inicia_captura = '1';
    """)
    resultado = cursor.fetchone()

    if resultado:
        dni, nombres, apellido_paterno = resultado
        personName = f"{nombres} {apellido_paterno}"
        print(f"Procesando trabajador: {personName}")

        # Crear el directorio si no existe
        personPath = os.path.join(ruta_imagen, dni)
        if not os.path.exists(personPath):
            os.makedirs(personPath)
            registrar_log(cursor, f"Carpeta creada: {personPath}",dni,'0')
        else:
            registrar_log(cursor, f"Carpeta existente: {personPath}",dni,'0')

        # Abrir la cámara
        cap = cv2.VideoCapture(0, cv2.CAP_DSHOW)
        if not cap.isOpened():
            raise RuntimeError("Error al abrir la cámara.")

        # Capturar rostros
        count = capturar_rostros(personPath, cap, total_foto)
        
        if count > 0:
            fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
            cursor.execute("""
                UPDATE maestro.trabajador 
                SET inicia_captura = '0', captura_finalizada = '1' , fecha_modificacion = %s 
                WHERE numero_dni = %s;
            """, (fecha_hora_actual, dni))
           
            cnxn.commit()

            registrar_log(cursor, f"Registro actualizado para {personName}.",dni,'0')

            cursor.execute("""
                UPDATE transaccional.seguimiento_log
                SET es_finalizado = '1' WHERE numero_dni = %s;
            """, (dni,))
            cnxn.commit()
            
            # Cerrar la cámara y ventanas de OpenCV
            cap.release()
            cv2.destroyAllWindows()
    else:
        registrar_log(cursor, "No se encontró ningún trabajador con inicia_captura = '1'.",dni,'0')

except Exception as e:
    print(f"Error durante la ejecución: {e}")
    registrar_log(cursor, f"Error: {str(e)}",dni,'0')
    
    fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    cursor.execute("""
        UPDATE maestro.trabajador 
        SET inicia_captura = '0', captura_finalizada = '0' , fecha_modificacion = %s 
        WHERE numero_dni = %s;
    """, (fecha_hora_actual, dni))
    
    cursor.execute("""
                UPDATE transaccional.seguimiento_log
                SET es_finalizado = '1' WHERE numero_dni = %s;
            """, (dni,))
    cnxn.commit()

finally:
       
    cursor.close()
    cnxn.close()

# input("Presiona Enter para finalizar...")
