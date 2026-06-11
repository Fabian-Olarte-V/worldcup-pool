# AI_LOG.md

## Resumen

Durante el desarrollo del sistema utilicé IA como asistente técnico para acelerar el análisis inicial, validar decisiones de arquitectura, generar propuestas base de modelado, apoyar la configuración de entidades y relaciones, y mejorar la productividad durante la implementación.

La IA también se usó durante el desarrollo de funcionalidades. En varios casos, se tomaron funcionalidades ya implementadas como base y se usaron prompts para generar nuevas funcionalidades similares, ajustando nombres, flujos, modelos y reglas específicas. Esto permitió mantener consistencia entre módulos y reducir tiempo en tareas repetitivas de implementación.

Los siguientes prompts son versiones consolidadas de conversaciones iterativas. No representan necesariamente un único mensaje exacto, sino los principales bloques de trabajo donde la IA fue utilizada como apoyo técnico.

---

## Prompt 1: Evaluación de Arquitectura y Estructura Inicial

### Contexto

Se necesitaba definir una arquitectura adecuada para una aplicación fullstack de predicciones mundialistas. El sistema incluía autenticación, roles, partidos precargados, predicciones de usuarios, carga de resultados por parte del administrador, cálculo de puntos, leaderboard e historial de predicciones.

Inicialmente se evaluaron alternativas simples, tanto en backend como en frontend. Para backend, una opción era mantener controllers, services, entities y acceso a datos dentro de un mismo proyecto con una separación básica por carpetas. Para frontend, una alternativa era usar una estructura general con carpetas como pages, components y services, sin separación por features.

Como el sistema debía poder crecer con nuevas funcionalidades, también se evaluaron alternativas con mayor separación de responsabilidades.

### Prompt

```txt
Estoy desarrollando una aplicación fullstack de predicciones mundialistas.

El sistema incluye:
- autenticación de usuarios;
- roles User y Admin;
- partidos precargados;
- predicciones de usuarios;
- carga de resultados finales por parte del administrador;
- cálculo de puntos;
- leaderboard;
- historial de predicciones.

Necesito evaluar qué arquitectura usar para que el proyecto sea mantenible y pueda crecer sin agregar complejidad innecesaria.

Quiero comparar estas opciones:

Backend:
1. Una estructura simple en ASP.NET Core donde controllers, services, entities y data access viven dentro de un mismo proyecto.
2. Layered Architecture separando API, Application, Domain e Infrastructure.
3. Clean Architecture.
4. CQRS o MediatR.

Frontend:
1. Una estructura simple con carpetas generales como pages, components y services.
2. Una arquitectura basada en features, core y shared.

Restricciones:
- El sistema es pequeño a mediano.
- La aplicación es principalmente CRUD.
- Se busca mantenibilidad y escalabilidad.
- Se debe evitar sobreingeniería.
- La arquitectura debe ser fácil de explicar y mantener.

Resultado esperado:
- Comparar las alternativas.
- Recomendar una arquitectura para backend.
- Recomendar una arquitectura para frontend.
- Proponer una estructura inicial de carpetas para ambos proyectos.
```

### Cómo ayudó la IA

La IA ayudó a comparar una estructura simple contra alternativas con mejor separación de responsabilidades. También permitió validar que Clean Architecture, CQRS y MediatR podían agregar más complejidad de la necesaria para el alcance del sistema.

Durante la iteración se concluyó que, aunque el sistema no era altamente complejo, una arquitectura más organizada ofrecía una mejor base para escalar y mantener el código.

### Decisión final

Se eligió:

* **Layered Architecture** para el backend.
* **Feature-Based Architecture with Core/Shared Structure** para el frontend.

Esta decisión permitió separar responsabilidades de forma clara:

* En backend: entrada HTTP, casos de uso, reglas de negocio e infraestructura.
* En frontend: funcionalidades de negocio, lógica transversal y componentes reutilizables.

---

## Prompt 2: Modelo de Datos, Entidades y Configuración con EF Core

### Contexto

Después de definir la arquitectura, se necesitaba diseñar el modelo principal del sistema. El modelo debía representar usuarios, equipos, partidos, predicciones y resultados reales. También era necesario definir las relaciones entre entidades y ubicar correctamente la lógica de puntuación.

La IA fue utilizada para generar una propuesta inicial de SQL con entidades, propiedades y relaciones. A partir de ese esquema, se usó como apoyo para generar la capa de dominio, las entidades principales y la configuración de EF Core.

### Prompt

