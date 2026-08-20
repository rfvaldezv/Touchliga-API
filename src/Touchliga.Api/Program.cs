using Touchliga.Infrastructure;
using Touchliga.Persistence;
using Microsoft.OpenApi.Models;
using Touchliga.Domain.Entities;
using Touchliga.Domain.ValueObjects;
using Touchliga.Persistence.Context;
using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Application;
using Touchliga.Api.Middleware;
using Touchliga.Infrastructure.Authentication.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;

// Licencia gratuita de QuestPDF (Community) -- válida para este uso.
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Touchliga API",
        Version = "v1",
        Description = "API oficial de Touchliga"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese únicamente el JWT."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id="Bearer",
                    Type=ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

// Capas
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

// Authentication y Authorization
//
// Antes: `builder.Services.AddAuthentication();` se llamaba sin configurar
// ningún esquema. JwtService generaba tokens perfectamente válidos, pero
// la API nunca los validaba en las peticiones entrantes: cualquier
// endpoint protegido con [Authorize] habría fallado en tiempo de
// ejecución ("No authenticationScheme was specified..."), y de hecho
// ningún controlador tenía [Authorize], así que toda la API estaba
// abierta sin autenticación real.
var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException("Falta configurar la sección 'Jwt'.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

builder.Services.AddAuthorization();

// CORS — Flutter Web corre en el navegador y el navegador SÍ aplica
// estas reglas de seguridad (a diferencia de Windows desktop, Android
// o iOS, que no las tienen). Sin esto, la app web se queda con "no es
// posible conectar con el servidor" aunque la API esté corriendo bien.
//
// En desarrollo se permite cualquier puerto de localhost, porque
// `flutter run -d web-server` elige uno distinto cada vez. Cuando la
// web se publique en un dominio real, hay que agregar ese dominio aquí.
builder.Services.AddCors(options =>
{
    options.AddPolicy("TouchligaWeb", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;

                var host = uri.Host.ToLowerInvariant();
                var esLocal = host == "localhost" || host == "127.0.0.1" || host == "::1" || uri.IsLoopback;

                return esLocal
                    || origin == "https://app.touchliga.com"
                    || origin == "http://app.touchliga.com";
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Debe ir primero: si se registra después de MapControllers(), nunca
// llega a envolver la ejecución de los controladores y las excepciones
// de dominio terminan como errores 500 sin el formato JSON esperado.
app.UseMiddleware<ExceptionMiddleware>();

// Swagger sigue expuesto en producción por ahora — mientras la app
// solo la usa un grupo cerrado de amigos, es útil para confirmar que
// se está hablando con el servidor correcto. Antes de una
// publicación pública real, esto debe volver a ser solo Development.
app.UseSwagger();

app.UseSwaggerUI();

// En desarrollo, omitimos la redirección a HTTPS: los clientes
// móviles (Android/iOS) no confían en el certificado autofirmado
// de desarrollo, y la redirección terminaba causando fallos de
// conexión silenciosos en la app (se veía como "no se pudo conectar
// con el servidor" en vez de un error de certificado explícito).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("TouchligaWeb");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TouchligaDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    // Roles base del sistema. "Jugador" no existe como rol: cualquier
    // usuario autenticado ya puede pronosticar y ver clasificaciones,
    // por eso los únicos roles con privilegios elevados a sembrar son
    // Administrador (control total) y Capturador (solo resultados).
    var rolAdmin = await context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Administrador");

    if (rolAdmin == null)
    {
        rolAdmin = Rol.Crear("Administrador", "Control total del sistema.", 1);
        context.Roles.Add(rolAdmin);
        await context.SaveChangesAsync();
    }

    var rolCapturador = await context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Capturador");

    if (rolCapturador == null)
    {
        rolCapturador = Rol.Crear("Capturador", "Solo puede capturar resultados de partidos.", 1);
        context.Roles.Add(rolCapturador);
        await context.SaveChangesAsync();
    }

    var usuarioAdmin = await context.Usuarios
        .FirstOrDefaultAsync(x => x.Correo.Value == "admin@touchliga.com");

    if (usuarioAdmin == null)
    {
        usuarioAdmin = Usuario.Crear(
            "Administrador",
            Email.Create("admin@touchliga.com"),
            hasher.Hash("Admin123*"),
            1);

        usuarioAdmin.ConfirmarCorreo();

        context.Usuarios.Add(usuarioAdmin);

        await context.SaveChangesAsync();

        Console.WriteLine("Usuario administrador creado.");
    }

    var yaTieneRol = await context.UsuarioRoles.AnyAsync(
        ur => ur.UsuarioId == usuarioAdmin.Id && ur.RolId == rolAdmin.Id);

    if (!yaTieneRol)
    {
        var usuarioRol = UsuarioRol.Crear(usuarioAdmin.Id, rolAdmin.Id, 1);
        context.UsuarioRoles.Add(usuarioRol);
        await context.SaveChangesAsync();

        Console.WriteLine("Rol Administrador asignado al usuario admin.");
    }
}

app.Run();
