namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Define a qué red destino apunta esta regla de acceso VPN
    /// y si el tráfico desde la red de origen está permitido.
    /// </summary>
    public class AccessDestinationDto
    {
        /// <summary>Red o destino de la regla (ej. "172.16.20.0/24", "Internet").</summary>
        public string Network { get; set; } = string.Empty;

        /// <summary>Indica si el acceso desde la red VPN hacia este destino está permitido.</summary>
        public bool IsAllowed { get; set; }
    }

    /// <summary>
    /// Política de acceso estática de la VPN WireGuard.
    /// Retornada por GET /api/vpn/access-policy. Define qué redes son alcanzables
    /// desde la red VPN (172.16.40.0/24) según las reglas de enrutamiento configuradas.
    /// </summary>
    public class VpnAccessPolicyDto
    {
        /// <summary>Red de origen VPN desde la que aplican las reglas (ej. "172.16.40.0/24").</summary>
        public string Origin { get; set; } = string.Empty;

        /// <summary>Lista de redes destino con su estado de acceso permitido o denegado.</summary>
        public List<AccessDestinationDto> Destinations { get; set; } = new();
    }
}
