using AlertaTempranaAPI.Layers.Dtos.Alerts;

namespace AlertaTempranaAPI.Layers.Domain.Constants
{

    public static class EmailTemplate
    {
        public static string EmergencyAlert(RequestAlert alert)
        {
            var isEmergency = alert.Type == EventType.Emergency;
            var accentColor = isEmergency ? "#C0392B" : "#2980B9";
            var badgeBg = isEmergency ? "#FDEDEC" : "#EBF5FB";
            var badgeText = isEmergency ? "🚨 EMERGENCIA" : "📍 POSICIÓN";
            var statusColor = alert.Status == StatusVehicule.OK ? "#27AE60" : "#E67E22";
            var mapsUrl = $"https://www.google.com/maps?q={alert.Coordinates.Latitude},{alert.Coordinates.Longitude}";
            var reportedAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss") + " UTC";

            return $"""
            <!DOCTYPE html>
            <html lang="es">
            <head>
              <meta charset="UTF-8">
              <meta name="viewport" content="width=device-width, initial-scale=1.0">
              <title>Alerta de vehículo</title>
            </head>
            <body style="margin:0; padding:0; background-color:#F2F3F4; font-family:'Segoe UI', Arial, sans-serif;">
 
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#F2F3F4; padding:40px 0;">
                <tr>
                  <td align="center">
                    <table width="600" cellpadding="0" cellspacing="0" style="max-width:600px; width:100%;">
 
                      <!-- Header -->
                      <tr>
                        <td style="background:{accentColor}; border-radius:10px 10px 0 0; padding:32px 40px; text-align:center;">
                          <p style="margin:0 0 8px; font-size:13px; color:rgba(255,255,255,0.75); letter-spacing:2px; text-transform:uppercase;">Sistema de monitoreo vehicular</p>
                          <h1 style="margin:0; font-size:28px; color:#FFFFFF; font-weight:700; letter-spacing:-0.5px;">
                            {(isEmergency ? "Alerta de Emergencia" : "Reporte de Posición")}
                          </h1>
                        </td>
                      </tr>
 
                      <!-- Badge de tipo de evento -->
                      <tr>
                        <td style="background:#FFFFFF; padding:24px 40px 0; text-align:center;">
                          <span style="display:inline-block; background:{badgeBg}; color:{accentColor}; border:1px solid {accentColor};
                                       font-size:13px; font-weight:700; letter-spacing:1px; padding:6px 18px; border-radius:20px;">
                            {badgeText}
                          </span>
                        </td>
                      </tr>
 
                      <!-- Datos del vehículo -->
                      <tr>
                        <td style="background:#FFFFFF; padding:28px 40px 0;">
                          <p style="margin:0 0 16px; font-size:11px; font-weight:700; color:#95A5A6; letter-spacing:1.5px; text-transform:uppercase;">Información del vehículo</p>
 
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td style="padding:12px 16px; background:#FAFAFA; border-radius:8px 8px 0 0; border:1px solid #ECF0F1; border-bottom:none;">
                                <span style="font-size:12px; color:#95A5A6; display:block; margin-bottom:2px;">Placa</span>
                                <span style="font-size:22px; font-weight:700; color:#2C3E50; letter-spacing:3px; font-family:'Courier New', monospace;">
                                  {alert.VehiculePlate.ToUpperInvariant()}
                                </span>
                              </td>
                            </tr>
                            <tr>
                              <td style="padding:12px 16px; background:#FAFAFA; border-radius:0 0 8px 8px; border:1px solid #ECF0F1;">
                                <span style="font-size:12px; color:#95A5A6; display:block; margin-bottom:4px;">Estado del vehículo</span>
                                <span style="display:inline-block; background:{statusColor}; color:#FFFFFF; font-size:12px;
                                             font-weight:700; padding:3px 12px; border-radius:12px;">
                                  {alert.Status}
                                </span>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
 
                      <!-- Coordenadas -->
                      <tr>
                        <td style="background:#FFFFFF; padding:20px 40px 0;">
                          <p style="margin:0 0 16px; font-size:11px; font-weight:700; color:#95A5A6; letter-spacing:1.5px; text-transform:uppercase;">Ubicación reportada</p>
 
                          <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #ECF0F1; border-radius:8px; overflow:hidden;">
                            <tr>
                              <td width="50%" style="padding:14px 16px; background:#FAFAFA; border-right:1px solid #ECF0F1;">
                                <span style="font-size:12px; color:#95A5A6; display:block; margin-bottom:2px;">Latitud</span>
                                <span style="font-size:15px; font-weight:600; color:#2C3E50; font-family:'Courier New', monospace;">
                                  {alert.Coordinates.Latitude:F6}
                                </span>
                              </td>
                              <td width="50%" style="padding:14px 16px; background:#FAFAFA;">
                                <span style="font-size:12px; color:#95A5A6; display:block; margin-bottom:2px;">Longitud</span>
                                <span style="font-size:15px; font-weight:600; color:#2C3E50; font-family:'Courier New', monospace;">
                                  {alert.Coordinates.Longitude:F6}
                                </span>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
 
                      <!-- Botón Ver en mapa -->
                      <tr>
                        <td style="background:#FFFFFF; padding:24px 40px 32px; text-align:center;">
                          <a href="{mapsUrl}"
                             style="display:inline-block; background:{accentColor}; color:#FFFFFF; font-size:14px;
                                    font-weight:700; padding:14px 36px; border-radius:8px; text-decoration:none;
                                    letter-spacing:0.3px;">
                            Ver ubicación en Google Maps →
                          </a>
                        </td>
                      </tr>
 
                      <!-- Footer -->
                      <tr>
                        <td style="background:#2C3E50; border-radius:0 0 10px 10px; padding:20px 40px; text-align:center;">
                          <p style="margin:0 0 4px; font-size:12px; color:rgba(255,255,255,0.5);">
                            Reporte generado el {reportedAt}
                          </p>
                          <p style="margin:0; font-size:11px; color:rgba(255,255,255,0.3);">
                            Este mensaje fue generado automáticamente. No responder a este correo.
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
}
