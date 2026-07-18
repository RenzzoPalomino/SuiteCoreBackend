using SuiteCoreBackend.DTOs.Backup;
using SuiteCoreBackend.DTOs.Oxidized;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IOxidizedService
    {
        /// <summary>
        /// Consulta <c>GET /nodes.json</c> en Oxidized y retorna todos los dispositivos registrados
        /// con el estado de su último backup (success, failed, never), IP, grupo, modelo y
        /// fecha de última modificación del respaldo almacenado.
        /// <para>
        /// Requiere que los dispositivos estén registrados en el <c>router.db</c> de Oxidized
        /// con IP y credenciales válidas. Un dispositivo con <c>status: failed</c> indica
        /// que Oxidized no pudo conectarse a él en el último intento de backup.
        /// </para>
        /// </summary>
        /// <returns>Lista de dispositivos con su estado de backup. Lista vacía si Oxidized no tiene nodos registrados.</returns>
        /// <exception cref="Exception">Si Oxidized responde con un status distinto de 2xx.</exception>
        Task<List<OxidizedDeviceDto>> GetDevicesAsync();

        /// <summary>
        /// Obtiene el texto completo de la configuración de un dispositivo almacenada en Oxidized.
        /// El comportamiento varía según los parámetros recibidos:
        /// <list type="bullet">
        ///   <item><description>
        ///     <b>Sin oid/epoch/num:</b> consulta <c>GET /node/fetch/{deviceName}</c> y retorna
        ///     el backup más reciente guardado por Oxidized (configuración actual del dispositivo).
        ///   </description></item>
        ///   <item><description>
        ///     <b>Con oid + epoch + num:</b> consulta <c>GET /node/version/view</c> y retorna
        ///     una versión histórica específica identificada por su hash Git (<c>oid</c>).
        ///     Estos parámetros se obtienen previamente desde <see cref="GetDeviceVersionsAsync"/>.
        ///   </description></item>
        /// </list>
        /// La respuesta de Oxidized puede llegar como JSON array de líneas o como texto plano;
        /// el servicio la normaliza a texto plano antes de retornarla.
        /// <para>
        /// Requiere que el dispositivo tenga al menos un backup exitoso almacenado en Oxidized.
        /// Para versiones históricas, Oxidized debe tener configurado <c>output: git</c>.
        /// </para>
        /// </summary>
        /// <param name="deviceName">Nombre completo del dispositivo en Oxidized (incluye grupo si aplica, ej. "switches/sw-core-01").</param>
        /// <param name="oid">Hash del commit Git de la versión específica. Obtenido desde GetDeviceVersionsAsync.</param>
        /// <param name="epoch">Timestamp Unix de la versión específica. Obtenido desde GetDeviceVersionsAsync.</param>
        /// <param name="num">Número ordinal de la versión dentro del historial. Obtenido desde GetDeviceVersionsAsync.</param>
        /// <param name="group">Grupo del dispositivo en Oxidized. Necesario para versiones históricas si el dispositivo pertenece a un grupo.</param>
        /// <returns>DTO con el texto de configuración normalizado, tipo de backup ("current" o "version") y metadatos de la versión.</returns>
        /// <exception cref="Exception">Si Oxidized responde con un status distinto de 2xx o el dispositivo no tiene backup almacenado.</exception>
        Task<OxidizedBackupDto> GetDeviceBackupAsync(
            string deviceName,
            string? oid = null,
            long? epoch = null,
            int? num = null,
            string? group = ""
        );

        /// <summary>
        /// Consulta <c>GET /node/version?node_full={deviceName}</c> en Oxidized y retorna el
        /// historial completo de versiones (commits Git) del backup de un dispositivo.
        /// Cada versión representa una captura de la configuración en un momento en el tiempo.
        /// <para>
        /// El servicio enriquece cada versión con tres campos calculados que Oxidized no retorna:
        /// <list type="bullet">
        ///   <item><description><b>Epoch:</b> timestamp Unix derivado de los campos date/time del commit.</description></item>
        ///   <item><description><b>Num:</b> número ordinal de la versión (el más reciente recibe num = total, el más antiguo num = 1).</description></item>
        ///   <item><description><b>BackupUrl:</b> URL lista para consumir desde el frontend que apunta a <see cref="GetDeviceBackupAsync"/> con los parámetros de esa versión.</description></item>
        /// </list>
        /// </para>
        /// Requiere que Oxidized tenga configurado <c>output: git</c>. Sin Git activo,
        /// Oxidized solo conserva el backup más reciente y este endpoint retornará lista vacía.
        /// El parámetro <c>deviceName</c> debe ser el nombre completo (<c>full_name</c>) tal
        /// como lo reporta Oxidized, incluyendo el grupo (ej. "routers/borde-01").
        /// </summary>
        /// <param name="deviceName">Nombre completo del dispositivo en Oxidized (full_name con grupo).</param>
        /// <returns>Lista de versiones del historial ordenadas de más reciente a más antigua, con epoch, num y backupUrl calculados.</returns>
        /// <exception cref="Exception">Si Oxidized responde con un status distinto de 2xx o el dispositivo no existe.</exception>
        Task<List<OxidizedVersionDto>> GetDeviceVersionsAsync(string deviceName);
    }
}
