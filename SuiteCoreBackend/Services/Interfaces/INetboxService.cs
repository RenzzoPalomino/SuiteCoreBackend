using SuiteCoreBackend.DTOs.Netbox;

namespace SuiteCoreBackend.Services.Interfaces;

public interface INetboxService
{
    /// <summary>
    /// Obtiene el listado de todas las regiones registradas en Netbox.
    /// </summary>
    /// <returns>Colección de regiones.</returns>
    Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync();

    /// <summary>
    /// Crea una nueva región en Netbox.
    /// </summary>
    /// <param name="dto">Datos de la región a crear.</param>
    /// <returns>Región creada.</returns>
    Task<NetboxRegionDto> CreateRegionAsync(CreateNetboxRegionDto dto);

    /// <summary>
    /// Obtiene el detalle de una región por su Id.
    /// </summary>
    /// <param name="id">Id de la región en Netbox.</param>
    /// <returns>Detalle de la región.</returns>
    Task<NetboxRegionDetailDto> GetRegionByIdAsync(int id);

    /// <summary>
    /// Actualiza una región existente.
    /// </summary>
    /// <param name="id">Id de la región a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Región actualizada.</returns>
    Task<NetboxRegionDetailDto> UpdateRegionAsync(int id, UpdateNetboxRegionDto dto);

    /// <summary>
    /// Elimina una región de Netbox.
    /// </summary>
    /// <param name="id">Id de la región a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteRegionAsync(int id);

    /// <summary>
    /// Obtiene el listado de todas las direcciones IP registradas en Netbox.
    /// </summary>
    /// <returns>Colección de direcciones IP.</returns>
    Task<IEnumerable<NetboxIpAddressDto>> GetIpAddressesAsync();

    /// <summary>
    /// Obtiene una dirección IP por su Id.
    /// </summary>
    /// <param name="id">Id de la dirección IP.</param>
    /// <returns>Dirección IP encontrada.</returns>
    Task<NetboxIpAddressDto> GetIpAddressByIdAsync(int id);

    /// <summary>
    /// Crea una nueva dirección IP en Netbox.
    /// </summary>
    /// <param name="dto">Datos de la dirección IP a crear.</param>
    /// <returns>Dirección IP creada.</returns>
    Task<NetboxIpAddressDto> CreateIpAddressAsync(CreateNetboxIpAddressDto dto);

    /// <summary>
    /// Actualiza una dirección IP existente.
    /// </summary>
    /// <param name="id">Id de la dirección IP a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Dirección IP actualizada.</returns>
    Task<NetboxIpAddressDto> UpdateIpAddressAsync(int id, UpdateNetboxIpAddressDto dto);

    /// <summary>
    /// Elimina una dirección IP de Netbox.
    /// </summary>
    /// <param name="id">Id de la dirección IP a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteIpAddressAsync(int id);

    /// <summary>
    /// Obtiene el listado de todas las VLANs registradas en Netbox.
    /// </summary>
    /// <returns>Colección de VLANs.</returns>
    Task<IEnumerable<NetboxVlanDto>> GetVlansAsync();

    /// <summary>
    /// Obtiene una VLAN por su Id.
    /// </summary>
    /// <param name="id">Id de la VLAN.</param>
    /// <returns>VLAN encontrada.</returns>
    Task<NetboxVlanDto> GetVlanByIdAsync(int id);

    /// <summary>
    /// Crea una nueva VLAN en Netbox.
    /// </summary>
    /// <param name="dto">Datos de la VLAN a crear.</param>
    /// <returns>VLAN creada.</returns>
    Task<NetboxVlanDto> CreateVlanAsync(CreateNetboxVlanDto dto);

    /// <summary>
    /// Actualiza una VLAN existente.
    /// </summary>
    /// <param name="id">Id de la VLAN a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>VLAN actualizada.</returns>
    Task<NetboxVlanDto> UpdateVlanAsync(int id, UpdateNetboxVlanDto dto);

    /// <summary>
    /// Elimina una VLAN de Netbox.
    /// </summary>
    /// <param name="id">Id de la VLAN a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteVlanAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los cables registrados en Netbox.
    /// </summary>
    /// <returns>Colección de cables.</returns>
    Task<IEnumerable<NetboxCableDto>> GetCablesAsync();

    /// <summary>
    /// Obtiene un cable por su Id.
    /// </summary>
    /// <param name="id">Id del cable.</param>
    /// <returns>Cable encontrado.</returns>
    Task<NetboxCableDto> GetCableByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo cable en Netbox.
    /// </summary>
    /// <param name="dto">Datos del cable a crear.</param>
    /// <returns>Cable creado.</returns>
    Task<NetboxCableDto> CreateCableAsync(CreateNetboxCableDto dto);

