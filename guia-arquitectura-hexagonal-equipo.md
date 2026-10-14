# Guía de adaptación hexagonal y colaboración de tres integrantes

**Objetivo:** reorganizar la funcionalidad existente mediante puertos y adaptadores, conservando su comportamiento. Tres integrantes trabajan en ramas propias nacidas de `develop`; se integran mediante merges, sin PR, sin eliminar ramas y con commits convencionales cuya descripción está en español.

**Estado:** propuesta de trabajo, no implementación realizada. Esta guía no autoriza ejecutar comandos, modificar código, acceder a la base de datos ni publicar cambios. El usuario ejecuta los comandos `dotnet` y confirma sus resultados.

## 1. Qué cambia y qué no cambia

- Cambia la dirección de dependencias y la separación de responsabilidades.
- Se conservan endpoints, rutas, contratos JSON, códigos HTTP, reglas existentes y autenticación.
- No se agregan funcionalidades, tablas, ejercicios LINQ, roles ni JWT nuevo.
- No se regenera scaffolding ni se modifican migraciones o esquema como parte de esta adaptación.
- Mover carpetas no basta: el núcleo debe dejar de depender de HTTP, EF Core y configuración técnica.

**Aclaración importante:** no agregar funcionalidades es compatible con refactorizar código. Si se exige literalmente cero cambios de código, solo puede documentarse la arquitectura propuesta; no puede afirmarse que la aplicación ya sea hexagonal.

La arquitectura de puertos y adaptadores aísla la aplicación de los dispositivos y tecnologías externos mediante interfaces y adaptadores. Referencia: [Alistair Cockburn, Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture).

## 2. Estado observado del proyecto

Inspección local del 13 de octubre de 2026 mediante CodeGraph y lectura puntual. Las referencias siguientes son ubicaciones relativas dentro del repositorio; no representan todavía la estructura propuesta.

| Evidencia actual | Implicación para la adaptación |
|---|---|
| `Lab05-Andrew-Alca/Lab05-Andrew-Alca.csproj:4`: `net10.0` | No cambiar de framework como parte de la arquitectura. |
| `Lab05-Andrew-Alca/Services/Implements/MatriculasService.cs:25-39`: valida existencia de estudiante y curso y guarda mediante UoW | Las validaciones y la coordinación continúan en el caso de uso. |
| `Lab05-Andrew-Alca/Controllers/MatriculasController.cs:36-43`: recibe la entidad y contiene `if` | Hay una separación pendiente; adaptar el resultado HTTP fuera del Controller. |
| `Lab05-Andrew-Alca/Repositories/Implements/UnitOfWork.cs:8-19`: depende de DbContext y llama `SaveChangesAsync` | Su implementación pertenece al adaptador de persistencia. |
| `Lab05-Andrew-Alca/Services/Implements/AuthService.cs:14-21,63-81`: usa configuración, hash y generación JWT | Separar esas dependencias técnicas mediante puertos sin cambiar autenticación. |
| `Lab05-Andrew-Alca/Program.cs:32-38,97-100`: registra dependencias y middleware | Mantenerlo como raíz de composición, con un solo responsable. |

Hay siete entidades de negocio y `Users`, ocho Controllers, interfaces y Services, tres DTOs de autenticación, repositorio genérico y UoW. La existencia de DTOs CRUD generales no está acreditada: los contratos de Matrículas observados exponen entidades. Antes de introducir DTOs en esas rutas, acordar la correspondencia que conserve el contrato público.

Solo se observaron referencias locales a `main` y `origin/main`. **Esto no demuestra que `develop` no exista en GitHub:** no se realizó `fetch` ni consulta remota. Se observó `origin` configurado para `https://github.com/AndrewMechDev/des-aplic-avnz-06.git`.

## 3. ¿Los Services siguen teniendo lógica?

**Sí. No se elimina la lógica de los Services ni se traslada a los Controllers.** Los Services de aplicación representan casos de uso: coordinan puertos, validan condiciones de la operación y solicitan persistencia.

