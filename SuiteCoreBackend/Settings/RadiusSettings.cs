namespace SuiteCoreBackend.Settings
{
    [Obsolete("Esta clase de configuración no se está utilizando actualmente, " +
        "ya que la funcionalidad de RADIUS ha sido suspendida temporalmente.",true)]
    public class RadiusSettings
    {
        public string Server { get; set; } = string.Empty;
        public int AccountingPort { get; set; } = 1813;
        public string SharedSecret { get; set; } = string.Empty;
        public int TimeoutMs { get; set; } = 5000;
    }
}
