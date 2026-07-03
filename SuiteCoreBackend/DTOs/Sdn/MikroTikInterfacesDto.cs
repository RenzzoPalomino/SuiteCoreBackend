namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Lista de interfaces del MikroTik administrado por el SCNO.</summary>
    public class MikroTikInterfacesDto
    {
        public string Status { get; set; } = string.Empty;
        public List<MikroTikInterfaceItemDto> Interfaces { get; set; } = new();
    }

    public class MikroTikInterfaceItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Running { get; set; }
        public bool Disabled { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