| Elemento propuesto | Responsabilidad | Dependencias permitidas |
|---|---|---|
| Domain | Reglas puras y modelos de negocio cuando las reglas existentes lo justifiquen | Lenguaje y tipos propios, no HTTP ni EF |
| Application | Casos de uso/Services, contratos de entrada, DTOs y puertos de salida | Domain y contratos propios |
| Adaptador HTTP | Controllers, traducción de resultados y manejo centralizado de errores | Application y ASP.NET Core |
| Adaptador de persistencia | DbContext, repositorios, UoW, mapeos y consultas EF | Puertos del núcleo y EF Core |
| Adaptador de seguridad | Hash, emisión de token y configuración técnica existentes | Puertos del núcleo y bibliotecas técnicas |
| Raíz de composición | Ensamblar las implementaciones con DI en `Program.cs` | Núcleo y adaptadores |

Ejemplo existente: comprobar que un estudiante y un curso existan requiere consultas; corresponde al Service/caso de uso usando puertos. Una regla calculable sin persistencia puede vivir en Domain, si ya existe. No inventar reglas para llenar carpetas.

Los Controllers únicamente reciben la solicitud, llaman al caso de uso y delegan la traducción HTTP. **No contienen lógica de negocio, `if`, `switch`, ternarios, `try/catch` ni LINQ.** Helpers/presentadores HTTP y middleware pueden contener decisiones de transporte; nunca reglas de negocio.

### Contratos y datos

- Las interfaces de Service pueden actuar como puertos de entrada después de eliminar tipos de infraestructura de sus contratos.
- `IUnitOfWork` e `IGenericRepository` pueden ser puertos de salida si sus tipos son independientes de EF. No basta con mover sus archivos si aún exponen modelos de persistencia acoplados.
- La implementación `UnitOfWork` y `GenericRepository` permanece en infraestructura.
- No devolver `DbSet`, DbContext ni `IQueryable` ligado a EF desde el núcleo.
- Los modelos generados para persistencia no se convierten automáticamente en Domain por cambiar de carpeta. Mantenerlos separados cuando su acoplamiento lo requiera; agregar modelos puros y mapeos solo donde sean necesarios.
- DTOs siguen siendo obligatorios como frontera de aplicación, también sin JWT. Preservar nombres, tipos, nullability y forma JSON cuando se sustituya una entidad expuesta.
- LINQ traducido a SQL pertenece al adaptador de persistencia. LINQ sobre datos en memoria no ligado a un proveedor puede utilizarse en el núcleo cuando corresponda.
- Auth conserva sus casos de uso; hash y emisión JWT pasan detrás de puertos. No agregar ni retirar JWT sin acuerdo explícito.

### Estructura propuesta, no creada

```text
Lab05-Andrew-Alca/
  Domain/
  Application/
    DTOs/
    Ports/Input/
    Ports/Output/
    Services/
  Infrastructure/
    Persistence/
      Models/
      Repositories/
    Security/
  Adapters/Http/
    Controllers/
    Extensions/
    Middleware/
  Program.cs
```

La dirección conceptual es **adaptadores → Application → Domain**. Application declara los puertos; infraestructura los implementa; DI conecta ambos. La ejecución puede llamar al adaptador a través de una interfaz sin que el núcleo importe su implementación.

Para este laboratorio puede mantenerse un proyecto con carpetas y revisión de dependencias. Esto no impone límites de compilación. Proyectos separados los hacen exigibles mediante referencias, pero añaden configuración: es una decisión opcional a confirmar, no un requisito que se implementa automáticamente.

## 4. Tres integrantes, tres ramas y propietarios claros

| Integrante | Rama propuesta desde develop | Propiedad del trabajo |
|---|---|---|
| Integrante 1 | `feature/hexagonal-nucleo` | Domain necesario, Application, interfaces/puertos, DTOs y Services de aplicación |
| Integrante 2 | `feature/hexagonal-persistencia` | Adaptadores EF, repositorios/UoW y adaptadores técnicos de seguridad |
| Integrante 3 | `feature/hexagonal-api` | Controllers, helpers HTTP, middleware, `Program.cs` e integración del equipo |

