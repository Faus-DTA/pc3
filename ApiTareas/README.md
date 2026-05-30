# ApiTareas - Evaluación PC03

Este proyecto es una Web API construida con **ASP.NET Core 8** que cumple con los requerimientos de la evaluación (Preguntas 1 a 4). Incluye operaciones CRUD con Entity Framework Core (SQLite), integración de APIs externas y Análisis de Sentimiento con ML.NET.

## Tecnologías y Paquetes
- **.NET 8**
- **Entity Framework Core (SQLite)**
- **Microsoft.ML** (Machine Learning para análisis de sentimiento)
- **IHttpClientFactory** (Consumo de APIs externas)

## Funcionalidades y Endpoints

### 1. Gestión de Tareas (CRUD Base - Pregunta 1)
- `GET /api/tareas` - Lista todas las tareas.
- `GET /api/tareas/{id}` - Obtiene el detalle de una tarea.
- `POST /api/tareas` - Crea una nueva tarea (Valida título, estado, prioridad y que la fecha de vencimiento sea futura).
- `PUT /api/tareas/{id}` - Actualiza una tarea existente.
- `DELETE /api/tareas/{id}` - Elimina una tarea por su ID.

### 2. Filtros de Búsqueda Avanzada (Pregunta 2)
Endpoint modificado con parámetros opcionales y validaciones:
- `GET /api/tareas?estado=Pendiente&prioridad=Alta&fechaInicio=2024-01-01&fechaFin=2024-12-31`
- Retorna `400 Bad Request` si la lógica de fechas es incorrecta o si los estados/prioridades no son válidos.

### 3. Consumo de API Externa (Pregunta 3)
Consulta a `https://jsonplaceholder.typicode.com/todos` mapeando los resultados a un DTO propio (`TareaExternaDto`):
- `GET /api/tareas-externas` - Retorna toda la lista de la API externa transformada.
- `GET /api/tareas-externas/{id}` - Retorna una sola tarea o un `404 Not Found` validado.

### 4. Machine Learning - Análisis de Sentimiento (Pregunta 4, Opción A)
Predicción de sentimientos a través de clasificación binaria (`SdcaLogisticRegression`) con un dataset precargado:
- `POST /api/ml/sentimiento`
  - **Body (JSON)**: `{ "comentario": "El sistema funciona bien" }`
  - **Respuesta (JSON)**: `{ "comentario": "El sistema funciona bien", "sentimiento": "Positivo" }`

## Instrucciones de Ejecución Local

1. Restaura los paquetes NuGet:
   ```bash
   dotnet restore
   ```

2. Aplica las migraciones a la base de datos (se generará un archivo `tareas.db` localmente):
   ```bash
   dotnet ef database update
   ```

3. Levanta el servidor:
   ```bash
   dotnet run
   ```

4. Puedes interactuar con la API a través de la ruta local generada (ej. `http://localhost:5000/swagger` si está configurado OpenAPI).
