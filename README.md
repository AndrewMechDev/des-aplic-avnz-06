# des-aplic-avnz-06

API .NET 10 (Web API) con Repository genérico + UnitOfWork genérico, autenticación JWT y
autorización por roles. Dominio: Colegio (Estudiantes, Cursos, Profesores, Matriculas,
Evaluaciones, Asistencias, Materias).

## Configuración local (para clonar y correr el proyecto)

Este repo **no incluye** `appsettings.json`, `db.sql` ni `insert.sql` (quedan fuera de
control de versiones a propósito, porque `appsettings.json` tiene credenciales reales de
base de datos y JWT). Para levantar el proyecto:

### 1. Base de datos

Tenés dos opciones:

**Opción A — Usar la misma base de Neon del equipo**
Pedile a quien administra el proyecto que te pase por privado (no por GitHub) el
`ConnectionStrings:DefaultConnection` ya armado.

**Opción B — Usar tu propia base (Neon u otro Postgres)**
1. Creá una base Postgres nueva (por ejemplo, un proyecto gratis en [neon.tech](https://neon.tech)).
2. Pedí los archivos `db.sql` e `insert.sql` a quien administra el proyecto (no están en el
   repo) y ejecutalos en el editor SQL de tu base, en ese orden, para crear las tablas y
   cargar datos de prueba.
3. Copiá la connection string de tu propia base (Neon te la da armada en su dashboard).

### 2. `appsettings.json`

1. Copiá `Lab05-Andrew-Alca/appsettings.Example.json` y renombralo a
   `Lab05-Andrew-Alca/appsettings.json`.
2. Completá:
   - `ConnectionStrings:DefaultConnection` con la connection string del paso 1 (la del
     equipo o la tuya propia).
   - `Jwt:SecretKey` con una clave propia (cualquier string largo, no hace falta que
     coincida con la de nadie más — pero ojo: si tu compañero genera un token con SU clave y
     vos corrés la API con OTRA clave, la validación del token va a fallar. Si van a probar
     login entre varios, usen la MISMA clave).
   - `Jwt:Issuer` / `Jwt:Audience` podés dejarlos como están en el ejemplo.
3. **Nunca** hagas `git add` de tu `appsettings.json` — ya está en `.gitignore`, pero prestá
   atención si alguna vez lo forzás con `git add -f` por error.

### 3. Paquetes NuGet

`dotnet restore` (o Rider lo hace solo al abrir la solución). Todos los paquetes están
fijados en la familia `10.x`, acorde a `net10.0` — no actualizar ninguno a `11.x`.

### 4. Correr el proyecto

```bash
dotnet run --project Lab05-Andrew-Alca
```

Swagger queda disponible en la raíz: `http://localhost:5265/`.

## Autenticación

- `POST /api/Auth/register` — crea un usuario (`username`, `password`, `rol`: `Admin` o
  `Usuario`).
- `POST /api/Auth/login` — devuelve un JWT.
- El resto de los endpoints requieren `[Authorize]` — usá el botón "Authorize" en Swagger con
  el token obtenido en el login (pegar SOLO el token, sin la palabra `Bearer` ni comillas).
