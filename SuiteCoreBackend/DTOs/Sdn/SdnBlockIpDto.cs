namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Payload para bloquear o desbloquear una IP en el SCNO.</summary>
    public class SdnBlockIpDto
    {
        /// <summary>Dirección IP a bloquear o desbloquear.</summary>
        public string Ip { get; set; } = string.Empty;
    }
}