    /// <summary>
    /// Actualiza un cable existente.
    /// </summary>
    /// <param name="id">Id del cable a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Cable actualizado.</returns>
    Task<NetboxCableDto> UpdateCableAsync(int id, UpdateNetboxCableDto dto);

    /// <summary>
    /// Elimina un cable de Netbox.
    /// </summary>
    /// <param name="id">Id del cable a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteCableAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los sitios registrados en Netbox.
    /// </summary>
    /// <returns>Colección de sitios.</returns>
    Task<IEnumerable<NetboxSiteDto>> GetSitesAsync();

    /// <summary>
    /// Obtiene un sitio por su Id.
    /// </summary>
    /// <param name="id">Id del sitio.</param>
    /// <returns>Sitio encontrado.</returns>
    Task<NetboxSiteDto> GetSiteByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo sitio en Netbox.
    /// </summary>
    /// <param name="dto">Datos del sitio a crear.</param>
    /// <returns>Sitio creado.</returns>
    Task<NetboxSiteDto> CreateSiteAsync(CreateNetboxSiteDto dto);

    /// <summary>
    /// Actualiza un sitio existente.
    /// </summary>
    /// <param name="id">Id del sitio a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Sitio actualizado.</returns>
    Task<NetboxSiteDto> UpdateSiteAsync(int id, UpdateNetboxSiteDto dto);

    /// <summary>
    /// Elimina un sitio de Netbox.
    /// </summary>
    /// <param name="id">Id del sitio a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteSiteAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los module type profiles registrados en Netbox.
    /// </summary>
    /// <returns>Colección de module type profiles.</returns>
    Task<IEnumerable<NetboxModuleTypeProfileDto>> GetModuleTypeProfilesAsync();

    /// <summary>
    /// Obtiene un module type profile por su Id.
    /// </summary>
    /// <param name="id">Id del module type profile.</param>
    /// <returns>Module type profile encontrado.</returns>
    Task<NetboxModuleTypeProfileDto> GetModuleTypeProfileByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo module type profile en Netbox.
    /// </summary>
    /// <param name="dto">Datos del module type profile a crear.</param>
    /// <returns>Module type profile creado.</returns>
    Task<NetboxModuleTypeProfileDto> CreateModuleTypeProfileAsync(CreateNetboxModuleTypeProfileDto dto);

    /// <summary>
    /// Actualiza un module type profile existente.
    /// </summary>
    /// <param name="id">Id del module type profile a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Module type profile actualizado.</returns>
    Task<NetboxModuleTypeProfileDto> UpdateModuleTypeProfileAsync(int id, UpdateNetboxModuleTypeProfileDto dto);

    /// <summary>
    /// Elimina un module type profile de Netbox.
    /// </summary>
    /// <param name="id">Id del module type profile a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteModuleTypeProfileAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los fabricantes registrados en Netbox.
    /// </summary>
    /// <returns>Colección de fabricantes.</returns>
    Task<IEnumerable<NetboxManufacturerDto>> GetManufacturersAsync();

    /// <summary>
    /// Obtiene un fabricante por su Id.
    /// </summary>
    /// <param name="id">Id del fabricante.</param>
    /// <returns>Fabricante encontrado.</returns>
    Task<NetboxManufacturerDto> GetManufacturerByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo fabricante en Netbox.
    /// </summary>
    /// <param name="dto">Datos del fabricante a crear.</param>
    /// <returns>Fabricante creado.</returns>
    Task<NetboxManufacturerDto> CreateManufacturerAsync(CreateNetboxManufacturerDto dto);

    /// <summary>
    /// Actualiza un fabricante existente.
    /// </summary>
    /// <param name="id">Id del fabricante a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Fabricante actualizado.</returns>
    Task<NetboxManufacturerDto> UpdateManufacturerAsync(int id, UpdateNetboxManufacturerDto dto);

    /// <summary>
    /// Elimina un fabricante de Netbox.
    /// </summary>
    /// <param name="id">Id del fabricante a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteManufacturerAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los roles de dispositivo registrados en Netbox.
    /// </summary>
    /// <returns>Colección de roles de dispositivo.</returns>
    Task<IEnumerable<NetboxDeviceRoleDto>> GetDeviceRolesAsync();

    /// <summary>
    /// Obtiene un rol de dispositivo por su Id.
    /// </summary>
    /// <param name="id">Id del rol de dispositivo.</param>
    /// <returns>Rol de dispositivo encontrado.</returns>
    Task<NetboxDeviceRoleDto> GetDeviceRoleByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo rol de dispositivo en Netbox.
    /// </summary>
    /// <param name="dto">Datos del rol de dispositivo a crear.</param>
    /// <returns>Rol de dispositivo creado.</returns>
    Task<NetboxDeviceRoleDto> CreateDeviceRoleAsync(CreateNetboxDeviceRoleDto dto);

