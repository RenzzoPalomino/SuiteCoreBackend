using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Oxidized
{
    /// <summary>
    /// Representa una versión (commit) del historial de backups de un dispositivo en Oxidized.
    /// Mapeado desde la respuesta de GET /node/version?node={name} en la API de Oxidized.
    /// </summary>
    public class OxidizedVersionDto
    {
        /// <summary>Fecha del commit en formato ISO (ej. "2024-01-15").</summary>
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        /// <summary>Hora del commit.</summary>
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        /// <summary>Hash del commit en el repositorio Git interno de Oxidized.</summary>
        [JsonPropertyName("oid")]
        public string? Oid { get; set; }

        /// <summary>Información del autor que disparó el backup (usuario del sistema Oxidized).</summary>
        [JsonPropertyName("author")]
        public OxidizedAuthorDto? Author { get; set; }

        /// <summary>Mensaje del commit (generalmente vacío en Oxidized, se usa para notas manuales).</summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>Timestamp Unix del commit. Calculado desde Date+Time por el servicio.</summary>
        public long? Epoch { get; set; }

        /// <summary>Número ordinal de la versión dentro del historial del dispositivo (1 = más reciente).</summary>
        public int Num { get; set; }

        /// <summary>URL para obtener el backup de esta versión específica vía GET /api/oxidized/devices/{name}/backup.</summary>
        public string? BackupUrl { get; set; }
    }

    /// <summary>
    /// Información del autor asociado a un commit de backup en Oxidized.
    /// </summary>
    public class OxidizedAuthorDto
    {
        /// <summary>Nombre del autor del commit.</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Email del autor del commit.</summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>Timestamp del autor en el commit Git.</summary>
        [JsonPropertyName("time")]
        public string? Time { get; set; }
    }
}
