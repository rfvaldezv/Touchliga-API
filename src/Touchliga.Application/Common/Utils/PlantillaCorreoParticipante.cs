namespace Touchliga.Application.Common.Utils;

/// <summary>
/// Arma el HTML de los correos de bienvenida/actualización que se
/// le mandan a un participante al darlo de alta o editar su info.
/// Un solo diseño reutilizado en ambos casos, cambia el encabezado
/// y si se muestra o no la contraseña.
/// </summary>
public static class PlantillaCorreoParticipante
{
    private const string LogoUrl = "https://app.touchliga.com/icons/Icon-512.png";
    private const string AppUrl = "https://app.touchliga.com";
    private const string NavyOscuro = "#1F2841";
    private const string NavyMedio = "#2D3B5E";
    private const string NaranjaAcento = "#FF6D00";

    public static string Bienvenida(
        string nombreCompleto,
        string correo,
        string passwordTemporal,
        string telefono,
        string? ciudad,
        string? estado,
        string? pais)
    {
        return Construir(
            titulo: "¡Bienvenido a Touchliga!",
            subtitulo: "Pasión que nos une — tu cuenta ya está lista",
            saludo: $"¡Qué gusto tenerte, {nombreCompleto}! 🏈",
            mensaje: "Ya formas parte de la quiniela — aquí tienes tus datos de acceso. "
                + "Te recomendamos cambiar tu contraseña la primera vez que entres, desde tu Perfil.",
            nombreCompleto: nombreCompleto,
            correo: correo,
            passwordTemporal: passwordTemporal,
            telefono: telefono,
            ciudad: ciudad,
            estado: estado,
            pais: pais,
            textoBoton: "Entrar a Touchliga");
    }

    public static string InformacionActualizada(
        string nombreCompleto,
        string correo,
        string telefono,
        string? ciudad,
        string? estado,
        string? pais)
    {
        return Construir(
            titulo: "Tu información se actualizó",
            subtitulo: "Touchliga — Pasión que nos une",
            saludo: $"Hola, {nombreCompleto} 👋",
            mensaje: "Se acaba de actualizar la información de tu cuenta. Si tú no hiciste este "
                + "cambio, contacta al administrador de tu liga lo antes posible.",
            nombreCompleto: nombreCompleto,
            correo: correo,
            passwordTemporal: null,
            telefono: telefono,
            ciudad: ciudad,
            estado: estado,
            pais: pais,
            textoBoton: "Ver mi cuenta");
    }

    public static string PronosticosConfirmados(
        string nombre,
        string nombreJornada,
        List<(string equipoLocal, string equipoVisitante, string prediccion)> detalles)
    {
        var filasDetalle = string.Join("\n", detalles.Select((d, i) => $"""
              <tr>
                <td style="padding:10px 12px; color:#FFFFFF; font-size:14px; {(i > 0 ? "border-top:1px solid #2D3B5E;" : "")}">{d.equipoLocal} <span style="color:#5C6480;">vs</span> {d.equipoVisitante}</td>
                <td style="padding:10px 12px; text-align:right; {(i > 0 ? "border-top:1px solid #2D3B5E;" : "")}">
                  <span style="background:{NaranjaAcento}22; color:{NaranjaAcento}; font-weight:700; font-size:14px; padding:3px 10px; border-radius:6px;">{d.prediccion}</span>
                </td>
              </tr>
              """));

        return $"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>Pronósticos confirmados</title>
        </head>
        <body style="margin:0; padding:0; background-color:#0F1420; font-family:-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;">
          <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#0F1420; padding:32px 16px;">
            <tr>
              <td align="center">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="max-width:480px; background:linear-gradient(180deg,{NavyOscuro} 0%,{NavyMedio} 100%); border-radius:16px; overflow:hidden; box-shadow:0 8px 32px rgba(0,0,0,0.4);">

                  <tr>
                    <td align="center" style="padding:36px 24px 20px;">
                      <img src="{LogoUrl}" alt="Touchliga" width="88" height="88" style="display:block; border-radius:18px;">
                      <p style="margin:16px 0 0; color:{NaranjaAcento}; font-size:12px; font-weight:700; letter-spacing:2px; text-transform:uppercase;">Pronósticos recibidos</p>
                    </td>
                  </tr>

                  <tr>
                    <td style="height:3px; background:linear-gradient(90deg,{NaranjaAcento} 0%,#FFB073 50%,{NaranjaAcento} 100%);"></td>
                  </tr>

                  <tr>
                    <td align="center" style="padding:32px 32px 8px;">
                      <div style="width:64px; height:64px; background:{NaranjaAcento}22; border-radius:50%; display:inline-block; line-height:64px; font-size:32px;">✅</div>
                      <h1 style="margin:20px 0 8px; color:#FFFFFF; font-size:20px; font-weight:800;">¡Todo listo, {nombre}!</h1>
                      <p style="margin:0; color:#C4CADB; font-size:15px; line-height:1.6;">
                        Tus pronósticos para <strong style="color:#FFFFFF;">{nombreJornada}</strong> quedaron
                        registrados completos — los {detalles.Count} partidos. Este correo también te
                        sirve como comprobante de tu registro.
                      </p>
                    </td>
                  </tr>

                  <tr>
                    <td style="padding:16px 32px 8px;">
                      <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background:#161D30; border-radius:12px; overflow:hidden;">
                        {filasDetalle}
                      </table>
                    </td>
                  </tr>

                  <tr>
                    <td align="center" style="padding:20px 32px 8px;">
                      <p style="margin:0; color:#8B93A7; font-size:13px; line-height:1.6;">
                        Si necesitas corregir alguno, puedes hacerlo desde la app mientras la jornada siga abierta.
                      </p>
                    </td>
                  </tr>

                  <tr>
                    <td align="center" style="padding:28px 32px 36px;">
                      <a href="{AppUrl}" style="display:inline-block; background:{NaranjaAcento}; color:#0F1420; text-decoration:none; font-weight:800; font-size:15px; padding:14px 40px; border-radius:10px; letter-spacing:0.3px;">¡Mucha suerte! 🏈</a>
                    </td>
                  </tr>

                  <tr>
                    <td align="center" style="padding:0 32px 28px;">
                      <p style="margin:0; color:#5C6480; font-size:12px; line-height:1.6;">Touchliga · Pasión que nos une</p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
    }

