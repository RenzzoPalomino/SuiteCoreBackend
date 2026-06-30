namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Representa un nodo (device) registrado en el tailnet de Tailscale.
    /// Mapeado desde la respuesta de la API <c>GET /api/v2/tailnet/{tailnet}/devices</c>.
    /// </summary>
    public class TailscalePeerDto
    {
        /// <summary>Nombre del host tal como está registrado en Tailscale (ej. "laptop-ivan").</summary>
        public string Hostname { get; set; } = string.Empty;

        /// <summary>IP asignada por Tailscale dentro de la red mesh (rango 100.x.x.x).</summary>
        public string Ip { get; set; } = string.Empty;

        /// <summary>Usuario propietario del dispositivo en Tailscale (ej. "ivanrup123@").</summary>
        public string User { get; set; } = string.Empty;

        /// <summary>Sistema operativo del dispositivo (ej. "windows", "linux", "iOS").</summary>
        public string Os { get; set; } = string.Empty;

        /// <summary>Indica si el nodo está conectado activamente al tailnet en este momento.</summary>
        public bool IsOnline { get; set; }

        /// <summary>Última vez visto en formato legible (ej. "active", "2h ago", "3d ago").</summary>
        public string LastSeen { get; set; } = string.Empty;
    }
}
