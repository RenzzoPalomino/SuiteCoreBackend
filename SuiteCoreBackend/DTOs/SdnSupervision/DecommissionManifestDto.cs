namespace SuiteCoreBackend.DTOs.SdnSupervision
{
    /// <summary>Manifiesto canónico del último proceso de decomisión SDN.</summary>
    public class DecommissionManifestDto
    {
        public string Status { get; set; } = string.Empty;
        public string Manifest { get; set; } = string.Empty;
        public string Sha256 { get; set; } = string.Empty;
        public bool RouterosPreserved { get; set; }
    }
}
