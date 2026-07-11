using SuiteCoreBackend.DTOs.Netbox;

namespace SuiteCoreBackend.Services.Interfaces;

public interface INetboxService
{
    Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync();
    Task<NetboxRegionDto> CreateRegionAsync(CreateNetboxRegionDto dto);
    Task<NetboxRegionDetailDto> GetRegionByIdAsync(int id);
    Task<NetboxRegionDetailDto> UpdateRegionAsync(int id, UpdateNetboxRegionDto dto);
    Task<bool> DeleteRegionAsync(int id);
    Task<IEnumerable<NetboxIpAddressDto>> GetIpAddressesAsync();
    Task<NetboxIpAddressDto> GetIpAddressByIdAsync(int id);
    Task<NetboxIpAddressDto> CreateIpAddressAsync(CreateNetboxIpAddressDto dto);
    Task<NetboxIpAddressDto> UpdateIpAddressAsync(int id, UpdateNetboxIpAddressDto dto);
    Task<bool> DeleteIpAddressAsync(int id);
    Task<IEnumerable<NetboxVlanDto>> GetVlansAsync();
    Task<NetboxVlanDto> GetVlanByIdAsync(int id);
    Task<NetboxVlanDto> CreateVlanAsync(CreateNetboxVlanDto dto);
    Task<NetboxVlanDto> UpdateVlanAsync(int id, UpdateNetboxVlanDto dto);
    Task<bool> DeleteVlanAsync(int id);
    Task<IEnumerable<NetboxCableDto>> GetCablesAsync();
    Task<NetboxCableDto> GetCableByIdAsync(int id);
    Task<NetboxCableDto> CreateCableAsync(CreateNetboxCableDto dto);
    Task<NetboxCableDto> UpdateCableAsync(int id, UpdateNetboxCableDto dto);
    Task<bool> DeleteCableAsync(int id);
    Task<IEnumerable<NetboxSiteDto>> GetSitesAsync();
    Task<NetboxSiteDto> GetSiteByIdAsync(int id);
    Task<NetboxSiteDto> CreateSiteAsync(CreateNetboxSiteDto dto);
    Task<NetboxSiteDto> UpdateSiteAsync(int id, UpdateNetboxSiteDto dto);
    Task<bool> DeleteSiteAsync(int id);
    Task<IEnumerable<NetboxModuleTypeProfileDto>> GetModuleTypeProfilesAsync();
    Task<NetboxModuleTypeProfileDto> GetModuleTypeProfileByIdAsync(int id);
    Task<NetboxModuleTypeProfileDto> CreateModuleTypeProfileAsync(CreateNetboxModuleTypeProfileDto dto);
    Task<NetboxModuleTypeProfileDto> UpdateModuleTypeProfileAsync(int id, UpdateNetboxModuleTypeProfileDto dto);
    Task<bool> DeleteModuleTypeProfileAsync(int id);
    Task<IEnumerable<NetboxManufacturerDto>> GetManufacturersAsync();
    Task<NetboxManufacturerDto> GetManufacturerByIdAsync(int id);
    Task<NetboxManufacturerDto> CreateManufacturerAsync(CreateNetboxManufacturerDto dto);
    Task<NetboxManufacturerDto> UpdateManufacturerAsync(int id, UpdateNetboxManufacturerDto dto);
    Task<bool> DeleteManufacturerAsync(int id);
    Task<IEnumerable<NetboxDeviceRoleDto>> GetDeviceRolesAsync();
    Task<NetboxDeviceRoleDto> GetDeviceRoleByIdAsync(int id);
    Task<NetboxDeviceRoleDto> CreateDeviceRoleAsync(CreateNetboxDeviceRoleDto dto);
    Task<NetboxDeviceRoleDto> UpdateDeviceRoleAsync(int id, UpdateNetboxDeviceRoleDto dto);
    Task<bool> DeleteDeviceRoleAsync(int id);
    Task<IEnumerable<NetboxDeviceDto>> GetDevicesAsync();
    Task<NetboxDeviceDto> GetDeviceByIdAsync(int id);
    Task<NetboxDeviceDto> CreateDeviceAsync(CreateNetboxDeviceDto dto);
    Task<NetboxDeviceDto> UpdateDeviceAsync(int id, UpdateNetboxDeviceDto dto);
    Task<bool> DeleteDeviceAsync(int id);
    Task<IEnumerable<NetboxRackDto>> GetRacksAsync();
    Task<NetboxRackDto> GetRackByIdAsync(int id);
    Task<NetboxRackDto> CreateRackAsync(CreateNetboxRackDto dto);
    Task<NetboxRackDto> UpdateRackAsync(int id, UpdateNetboxRackDto dto);
    Task<bool> DeleteRackAsync(int id);
}
