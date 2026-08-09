# FutLiga API (.NET) — Revisión técnica y correcciones

## Contexto
El backend ya viene con Clean Architecture bien planteada (Domain / Application
/ Infrastructure / Persistence / Api), CQRS con MediatR, EF Core con
migraciones, y un `FutLiga.ModuleGenerator` propio (herramienta con Roslyn que
genera módulos completos — entidad, repositorio, comandos, queries,
controlador — a partir de una definición). Es, con diferencia, la parte más
madura de todo el proyecto. Aun así se encontraron varios bugs serios,
algunos de seguridad.

## 🔴 Seguridad — atención inmediata

1. **La contraseña real de la base de datos de producción venía en texto
   plano dentro de `appsettings.json`**, un archivo pensado para ir a
   control de versiones. Se reemplazó por un placeholder. **Debes rotar
   esa contraseña ahora** (cambiarla directamente en el proveedor del SQL
   Server) y mover tanto la cadena de conexión como el `Jwt:SecretKey` a
   `dotnet user-secrets` en desarrollo, y a variables de entorno o un
   almacén de secretos (Azure Key Vault, AWS Secrets Manager, etc.) en
   producción. Nunca deben quedar commiteados en texto plano.

2. **`AuthenticationService.LoginAsync` logueaba a consola la contraseña
   en texto plano y el hash BCrypt** en cada intento de login (`Console.WriteLine`
   de depuración que quedó en el código). Eliminado.

3. **Endpoint público `POST /api/setup/admin`**, sin autenticación,
   que podía crear el usuario administrador con contraseña fija
   (`Admin123*`) si por alguna razón no existía. Ya era redundante con el
   seed automático que corre al iniciar la app. Se eliminó por completo.

4. **Ningún controlador tenía `[Authorize]`** — Ligas, Equipos,
   Temporadas, Jornadas, Jugadores, Árbitros, Canchas, Países, Ciudades,
   Estados, Categorías estaban totalmente abiertos sin autenticación. Se
   agregó `[Authorize]` a los 11 controladores de negocio (se dejó
   `AuthController` sin protección, como corresponde para login).

5. **`AddAuthentication()` se llamaba sin configurar ningún esquema** —
   aunque `JwtService` generaba tokens correctamente, la API nunca los
   validaba. Combinado con el punto anterior, cualquier `[Authorize]`
   habría fallado en tiempo de ejecución. Se agregó `.AddJwtBearer(...)`
   con `TokenValidationParameters` completos (issuer, audience, firma,
   expiración) usando la configuración real de `Jwt:*`.

## 🟠 Bugs funcionales

6. **El middleware de manejo de excepciones estaba registrado después de
   `MapControllers()`**, por lo que nunca llegaba a envolver la ejecución
   de los controladores — las excepciones de dominio no se traducían al
   `ErrorResponse` JSON esperado. Se movió al inicio del pipeline.

7. **`ExceptionMiddleware` no contemplaba `UnauthorizedAccessException`**
   (la que lanza el login con credenciales inválidas) — cualquier intento
   de login fallido devolvía `500 Internal Server Error` en vez de `401`.
   Corregido.

8. **La expiración devuelta en la respuesta de login no coincidía con la
   expiración real del token**: el JSON decía `Expira` calculado con un
   valor fijo (60 min en un archivo, 8 horas en otro — ver punto 9), pero
   el JWT en sí se firma con `Jwt:ExpirationMinutes`. Se agregó
   `IJwtService.GetAccessTokenExpiration()` como única fuente de verdad,
   usada tanto en el flujo real como en el paralelo (ver siguiente punto).

9. **Existían dos implementaciones distintas del login** que no compartían
   código: la que realmente usa `AuthController`
   (`FutLiga.Infrastructure.Authentication.Services.AuthenticationService`,
   invocada directo sin pasar por MediatR) y otra completa vía CQRS/MediatR
   (`FutLiga.Application.Authentication.Commands.Login.LoginCommandHandler`)
   que nunca se conecta a ningún controlador. Ambas quedaron corregidas en
   este pase, pero **siguen siendo dos caminos paralelos** — recomiendo
   consolidar en una sola implementación (lo natural, dado que todos los
   demás controladores usan MediatR, es hacer que `AuthController` también
   despache `LoginCommand` vía `IMediator`, y retirar el acceso directo a
   `IAuthenticationService`). No lo hice en este pase para no tocar más
   superficie de la necesaria sin tu visto bueno.

10. **`RefreshAsync` y `LogoutAsync` lanzan `NotImplementedException`** en
    ambas implementaciones. El `RefreshToken` sí se genera y persiste en
    el login, pero no hay forma de canjearlo todavía, ni endpoints
    `/api/auth/refresh` o `/api/auth/logout` en el controlador. Es el
    siguiente paso lógico para cerrar el ciclo de sesión.

## 🧹 Limpieza (código muerto de una refactorización anterior)

11. Tres carpetas completas eran **restos huérfanos sin `.csproj` propio y
    sin ser referenciadas por nada** (`src/FutLiga.AuthenticationService`,
    `src/FutLiga.UsuarioRepository`, `src/FutLiga.UnitOfWork`), con copias
    duplicadas y desactualizadas de clases que ya existen correctamente en
    `FutLiga.Infrastructure`, `FutLiga.Persistence` y `FutLiga.Domain`. No
    rompían la compilación (nunca se compilaban), pero eran una trampa:
    alguien podía editarlas pensando que eran el código real. Eliminadas.

12. Cinco archivos `Class1.cs` — la clase placeholder que Visual Studio /
    `dotnet new classlib` genera automáticamente y que nadie borró.
    Eliminados.

13. Un `appsettings.json` "fantasma" dentro de
    `Infrastructure/Authentication/Jwt/` que no es el que realmente lee la
    aplicación (el real está en `FutLiga.Api/appsettings.json`). Eliminado
    para evitar confusión.

14. Se agregó un `.gitignore` básico (no existía) para no volver a subir
    `bin/`, `obj/` ni archivos de secretos locales por accidente.

## Lo que ya funciona bien y no se tocó
- Los controladores CRUD (Ligas, Equipos, Temporadas, etc.) están limpios,
  consistentes y siguen el patrón CQRS correctamente — probablemente
  generados con tu `ModuleGenerator`.
- El contrato del login coincide con lo que espera Flutter: los nombres de
  propiedades en C# (`AccessToken`, `RefreshToken`, `UsuarioId`, `Correo`,
  `Nombre`, `Expira`) se serializan en camelCase por defecto y calzan con
  lo que `LoginResponseModel.fromApi` parsea del lado de la app.
- Bcrypt para hash de contraseñas, bien implementado.

## Pendiente para siguiente fase
- Consolidar las dos rutas de login en una sola (punto 9).
- Implementar `/api/auth/refresh` y `/api/auth/logout` de verdad.
- Reemplazar los metadatos de sesión hardcodeados (`"127.0.0.1"`, `"Swagger"`,
  `"Windows"`) por los datos reales de la petición HTTP (`HttpContext` /
  `ICurrentUserService`, que ya está registrado).
- Construir los endpoints propios de la quiniela (jornadas con partidos,
  captura de pronósticos, cálculo de puntos, tabla de posiciones) — hoy el
  backend tiene el catálogo (ligas, equipos, temporadas, países, etc.) pero
  no la lógica de negocio de "adivinar resultados y sumar puntos" en sí.