Mapeo inicial: Integrante 1 adapta `Services/*.cs`, `Services/Implements/*.cs` y `DTOs/*.cs`; Integrante 2 adapta `Data/ApplicationDbContext.cs`, `Repositories/Implements/*.cs` y ubicación de modelos de persistencia; Integrante 3 adapta `Controllers/*.cs`, `Extensions/ActionResultExtensions.cs` y `Program.cs`. Estas rutas están bajo `Lab05-Andrew-Alca/`.

**Interfaces de repositorio/UoW:** el Integrante 1 es propietario de `Repositories/IUnitOfWork.cs` e `IGenericRepository.cs` al convertirlas en contratos del núcleo. El Integrante 2 las consume, no las modifica simultáneamente. En Auth, el Integrante 1 extrae/declara contratos y el Integrante 2 implementa los adaptadores acordados, sin editar ambos el mismo Service al mismo tiempo.

Antes del trabajo paralelo, acordar firmas, resultados, DTOs y mapeos. Integrar primero el contrato coherente del núcleo en `develop`; después las otras dos ramas incorporan ese avance. Los adaptadores se conectan únicamente cuando sus contratos están disponibles. Cada handoff debe dejar una unidad compilable; no se fusionan cambios incompletos para desbloquear a otra persona.

`Program.cs` tiene un único escritor: Integrante 3. Cualquier ajuste compartido se solicita a su propietario. No regenerar modelos ni duplicar DTOs/puertos entre ramas.

## 5. Gitflow académico, sin PR y conservando ramas

```text
main                 permanece sin cambios
  develop            integración revisada del equipo
    feature/hexagonal-nucleo
    feature/hexagonal-persistencia
    feature/hexagonal-api
```

Es una adaptación del flujo de features de Gitflow; no se están creando releases ni hotfixes. Se recomienda un clon por integrante. No cambiar ramas en un checkout compartido con trabajo ajeno o cambios sin guardar.

### Preparación condicional

Los comandos son instrucciones para ejecución posterior, **no resultados obtenidos**. Antes de operaciones remotas se requiere autorización explícita para `origin`, operación, ramas y sesión/credencial que usará el usuario. La configuración del remoto no concede permiso para utilizarlo.

```powershell
# Solo lectura local
 git status --short
 git remote -v
 git branch -a
```

Si hay cambios pendientes, detenerse y conservarlos. Verificar especialmente archivos no rastreados: no agregarlos ni descartarlos automáticamente.

Una vez autorizado el fetch al destino confirmado:

```powershell
git fetch origin
git branch -a
```

- Si `develop` ya existe localmente, usarla y comprobar su seguimiento.
- Si solo existe `origin/develop`, crear el seguimiento local con `git switch --track origin/develop`.
- Si no existe ninguna, el responsable del equipo debe acordar la base aprobada y crear `develop` desde esa referencia. No asumir `main` ni reiniciar ramas.
- Si se confirma expresamente que la base es `main`: `git switch -c develop main`. Publicarla con `git push -u origin develop` solo con autorización remota.

### Crear una rama propia, una sola vez

Con `develop` existente, limpio y configurado para seguir el remoto:

```powershell
git switch develop
# Solo con autorización remota
git pull --ff-only origin develop
# Cada integrante ejecuta únicamente la línea de su rama
git switch -c feature/hexagonal-nucleo develop
# Integrante 2: git switch -c feature/hexagonal-persistencia develop
# Integrante 3: git switch -c feature/hexagonal-api develop
```

Si una rama ya existe, usar `git switch <rama>`; no recrearla. Si `--ff-only` falla, investigar la divergencia, sin reset ni force push.

### Commits auténticos y por unidad de trabajo

Configurar identidad local con el nombre y correo reales del integrante; no copiar los de otra persona. Formato: `tipo(alcance): descripción en español`.