```txt
Estoy construyendo el modelo de datos para una aplicación de predicciones mundialistas usando ASP.NET Core, EF Core Code First y MySQL.

El sistema necesita manejar:
- usuarios;
- roles User y Admin;
- equipos de fútbol;
- partidos precargados;
- predicciones por usuario;
- resultados finales;
- cálculo de puntos;
- leaderboard;
- historial de predicciones.

Necesito una propuesta inicial de modelo relacional y entidades de dominio.

Restricciones:
- Mantener el modelo simple.
- Evitar entidades innecesarias.
- Usar una base de datos relacional.
- Usar EF Core Code First.

Resultado esperado:
- Proponer un SQL o esquema inicial con tablas, propiedades y relaciones.
- Sugerir las entidades principales del dominio.
- Definir relaciones entre usuarios, equipos, partidos, predicciones y resultados.
```

### Cómo ayudó la IA

La IA ayudó a generar una propuesta inicial del esquema de base de datos, incluyendo tablas, propiedades y relaciones principales. Con base en ese esquema se generaron las entidades del dominio y configuraciones de EF Core.

También ayudó a validar que el modelo debía mantenerse simple, evitando entidades adicionales que no aportaban valor para el alcance actual.

### Decisión final

El modelo final se basó en las siguientes entidades:

* `AppUser`
* `SoccerTeam`
* `Match`
* `Prediction`
* `MatchResult`

---

## Prompt 3: Productividad en Backend usando Casos de Uso Existentes como Referencia

### Contexto

Durante el desarrollo de la app, la IA se utilizó para acelerar el desarrollo de nuevas funcionalidades tomando como referencia casos de uso ya implementados.

Por ejemplo en backend, como la aplicación es principalmente CRUD, varias funcionalidades seguían una estructura similar: controller, service o caso de uso, DTOs, repositorios, consultas con EF Core y respuesta hacia el frontend. La diferencia principal estaba en las entidades, tablas, filtros o reglas específicas de cada flujo.

Por esta razón, se usó IA para analizar funcionalidades existentes y generar nuevas funcionalidades respetando la misma arquitectura por capas.

### Prompt

```txt
Estoy trabajando en el backend de una aplicación de predicciones mundialistas usando ASP.NET Core con Layered Architecture.

El backend está organizado en capas:
- API: controllers y endpoints HTTP.
- Application: servicios/casos de uso y DTOs.
- Domain: entidades y reglas de negocio.
- Infrastructure: repositorios, EF Core y acceso a base de datos.

Ya existe una funcionalidad implementada que puedo usar como referencia. Quiero crear una nueva funcionalidad siguiendo el mismo patrón, sin romper la estructura actual del proyecto.

Funcionalidad nueva:
Crear el caso de uso para obtener el historial de predicciones de un usuario.

Contexto funcional:
- El sistema tiene usuarios, partidos, equipos, predicciones y resultados.
- El historial debe mostrar las predicciones realizadas por un usuario.
- La consulta debe respetar la estructura existente del backend.
- La funcionalidad debe ubicarse en las capas y carpetas correspondientes.

Restricciones:
- No cambiar funcionalidades existentes.
- No mezclar lógica HTTP con lógica de aplicación.
- No acceder directamente a EF Core desde el controller.
- Mantener el mismo estilo de código del proyecto.
- Reutilizar patrones existentes de DTOs, servicios, repositorios y respuestas.
- Mantener la lógica de negocio en backend.

Resultado esperado:
- Identificar qué archivos nuevos o modificados son necesarios.
- Proponer el endpoint correspondiente.
- Crear los DTOs necesarios.
- Crear el método en la capa de aplicación.
- Crear o extender el repositorio correspondiente.
- Ubicar cada archivo en la capa correcta.
- Seguir el mismo flujo de las funcionalidades existentes.
```

### Cómo ayudó la IA

La IA ayudó a entender la infraestructura existente del backend y a generar una nueva funcionalidad siguiendo el mismo patrón de casos de uso ya implementados.

Al tomar como referencia una funcionalidad previa, la IA pudo replicar la estructura necesaria para el historial de predicciones, ubicando los nuevos archivos en las capas correctas y manteniendo consistencia con el resto del proyecto.

Esto fue útil porque muchas funcionalidades del sistema comparten un flujo similar: recibir una petición desde un controller, delegar la lógica a la capa de aplicación, consultar datos mediante infraestructura/repositorios, mapear la información a DTOs y retornar una respuesta al frontend.

### Decisión final

La IA se utilizó como apoyo para acelerar la creación del caso de uso de historial de predicciones. La funcionalidad generada fue revisada, ajustada e integrada manualmente para asegurar que respetara la arquitectura del backend, las reglas del sistema y el estilo del código existente.
