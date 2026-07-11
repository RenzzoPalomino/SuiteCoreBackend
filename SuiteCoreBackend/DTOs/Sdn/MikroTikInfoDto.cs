namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Información del dispositivo MikroTik administrado por el SCNO.</summary>
    public class MikroTikInfoDto
    {
        public string Status { get; set; } = string.Empty;
        public string Identity { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Cpu { get; set; } = string.Empty;
        public int CpuLoad { get; set; }
        public string Architecture { get; set; } = string.Empty;
        public string Uptime { get; set; } = string.Empty;
        public long TotalMemory { get; set; }
        public long FreeMemory { get; set; }
    }
}
