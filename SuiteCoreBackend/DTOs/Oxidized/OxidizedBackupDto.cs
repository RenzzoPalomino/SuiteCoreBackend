namespace SuiteCoreBackend.DTOs.Oxidized
{
    /// <summary>
    /// Contiene la configuración completa (backup) de un dispositivo obtenida desde Oxidized.
    /// Retornado por GET /api/oxidized/devices/{deviceName}/backup.
    /// Puede corresponder al backup más reciente o a una versión específica si se pasa el parámetro oid.
    /// </summary>
    public class OxidizedBackupDto
    {
        /// <summary>Nombre del dispositivo en Oxidized al que pertenece este backup.</summary>
        public string DeviceName { get; set; } = string.Empty;

        /// <summary>Contenido de la configuración del dispositivo en texto plano (formato normalizado).</summary>
        public string Config { get; set; } = string.Empty;

        /// <summary>Tipo de backup si Oxidized lo especifica en la respuesta (puede ser null).</summary>
        public string? BackupType { get; set; }

        /// <summary>Hash del commit Git de esta versión en el repositorio interno de Oxidized.</summary>
        public string? Oid { get; set; }

        /// <summary>Timestamp Unix de esta versión del backup.</summary>
        public long? Epoch { get; set; }

        /// <summary>Número ordinal de la versión dentro del historial del dispositivo.</summary>
        public int? Num { get; set; }

        /// <summary>Fecha y hora en que este backup fue recuperado por la API (hora local del servidor).</summary>
        public DateTime RetrievedAt { get; set; } = DateTime.Now;
    }
}