Ejemplos propuestos:

```text
refactor(application): desacopla matrícula mediante puertos
refactor(infrastructure): implementa persistencia contra contratos del núcleo
refactor(api): delega respuestas HTTP sin condicionales en controladores
test(api): verifica contratos existentes después de la adaptación
docs(arquitectura): registra responsabilidades y evidencia del equipo
```

Cada commit entrega una unidad coherente, con comprobaciones y documentación pertinente. No separar artificialmente por tipos de archivos ni inflar commits para aparentar aportes.

```powershell
git diff --stat
# Ejemplo: sustituir por rutas exactas que pertenecen a la unidad revisada
git add 'ruta/exacta/archivo.cs' 'ruta/exacta/prueba.cs'
git diff --cached --stat
git diff --cached
git commit -m 'refactor(application): desacopla matrícula mediante puertos'
# Solo con autorización para publicar la rama propia
git push -u origin feature/hexagonal-nucleo
```

No usar `git add .`, squash, rebase de historia compartida, amend, force push ni atribución artificial. No agregar `Co-Authored-By` o atribución a IA.

### Sincronización e integración

Con el checkout limpio, actualizar `develop` mediante `--ff-only` y volver a la feature propia. Incorporar `develop` con merge, conservando historia:

```powershell
git switch feature/hexagonal-persistencia
git merge develop -m 'chore(integracion): incorpora contratos acordados desde develop'
```

Si hay conflictos, detenerse y resolverlos con los propietarios; revisar el diff completo y repetir comprobaciones. No descartar cambios ajenos ni fusionar marcadores pendientes.

El Integrante 3 coordina las integraciones en `develop`, después de revisar evidencias y recibir confirmación del equipo:

```powershell
git switch develop
# Solo con autorización remota y seguimiento correcto
git pull --ff-only origin develop
# Una rama validada cada vez; no ejecutar las tres a ciegas
git merge --no-ff feature/hexagonal-nucleo -m 'chore(integracion): integra núcleo hexagonal en develop'
# Después de comprobar: persistencia y, finalmente, API
# Solo con autorización para publicar develop
git push origin develop
```

Si el integrador trabaja en otro clon, primero debe obtener las ramas publicadas mediante fetch autorizado y seleccionar las referencias comprobadas (`origin/feature/...`); los nombres locales del ejemplo no existen automáticamente en todos los clones.

**Sin PR, sin eliminación de ramas, sin cambios en main.** Evitar comandos automáticos de finalización Gitflow que eliminen ramas. Los merges `--no-ff` conservan commits individuales y puntos de integración.

## 6. Ejecución en seis fases con autorización

| Fase | Resultado esperado | Responsable y comprobación |
|---|---|---|
| 1. Contexto y contratos | Base aprobada, ramas, propietarios, contratos de puertos/DTOs y línea base HTTP | Equipo; confirmar proyecto/BD actual, endpoints y estado Git |
| 2. Núcleo | Casos de uso y contratos desacoplados, sin nuevas funciones | Integrante 1; unidad compilable y prueba de comportamiento |
| 3. Adaptadores de salida | Persistencia y seguridad implementan los contratos acordados | Integrante 2; comprobar mapeos, guardado y autenticación |
| 4. Adaptador HTTP y DI | Controllers delgados, traducción de resultados y composición | Integrante 3; comprobar rutas y ausencia de lógica prohibida |
| 5. Regresión funcional | Contratos previos preservados y fallos documentados | Equipo; usuario ejecuta build/run/pruebas, sin inventar éxitos |
| 6. Integración y evidencias | Merges verificados en develop y aportes trazables | Integrante 3 coordina; repetir comprobaciones tras cada integración |

Después de las fases 1 a 5 preguntar: **«¿Autorizas continuar con la siguiente fase? Responde sí o no.»** Detenerse hasta la respuesta. Esa confirmación no concede automáticamente acceso remoto, a BD ni permiso para cambiar el alcance.

