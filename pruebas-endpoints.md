# Comprobación del adaptador HTTP: feature/hexagonal-api

## Alcance y estado

Esta rama adapta los ocho Controllers, los helpers de resultados HTTP, el manejador
centralizado de errores y su conexión en `Program.cs`. Los Controllers están en
`Lab05-Andrew-Alca/Adapters/Http/Controllers`; las validaciones de negocio continúan
en los Services existentes.

La referencia anterior al cambio es el commit `a8ae292`. Esta entrega usa las firmas
actuales de Services, Models y DTOs de autenticación. La integración de los nuevos
puertos/DTOs del núcleo y de los adaptadores de persistencia y seguridad queda
pendiente de las ramas de sus propietarios. Esta entrega por sí sola no completa
la adaptación hexagonal de toda la aplicación.

| Comprobación | Resultado |
|---|---|
| Rutas, verbos, firmas de acciones y atributos de autorización | Conservados mediante comparación estática con `a8ae292`: 8 Controllers, 39 acciones |
| Controllers sin condiciones, `try/catch` ni LINQ | Revisión estática realizada el 14 de octubre de 2026 |
| Traducción de creación de Matrículas | Trasladada al helper HTTP; conserva `400` y `201`, cuerpo y `Location` |
| Manejo de excepciones y orden de middleware | Código extraído del manejador anterior; conserva `401`, `400`, `500` y `{ "message": ... }` |
| Compilación y ejecución | Pendientes de salida del usuario |
| Comparación funcional de endpoints y autenticación | Pendiente; no se ejecutaron solicitudes ni operaciones de base de datos |
| SHA de la entrega y publicación/integración en develop | Pendientes |

## Preparación y ejecución

1. Usar la configuración local descrita en `README.md` y una base con datos de
   prueba acordados. Registrar qué configuración y datos se usan para comparar
   antes y después.
2. Confirmar un estudiante y un curso existentes, identificadores inexistentes y
   registros destinados a actualización/eliminación. No inventar identificadores.
3. El usuario ejecuta desde la raíz del repositorio:

```powershell
dotnet build Lab05-Andrew-Alca.sln
dotnet run --project Lab05-Andrew-Alca
```

4. Registrar las salidas y la URL local que indique la ejecución. Swagger se sirve
   en la raíz en Development. Para obtener la comparación anterior, ejecutar las
   mismas solicitudes contra la referencia `a8ae292` en un checkout separado y
   con la misma preparación de datos.
5. Obtener un token con `POST /api/Auth/login`. Las solicitudes autenticadas usan
   `Authorization: Bearer <TOKEN_LOGIN>`; en Swagger se pega solamente el token.

## CRUD de los siete recursos

Las bases de ruta son `/api/Estudiantes`, `/api/Cursos`, `/api/Profesores`,
`/api/Evaluaciones`, `/api/Asistencias`, `/api/Materias` y `/api/Matriculas`.
Todas requieren autenticación. Se conservan los cuerpos de las entidades actuales,
incluidas sus propiedades de navegación; esta rama no introduce DTOs CRUD nuevos.

| Solicitud por recurso | Preparación y cuerpo | Respuesta esperada según el código existente |
|---|---|---|
| `GET /api/{recurso}` | Token válido | `200`, colección JSON con la misma forma que antes |
| `GET /api/{recurso}/{id}` | ID existente y luego ID inexistente confirmado | `200` con el registro; `404` sin cuerpo para el inexistente |
| `POST /api/{recurso}` | JSON válido de prueba, `Content-Type: application/json`; guardar exactamente el cuerpo usado | `201`, registro creado y cabecera `Location` hacia su consulta por ID |
| `PUT /api/{recurso}/{id}` | JSON del registro de prueba; repetir con ID inexistente | `204` sin cuerpo; `404` sin cuerpo para el inexistente |
| `DELETE /api/{recurso}/{id}` | Registro de prueba destinado a eliminación; repetir con ID inexistente | `204` sin cuerpo; `404` sin cuerpo para el inexistente |
| Cualquier ruta CRUD sin token | Solicitud a la URL local confirmada | `401` |

## Creación de Matrículas

Enviar a `POST /api/Matriculas` con token válido y
`Content-Type: application/json`. Para el caso válido, sustituir los marcadores
por los IDs numéricos confirmados del fixture:

```text
{
  "idEstudiante": <ID_ESTUDIANTE_EXISTENTE>,
  "idCurso": <ID_CURSO_EXISTENTE>,
  "semestre": "2026-2"
}
```

| Caso | Cuerpo/variación | Respuesta esperada |
|---|---|---|
| Estudiante y curso existentes | Cuerpo anterior con IDs confirmados | `201`, Matrícula creada con la misma forma JSON y `Location` hacia `GET /api/Matriculas/{id}` |
| Estudiante ausente | `{ "idEstudiante": null, "idCurso": null, "semestre": "2026-2" }` | `400`, `{ "message": "El estudiante indicado no existe." }`, sin guardar |
| Estudiante inexistente | Sustituir `idEstudiante` por un ID inexistente confirmado | Mismo `400` y mensaje de estudiante, sin guardar |
| Curso ausente o inexistente | Estudiante existente y `idCurso` nulo o un ID inexistente confirmado | `400`, `{ "message": "El curso indicado no existe." }`, sin guardar |

Las comprobaciones de existencia siguen en `MatriculasService`; el helper HTTP
solo traduce el resultado del Service. Comparar tanto la respuesta como el efecto
de guardado usando los datos de prueba acordados.

## Autenticación y roles

| Solicitud | Preparación/cuerpo | Respuesta esperada |
|---|---|---|
| `POST /api/Auth/register` | `{ "username": "<USUARIO_PRUEBA_NUEVO>", "password": "<CLAVE_PRUEBA>", "rol": "Usuario" }` | `200` con `idUsuario`, `username` y `rol`; sin `passwordHash` |
| Registro repetido | Mismo username ya creado | `400`, `{ "message": "El nombre de usuario ya existe." }` |
| `POST /api/Auth/login` | `{ "username": "<USUARIO_PRUEBA_EXISTENTE>", "password": "<CLAVE_PRUEBA>" }` | `200`, `{ "success": true, "token": "..." }` |
| Login incorrecto | Usuario inexistente confirmado o clave incorrecta | `401`, `{ "message": "Usuario o contraseña incorrectos." }` |
| `GET /api/Auth/admin` | Token de Admin, luego token de Usuario, luego sin token | `200` con `{ "message": "Acceso permitido: rol Admin" }`; `403`; `401` |
| `GET /api/Auth/perfil` | Tokens de Admin y Usuario, luego sin token | `200` con el saludo del username autenticado; `401` sin token |

## Registro de evidencia

Por cada caso anotar fecha, autor, SHA o estado del árbol evaluado, configuración,
datos del fixture, método/ruta, cuerpo enviado, resultado esperado, estado HTTP,
cabeceras pertinentes, cuerpo obtenido y efecto de guardado. Omitir claves y tokens
de la evidencia compartida. Los resultados funcionales permanecen pendientes
hasta ejecutar y revisar estas solicitudes.
