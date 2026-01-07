# InfoWise - News Management Service

**NewsManagementService** es un microservicio backend desarrollado en .NET 8 encargado de la gestión, almacenamiento y distribución de resúmenes de noticias. Este servicio actúa como un componente central que integra el procesamiento de datos proveniente de flujos de trabajo de **n8n** (impulsados por IA/Gemini) y gestiona las preferencias de los usuarios para la entrega de contenido personalizado.

## Tecnologías Utilizadas

El proyecto utiliza el siguiente stack tecnológico:

* **Framework Principal:** .NET 8.0 (ASP.NET Core Web API)
* **Base de Datos:** PostgreSQL (con Npgsql y Entity Framework Core)
* **Mensajería Asíncrona:** RabbitMQ
* **Documentación de API:** Swagger / OpenAPI
* **Contenerización:** Docker Support

## Prerrequisitos

Antes de iniciar, asegúrate de tener instalado:

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [PostgreSQL](https://www.postgresql.org/)
* [RabbitMQ](https://www.rabbitmq.com/) (o correrlo vía Docker)

## Configuración

El servicio requiere configurar la cadena de conexión a la base de datos y las credenciales de RabbitMQ.

1. Navega al archivo `appsettings.json` (o `appsettings.Development.json`) y actualiza las siguientes secciones:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=NewsServiceDatabase;Username=tu_usuario;Password=tu_contraseña"
  },
  "RabbitMQConfiguration": {
    "HostName": "localhost",
    "Port": 5672,
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest"
  }
}

```

## Instalación y Ejecución

1. **Clonar el repositorio:**
```bash
git clone https://github.com/arieloou/infowise-newsmanagementservice.git
cd InfoWise-NewsManagementService/NewsManagementService

```


2. **Restaurar dependencias:**
```bash
dotnet restore

```


3. **Base de Datos:**
El proyecto utiliza Entity Framework Core. Al iniciar la aplicación, esta intentará ejecutar las migraciones pendientes y sembrar datos iniciales automáticamente (`DbInitializer`).
Si necesitas crear las migraciones manualmente:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update

```


4. **Ejecutar la aplicación:**
```bash
dotnet run

```


El servicio estará disponible por defecto en los puertos configurados en `launchSettings.json`.

## API Endpoints

La documentación completa de la API está disponible vía Swagger en `/swagger` cuando se ejecuta en entorno de desarrollo.

### Controlador: News Summary

| Método | Endpoint | Descripción |
| --- | --- | --- |
| `GET` | `/news-management/health` | Verifica el estado de salud del servicio. |
| `GET` | `/news-management/categories/all` | Obtiene todas las categorías de noticias disponibles. |
| `GET` | `/news-management/summaries/user/{userId}` | Obtiene los resúmenes de noticias personalizados para un usuario específico. |

## Integración con RabbitMQ

El servicio cuenta con `BackgroundServices` que consumen eventos de colas de RabbitMQ para procesar información de manera asíncrona:

* **N8NEventsConsumer**: Escucha en la cola `news.summary-processed.queue`. Procesa los resúmenes de noticias generados por la IA (Gemini) desde n8n y los guarda en la base de datos.
* **UserPreferencesConsumer**: Gestiona la sincronización de preferencias de usuario.

## Docker

El proyecto incluye soporte para Docker. Para construir la imagen:

```bash
docker build -t news-management-service .

```

## Estructura del Proyecto

* **Application**: Lógica de negocio (`NewsAppService`).
* **Controllers**: Endpoints de la API (`NewsSummaryController`).
* **Infrastructure**: Acceso a datos, repositorios, configuraciones de RabbitMQ y DB Context.
* **Interfaces**: Definiciones de contratos para repositorios y servicios.
* **Models**: Entidades de dominio (`NewsSummary`, `NewsCategory`, etc.).

---

## License
This project is distributed under the MIT License, as specified in the [LICENSE](LICENSE) file.
