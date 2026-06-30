using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Backup
{
    /// <summary>
    /// Representa un dispositivo registrado en Oxidized.
    /// Mapeado directamente desde la respuesta JSON de la API de Oxidized (GET /nodes.json).
    /// </summary>
    public class OxidizedDeviceDto
    {
        /// <summary>Nombre corto del dispositivo en Oxidized (ej. "sw-core-01").</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Nombre completo del dispositivo incluyendo grupo (ej. "switches/sw-core-01").</summary>
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        /// <summary>Dirección IP del dispositivo gestionado por Oxidized.</summary>
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        /// <summary>Grupo de Oxidized al que pertenece el dispositivo (ej. "switches", "routers").</summary>
        [JsonPropertyName("group")]
        public string? Group { get; set; }

        /// <summary>Modelo del dispositivo tal como lo identifica Oxidized (ej. "ios", "junos").</summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>Información del último job de backup ejecutado por Oxidized.</summary>
        [JsonPropertyName("last")]
        public OxidizedLastDto? Last { get; set; }

        /// <summary>Fecha y hora de la última modificación del backup almacenado en Oxidized.</summary>
        [JsonPropertyName("mtime")]
        public string? LastChanged { get; set; }

        /// <summary>Estado del último backup (ej. "success", "failed", "never").</summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    /// <summary>
    /// Detalles del último job de backup ejecutado por Oxidized para un dispositivo.
    /// </summary>
    public class OxidizedLastDto
    {
        /// <summary>Timestamp de inicio del último job de backup.</summary>
        [JsonPropertyName("start")]
        public string? Start { get; set; }

        /// <summary>Timestamp de finalización del último job de backup.</summary>
        [JsonPropertyName("end")]
        public string? End { get; set; }

        /// <summary>Resultado del job (ej. "success", "fail").</summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>Duración del job en segundos.</summary>
        [JsonPropertyName("time")]
        public double? Time { get; set; }
    }
}
