namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Métricas de tráfico de la interfaz WireGuard obtenidas desde /proc/net/dev.
    /// Retornado por GET /api/vpn/wireguard/stats. El frontend puede consultar este
    /// endpoint periódicamente y calcular el delta entre dos snapshots para obtener Mbps.
    /// Los contadores son acumulativos desde el último reinicio de la interfaz.
    /// </summary>
    public class WireGuardStatsDto
    {
        /// <summary>Nombre de la interfaz de red (ej. "wg0").</summary>
        public string Interface { get; set; } = string.Empty;

        /// <summary>Bytes totales recibidos por la interfaz (acumulativo).</summary>
        public long RxBytes { get; set; }

        /// <summary>Bytes totales enviados por la interfaz (acumulativo).</summary>
        public long TxBytes { get; set; }

        /// <summary>Paquetes totales recibidos por la interfaz.</summary>
        public long RxPackets { get; set; }

        /// <summary>Paquetes totales enviados por la interfaz.</summary>
        public long TxPackets { get; set; }

        /// <summary>Errores de recepción reportados por el kernel para esta interfaz.</summary>
        public long RxErrors { get; set; }

        /// <summary>Errores de envío reportados por el kernel para esta interfaz.</summary>
        public long TxErrors { get; set; }

        /// <summary>
        /// Timestamp UTC del snapshot. Usar la diferencia entre dos timestamps consecutivos
        /// junto con la diferencia de bytes para calcular throughput en Mbps.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
