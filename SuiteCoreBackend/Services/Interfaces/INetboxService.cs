using SuiteCoreBackend.DTOs.Netbox;

namespace SuiteCoreBackend.Services.Interfaces;

public interface INetboxService
{
    /// <summary>
    /// Obtiene el listado de todas las direcciones IP registradas en Netbox.
    /// </summary>
    /// <returns>Colección de direcciones IP.</returns>
    Task<IEnumerable<NetboxIpAddressDto>> GetIpAddressesAsync();

    /// <summary>
    /// Obtiene el listado de todas las VLANs registradas en Netbox.
    /// </summary>
    /// <returns>Colección de VLANs.</returns>
    Task<IEnumerable<NetboxVlanDto>> GetVlansAsync();

    /// <summary>
    /// Obtiene el listado de todos los sitios registrados en Netbox.
    /// </summary>
    /// <returns>Colección de sitios.</returns>
    Task<IEnumerable<NetboxSiteDto>> GetSitesAsync();

    /// <summary>
    /// Obtiene el listado de todos los fabricantes registrados en Netbox.
    /// </summary>
    /// <returns>Colección de fabricantes.</returns>
    Task<IEnumerable<NetboxManufacturerDto>> GetManufacturersAsync();

    /// <summary>
    /// Obtiene el listado de todos los roles de dispositivo registrados en Netbox.
    /// </summary>
    /// <returns>Colección de roles de dispositivo.</returns>
    Task<IEnumerable<NetboxDeviceRoleDto>> GetDeviceRolesAsync();

    /// <summary>
    /// Obtiene el listado de todos los dispositivos registrados en Netbox.
    /// </summary>
    /// <returns>Colección de dispositivos.</returns>
    Task<IEnumerable<NetboxDeviceDto>> GetDevicesAsync();

    /// <summary>
    /// Obtiene el listado de todos los racks registrados en Netbox.
    /// </summary>
    /// <returns>Colección de racks.</returns>
    Task<IEnumerable<NetboxRackDto>> GetRacksAsync();

    /// <summary>
    /// Obtiene el listado de todos los tipos de dispositivo registrados en Netbox.
    /// </summary>
    /// <returns>Colección de tipos de dispositivo.</returns>
    Task<IEnumerable<NetboxDeviceTypeDto>> GetDeviceTypesAsync();

    /// <summary>
    /// Obtiene el listado de todas las máquinas virtuales registradas en Netbox.
    /// </summary>
    /// <returns>Colección de máquinas virtuales.</returns>
    Task<IEnumerable<NetboxVirtualMachineDto>> GetVirtualMachinesAsync();

    /// <summary>
    /// Obtiene el listado de todos los clusters registrados en Netbox.
    /// </summary>
    /// <returns>Colección de clusters.</returns>
    Task<IEnumerable<NetboxClusterDto>> GetClustersAsync();
}