    /// <summary>
    /// Actualiza un rol de dispositivo existente.
    /// </summary>
    /// <param name="id">Id del rol de dispositivo a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Rol de dispositivo actualizado.</returns>
    Task<NetboxDeviceRoleDto> UpdateDeviceRoleAsync(int id, UpdateNetboxDeviceRoleDto dto);

    /// <summary>
    /// Elimina un rol de dispositivo de Netbox.
    /// </summary>
    /// <param name="id">Id del rol de dispositivo a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteDeviceRoleAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los dispositivos registrados en Netbox.
    /// </summary>
    /// <returns>Colección de dispositivos.</returns>
    Task<IEnumerable<NetboxDeviceDto>> GetDevicesAsync();

    /// <summary>
    /// Obtiene un dispositivo por su Id.
    /// </summary>
    /// <param name="id">Id del dispositivo.</param>
    /// <returns>Dispositivo encontrado.</returns>
    Task<NetboxDeviceDto> GetDeviceByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo dispositivo en Netbox.
    /// </summary>
    /// <param name="dto">Datos del dispositivo a crear.</param>
    /// <returns>Dispositivo creado.</returns>
    Task<NetboxDeviceDto> CreateDeviceAsync(CreateNetboxDeviceDto dto);

    /// <summary>
    /// Actualiza un dispositivo existente.
    /// </summary>
    /// <param name="id">Id del dispositivo a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Dispositivo actualizado.</returns>
    Task<NetboxDeviceDto> UpdateDeviceAsync(int id, UpdateNetboxDeviceDto dto);

    /// <summary>
    /// Elimina un dispositivo de Netbox.
    /// </summary>
    /// <param name="id">Id del dispositivo a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteDeviceAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los racks registrados en Netbox.
    /// </summary>
    /// <returns>Colección de racks.</returns>
    Task<IEnumerable<NetboxRackDto>> GetRacksAsync();

    /// <summary>
    /// Obtiene un rack por su Id.
    /// </summary>
    /// <param name="id">Id del rack.</param>
    /// <returns>Rack encontrado.</returns>
    Task<NetboxRackDto> GetRackByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo rack en Netbox.
    /// </summary>
    /// <param name="dto">Datos del rack a crear.</param>
    /// <returns>Rack creado.</returns>
    Task<NetboxRackDto> CreateRackAsync(CreateNetboxRackDto dto);

    /// <summary>
    /// Actualiza un rack existente.
    /// </summary>
    /// <param name="id">Id del rack a actualizar.</param>
    /// <param name="dto">Campos a modificar.</param>
    /// <returns>Rack actualizado.</returns>
    Task<NetboxRackDto> UpdateRackAsync(int id, UpdateNetboxRackDto dto);

    /// <summary>
    /// Elimina un rack de Netbox.
    /// </summary>
    /// <param name="id">Id del rack a eliminar.</param>
    /// <returns>True si la eliminación fue exitosa.</returns>
    Task<bool> DeleteRackAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los tipos de dispositivo registrados en Netbox.
    /// </summary>
    /// <returns>Colección de tipos de dispositivo.</returns>
    Task<IEnumerable<NetboxDeviceTypeDto>> GetDeviceTypesAsync();

    /// <summary>
    /// Obtiene un tipo de dispositivo por su Id.
    /// </summary>
    /// <param name="id">Id del tipo de dispositivo.</param>
    /// <returns>Tipo de dispositivo encontrado.</returns>
    Task<NetboxDeviceTypeDto> GetDeviceTypeByIdAsync(int id);

    /// <summary>
    /// Obtiene el listado de todas las máquinas virtuales registradas en Netbox.
    /// </summary>
    /// <returns>Colección de máquinas virtuales.</returns>
    Task<IEnumerable<NetboxVirtualMachineDto>> GetVirtualMachinesAsync();

    /// <summary>
    /// Obtiene una máquina virtual por su Id.
    /// </summary>
    /// <param name="id">Id de la máquina virtual.</param>
    /// <returns>Máquina virtual encontrada.</returns>
    Task<NetboxVirtualMachineDto> GetVirtualMachineByIdAsync(int id);

    /// <summary>
    /// Obtiene el listado de todos los clusters registrados en Netbox.
    /// </summary>
    /// <returns>Colección de clusters.</returns>
    Task<IEnumerable<NetboxClusterDto>> GetClustersAsync();

    /// <summary>
    /// Obtiene un cluster por su Id.
    /// </summary>
    /// <param name="id">Id del cluster.</param>
    /// <returns>Cluster encontrado.</returns>
    Task<NetboxClusterDto> GetClusterByIdAsync(int id);
}
