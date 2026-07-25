using AutoMapper;
using SuiteCoreBackend.DTOs.Netbox;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Services.Implementations;

public class NetboxService : INetboxService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public NetboxService(HttpClient httpClient, IConfiguration config, IMapper mapper)
    {
        _httpClient = httpClient;
        _config = config;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NetboxIpAddressDto>> GetIpAddressesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/ip-addresses";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxIpResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxIpAddressDto>();
            }

            return _mapper.Map<List<NetboxIpAddressDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las direcciones IP desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxVlanDto>> GetVlansAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/vlans";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxVlanResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxVlanDto>();
            }

            return _mapper.Map<List<NetboxVlanDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las VLANs desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxSiteDto>> GetSitesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/sites";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxSiteResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxSiteDto>();
            }

            return _mapper.Map<List<NetboxSiteDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los sitios desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxManufacturerDto>> GetManufacturersAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/manufacturers";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxManufacturerResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxManufacturerDto>();
            }

            return _mapper.Map<List<NetboxManufacturerDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los fabricantes desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceRoleDto>> GetDeviceRolesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/device-roles";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxDeviceRoleResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxDeviceRoleDto>();
            }

            return _mapper.Map<List<NetboxDeviceRoleDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los roles de dispositivo desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceTypeDto>> GetDeviceTypesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/device-types";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxDeviceTypeResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxDeviceTypeDto>();
            }

            return _mapper.Map<List<NetboxDeviceTypeDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los tipos de dispositivo desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceDto>> GetDevicesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/devices";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxDeviceResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxDeviceDto>();
            }

            return _mapper.Map<List<NetboxDeviceDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los dispositivos desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxRackDto>> GetRacksAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/racks";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxRackResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxRackDto>();
            }

            return _mapper.Map<List<NetboxRackDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los racks desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxVirtualMachineDto>> GetVirtualMachinesAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/virtual-machines";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxVirtualMachineResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxVirtualMachineDto>();
            }

            return _mapper.Map<List<NetboxVirtualMachineDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las máquinas virtuales desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxClusterDto>> GetClustersAsync()
    {
        try
        {
            var url = _config["Scno:BaseUrl"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/v1/netbox/clusters";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxClusterResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxClusterDto>();
            }

            return _mapper.Map<List<NetboxClusterDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los clusters desde Netbox", ex);
        }
    }
}
