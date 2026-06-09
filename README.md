# WorldCupPool
Aplicacion full stack para administrar un pool de predicciones del Mundial.


## Levantar el proyecto
Requisito:

- Docker

Desde la raiz del proyecto:

```bash
docker compose up
```

Con eso se levantan:

- `frontend` en `http://localhost`
- `backend` en `http://localhost:8080`
- `mysql` en `localhost:3306`


## Estructura
El proyecto esta dividido en tres partes principales:

- `Frontend/`: aplicacion Angular
- `Backend/`: API en ASP.NET Core con capas `Api`, `Application`, `Domain`, `Infrastructure` y `Tests`
- `Aws/`: task definitions y archivos de despliegue para ECS/Fargate


## Notas
- No se agrego una suite completa de tests para todo el proyecto; solo hay pruebas generales para las funcionalidades principales.
- Los datos iniciales del sistema se cargan por medio de una semilla al arrancar la aplicacion.
- Dado que el alcance del proyecto se concentro en una solucion full stack completa, incluyendo frontend, backend, despliegue y documentacion, no se priorizo una vista mobile dedicada.

## Usuario de prueba
- Admin: usuario `Admin` - contraseña `Admin123`
- User: usuario `User` - contraseña `User1234`