Al finalizar: **«¿Quieres que revisemos algún punto adicional antes de cerrar?»** Detenerse. No generar entregables adicionales sin el acuerdo correspondiente.

## 7. Pruebas y evidencia para el profesor

La compilación no acredita que los endpoints funcionen. Comparar antes/después con datos de prueba reales, identificadores confirmados y la misma configuración. No probar escritura sobre una BD compartida sin autorización y datos destinados a pruebas.

| Área existente | Comprobación requerida | Estado inicial |
|---|---|---|
| CRUD de las siete entidades | Listado, ID existente/inexistente, creación, actualización y eliminación | Pendiente de ejecución |
| Matrículas | Estudiante/curso válido e inexistente; mismo error y efecto de guardado | Pendiente de ejecución |
| Auth | Registro/login, credenciales incorrectas y no exposición del hash | Pendiente de ejecución |
| Seguridad | Sin token, token válido y política de rol permitida/denegada | Pendiente de ejecución |
| Arquitectura | Núcleo sin EF/HTTP/configuración; Controllers sin condiciones ni LINQ | Pendiente de refactorización |

Registrar solicitud, datos/fixture, respuesta esperada, respuesta obtenida, fecha, autor y SHA evaluado. No inventar IDs ni resultados. Con tests deterministas relevantes, observar su fallo inicial antes del cambio y éxito posterior; si no existen, documentar la excepción y comparar la línea base funcional.

El usuario ejecuta `dotnet build`, `dotnet run` y `dotnet test` cuando exista un proyecto de pruebas aplicable. No afirmar pruebas realizadas hasta recibir y revisar sus salidas. Para endpoints se acuerda la sesión local activa; una prueba contra destino remoto requiere autorización específica.

Al cierre de la implementación futura, entregar `pruebas-endpoints.md` con pasos reproducibles, métodos/rutas reales, cuerpos y cabeceras, preparación de datos, respuestas esperadas y resultados observados o pendientes. **Ese archivo no se crea todavía con esta guía.**

### Trazabilidad individual

```powershell
git log --all --graph --decorate --oneline
git log develop --no-merges --format='%h | %an | %ae | %s'
git shortlog -sne develop --no-merges
# Sustituir por identidad real comprobada
git log develop --author='Nombre real' --stat
```

| Integrante | Rama | SHA | Objetivo del commit | Archivos propios | Comprobación y evidencia |
|---|---|---|---|---|---|
| Integrante 1 | feature/hexagonal-nucleo | Pendiente | Pendiente | Pendiente | Pendiente |
| Integrante 2 | feature/hexagonal-persistencia | Pendiente | Pendiente | Pendiente | Pendiente |
| Integrante 3 | feature/hexagonal-api | Pendiente | Pendiente | Pendiente | Pendiente |

El número de commits y líneas no mide por sí solo calidad o igualdad del aporte. Evaluar responsabilidades, decisiones, cambios útiles y pruebas. Cada persona debe crear sus propios commits; el integrador conserva su autoría y registra merges, no vuelve a crear el trabajo de todos bajo su nombre.

## 8. Criterios de cierre

- [ ] Contratos y límites acordados antes del trabajo dependiente.
- [ ] Services conservan casos de uso, sin depender de implementaciones técnicas.
- [ ] Controllers no contienen lógica, condiciones, `try/catch` ni LINQ.
- [ ] DTOs preservan el contrato público y no exponen datos sensibles.
- [ ] EF, UoW concreto, hash y JWT concreto quedan en adaptadores.
- [ ] Funcionalidad y esquema existentes se conservan.
- [ ] Usuario informa resultados de build y regresión; pendientes visibles.
- [ ] Develop conserva commits auténticos y merges de los tres integrantes.
- [ ] No se crean PR, no se eliminan ramas y main permanece intacto.
- [ ] Evidencias y guía de pruebas permiten repetir la validación.

**Siguiente paso:** acordar nombres reales, base de `develop` y alcance exacto del proyecto con el equipo antes de comenzar la fase 1. La creación de esta guía no inicia la refactorización.

