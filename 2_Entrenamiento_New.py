import psycopg2
import os
import numpy as np
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
        INSERT INTO transaccional.seguimiento_log (usuario, fecha_registro, comentario, numero_dni, es_finalizado) 
        VALUES (%s, %s, %s, %s, %s);
    """
    cursor.execute(query_log, ('python', fecha_hora_actual, comentario, numero_dni, es_finalizado))
    cnxn.commit()


# Conexión a la base de datos PostgreSQL
# try:
#     cnxn = psycopg2.connect(
#         host="11.35.92.89",  # Cambia según tu configuración
#         port="5432",       # El puerto por defecto de PostgreSQL
#         database="db_asistencia", # El nombre de tu base de datos
#         user="postgres", # Tu usuario de PostgreSQL
#         password="12345678"  # Tu contraseña de PostgreSQL
#     )
#     cursor = cnxn.cursor()
# except Exception as e:
#     print(f"Error conectando a la base de datos: {e}")
#     exit()

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

except Exception as e:
    print(f"Error conectando a la base de datos: {e}")
    exit()

# Directorio donde se encuentran los datos de las personas
#directorio = os.getcwd()
#dataPath = os.path.join(directorio, "data")
#peopleList = os.listdir(dataPath)

dataPath = ruta_imagen 
peopleList = os.listdir(ruta_imagen)

print('Lista de personas: ', peopleList)

# Listas para guardar los encodings y los nombres correspondientes
known_encodings = []
known_names = []

# Recorrer cada carpeta de persona
for personName in peopleList:
    personPath = os.path.join(dataPath, personName)
    print(f'Leyendo imágenes de: {personName}')
    registrar_log(cursor, f"Leyendo imágenes {personName}.","python","")

    # Recorrer las imágenes dentro de la carpeta de cada persona
    for fileName in os.listdir(personPath):
        
        imagePath = os.path.join(personPath, fileName)
        print(f'Procesando imagen: {imagePath}')

        # Verificar si el archivo tiene extensión .jpg
        if fileName.lower().endswith('.jpg'):       
        
            # Cargar la imagen y convertirla de BGR a RGB
            image = face_recognition.load_image_file(imagePath)
            encodings = face_recognition.face_encodings(image)
    
            # Si la imagen contiene un rostro
            if len(encodings) > 0:
                # Tomar el primer encoding (en caso de que haya múltiples rostros en la imagen)
                encoding = encodings[0]
    
                # Guardar el encoding y el nombre de la persona
                known_encodings.append(encoding)
                known_names.append(personName)
    
    fecha_hora_actual = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    cursor.execute("""
        UPDATE maestro.trabajador 
        SET captura_finalizada = '0' , fecha_modificacion = %s 
        WHERE numero_dni = %s;
    """, (fecha_hora_actual, personName))
   
    cnxn.commit()

# Convertir las listas a arrays de NumPy para almacenarlas fácilmente
known_encodings_array = np.array(known_encodings)
known_names_array = np.array(known_names)

# Guardar los encodings y los nombres en archivos .npy
np.save('encodings.npy', known_encodings_array)
np.save('names.npy', known_names_array)

registrar_log(cursor, "Entrenamiento completado. Encodings y nombres guardados en archivos.","python","")
print('Entrenamiento completado. Encodings y nombres guardados en archivos.')


# In[ ]:




