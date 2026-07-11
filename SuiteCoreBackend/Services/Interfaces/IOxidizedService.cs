using SuiteCoreBackend.DTOs.Backup;
using SuiteCoreBackend.DTOs.Oxidized;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IOxidizedService
    {
        /// <summary>
        /// Método para obtener la lista de dispositivos desde Oxidized.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<OxidizedDeviceDto>> GetDevicesAsync();
        /// <summary>
        /// Método para obtener la configuración de un dispositivo desde Oxidized.
        /// Si no se envían datos de versión, obtiene la configuración actual.
        /// Si se envían oid, epoch y num, obtiene una versión específica.
        /// </summary>
        /// <param name="deviceName">Nombre del dispositivo registrado en Oxidized.</param>
        /// <param name="oid">OID/hash de la versión específica.</param>
        /// <param name="epoch">Epoch de la versión específica.</param>
        /// <param name="num">Número de versión específica.</param>
        /// <param name="group">Grupo del dispositivo en Oxidized.</param>
        /// <returns>DTO con la configuración obtenida.</returns>
        /// <exception cref="Exception"></exception>
        Task<OxidizedBackupDto> GetDeviceBackupAsync(
            string deviceName,
            string? oid = null,
            long? epoch = null,
            int? num = null,
            string? group = ""
        );
        /// <summary>
        /// Método para mostrar historial/versiones disponibles del backup de un dispositivo.
        /// </summary>
        /// <param name="deviceName">Nombre completo del dispositivo registrado en Oxidized.</param>
        /// <returns>Lista de versiones disponibles del backup del dispositivo.</returns>
        /// <exception cref="Exception"></exception>
        Task<List<OxidizedVersionDto>> GetDeviceVersionsAsync(string deviceName);
    }
}