    private static string Construir(
        string titulo,
        string subtitulo,
        string saludo,
        string mensaje,
        string nombreCompleto,
        string correo,
        string? passwordTemporal,
        string telefono,
        string? ciudad,
        string? estado,
        string? pais,
        string textoBoton)
    {
        var filaPassword = passwordTemporal is null
            ? ""
            : $"""
              <tr>
                <td style="padding:10px 0; color:#8B93A7; font-size:14px;">Contraseña temporal</td>
                <td style="padding:10px 0; text-align:right;">
                  <span style="background:{NaranjaAcento}22; color:{NaranjaAcento}; font-weight:700; font-size:15px; padding:4px 12px; border-radius:6px; font-family:monospace;">{passwordTemporal}</span>
                </td>
              </tr>
              """;

        var ubicacion = string.Join(", ", new[] { ciudad, estado, pais }.Where(x => !string.IsNullOrWhiteSpace(x)));
        var filaUbicacion = string.IsNullOrWhiteSpace(ubicacion)
            ? ""
            : $"""
              <tr>
                <td style="padding:10px 0; color:#8B93A7; font-size:14px; border-top:1px solid #2D3B5E;">Ubicación</td>
                <td style="padding:10px 0; text-align:right; color:#FFFFFF; font-size:15px; border-top:1px solid #2D3B5E;">{ubicacion}</td>
              </tr>
              """;

        return $"""
        <!DOCTYPE html>
        <html lang="es">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>{titulo}</title>
        </head>
        <body style="margin:0; padding:0; background-color:#0F1420; font-family:-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;">
          <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#0F1420; padding:32px 16px;">
            <tr>
              <td align="center">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="max-width:480px; background:linear-gradient(180deg,{NavyOscuro} 0%,{NavyMedio} 100%); border-radius:16px; overflow:hidden; box-shadow:0 8px 32px rgba(0,0,0,0.4);">

                  <!-- Encabezado con logo -->
                  <tr>
                    <td align="center" style="padding:36px 24px 20px;">
                      <img src="{LogoUrl}" alt="Touchliga" width="88" height="88" style="display:block; border-radius:18px;">
                      <p style="margin:16px 0 0; color:{NaranjaAcento}; font-size:12px; font-weight:700; letter-spacing:2px; text-transform:uppercase;">{subtitulo}</p>
                    </td>
                  </tr>

                  <!-- Franja de acento -->
                  <tr>
                    <td style="height:3px; background:linear-gradient(90deg,{NaranjaAcento} 0%,#FFB073 50%,{NaranjaAcento} 100%);"></td>
                  </tr>

                  <!-- Saludo y mensaje -->
                  <tr>
                    <td style="padding:28px 32px 8px;">
                      <h1 style="margin:0 0 12px; color:#FFFFFF; font-size:22px; font-weight:800;">{saludo}</h1>
                      <p style="margin:0; color:#C4CADB; font-size:15px; line-height:1.6;">{mensaje}</p>
                    </td>
                  </tr>

                  <!-- Tarjeta de datos -->
                  <tr>
                    <td style="padding:20px 32px 8px;">
                      <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background:#161D30; border-radius:12px; padding:20px 20px;">
                        <tr>
                          <td style="padding:0 20px;">
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                              <tr>
                                <td style="padding:10px 0; color:#8B93A7; font-size:14px;">Nombre</td>
                                <td style="padding:10px 0; text-align:right; color:#FFFFFF; font-size:15px; font-weight:600;">{nombreCompleto}</td>
                              </tr>
                              <tr>
                                <td style="padding:10px 0; color:#8B93A7; font-size:14px; border-top:1px solid #2D3B5E;">Correo (usuario)</td>
                                <td style="padding:10px 0; text-align:right; color:#FFFFFF; font-size:15px; border-top:1px solid #2D3B5E;">{correo}</td>
                              </tr>
                              {filaPassword}
                              <tr>
                                <td style="padding:10px 0; color:#8B93A7; font-size:14px; border-top:1px solid #2D3B5E;">Teléfono</td>
                                <td style="padding:10px 0; text-align:right; color:#FFFFFF; font-size:15px; border-top:1px solid #2D3B5E;">{telefono}</td>
                              </tr>
                              {filaUbicacion}
                            </table>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Botón -->
                  <tr>
                    <td align="center" style="padding:28px 32px 36px;">
                      <a href="{AppUrl}" style="display:inline-block; background:{NaranjaAcento}; color:#0F1420; text-decoration:none; font-weight:800; font-size:15px; padding:14px 40px; border-radius:10px; letter-spacing:0.3px;">{textoBoton} →</a>
                    </td>
                  </tr>

                  <!-- Pie -->
                  <tr>
                    <td align="center" style="padding:0 32px 28px;">
                      <p style="margin:0; color:#5C6480; font-size:12px; line-height:1.6;">
                        Touchliga · Pasión que nos une<br>
                        Si no reconoces esta cuenta, ignora este correo.
                      </p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
    }
}
