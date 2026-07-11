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

    public async Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxRegionDto>();
            }

            //return netboxResponse.Results.Select(r => new NetboxRegionDto
            //{
            //    Name = r.Name,
            //    SiteCount = r.SiteCount,
            //    Description = r.Description
            //}).ToList();
            return _mapper.Map<List<NetboxRegionDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las regiones desde Netbox", ex);
        }
    }

    public async Task<NetboxRegionDto> CreateRegionAsync(CreateNetboxRegionDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdRegionResult = JsonSerializer.Deserialize<NetboxRegionResult>(jsonResponse, options);

            if (createdRegionResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de región.");
            }

            return _mapper.Map<NetboxRegionDto>(createdRegionResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear la región en Netbox", ex);
        }
    }

    public async Task<NetboxRegionDetailDto> GetRegionByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la región con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var regionResult = JsonSerializer.Deserialize<NetboxRegionResult>(json, options);

            if (regionResult == null)
            {
                throw new Exception("Error al deserializar la región obtenida desde Netbox.");
            }

            return _mapper.Map<NetboxRegionDetailDto>(regionResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la región con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxRegionDetailDto> UpdateRegionAsync(int id, UpdateNetboxRegionDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la región con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedRegion = JsonSerializer.Deserialize<NetboxRegionResult>(jsonResponse, options);

            if (updatedRegion == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de región.");
            }

            return _mapper.Map<NetboxRegionDetailDto>(updatedRegion);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar la región con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteRegionAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/regions/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró la región con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la región con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxIpAddressDto>> GetIpAddressesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/ip-addresses/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxIpAddressDto> GetIpAddressByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/ip-addresses/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la dirección IP con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var ipResult = JsonSerializer.Deserialize<NetboxIpAddressResult>(json, options);

            if (ipResult == null)
            {
                throw new Exception("Error al deserializar la dirección IP obtenida desde Netbox.");
            }

            return _mapper.Map<NetboxIpAddressDto>(ipResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la dirección IP con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxIpAddressDto> CreateIpAddressAsync(CreateNetboxIpAddressDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/ip-addresses/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdIpResult = JsonSerializer.Deserialize<NetboxIpAddressResult>(jsonResponse, options);

            if (createdIpResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de dirección IP.");
            }

            return _mapper.Map<NetboxIpAddressDto>(createdIpResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear la dirección IP en Netbox", ex);
        }
    }

    public async Task<NetboxIpAddressDto> UpdateIpAddressAsync(int id, UpdateNetboxIpAddressDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/ip-addresses/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la dirección IP con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedIpResult = JsonSerializer.Deserialize<NetboxIpAddressResult>(jsonResponse, options);

            if (updatedIpResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de dirección IP.");
            }

            return _mapper.Map<NetboxIpAddressDto>(updatedIpResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar la dirección IP con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteIpAddressAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/ip-addresses/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró la dirección IP con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la dirección IP con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxVlanDto>> GetVlansAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/vlans/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxVlanDto> GetVlanByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/vlans/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la VLAN con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var vlanResult = JsonSerializer.Deserialize<NetboxVlanResult>(json, options);

            if (vlanResult == null)
            {
                throw new Exception("Error al deserializar la VLAN obtenida desde Netbox.");
            }

            return _mapper.Map<NetboxVlanDto>(vlanResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la VLAN con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxVlanDto> CreateVlanAsync(CreateNetboxVlanDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/vlans/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdVlanResult = JsonSerializer.Deserialize<NetboxVlanResult>(jsonResponse, options);

            if (createdVlanResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de VLAN.");
            }

            return _mapper.Map<NetboxVlanDto>(createdVlanResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear la VLAN en Netbox", ex);
        }
    }

    public async Task<NetboxVlanDto> UpdateVlanAsync(int id, UpdateNetboxVlanDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/vlans/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la VLAN con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedVlanResult = JsonSerializer.Deserialize<NetboxVlanResult>(jsonResponse, options);

            if (updatedVlanResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de VLAN.");
            }

            return _mapper.Map<NetboxVlanDto>(updatedVlanResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar la VLAN con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteVlanAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/ipam/vlans/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró la VLAN con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la VLAN con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxCableDto>> GetCablesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/cables/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxCableResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxCableDto>();
            }

            return _mapper.Map<List<NetboxCableDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los cables desde Netbox", ex);
        }
    }

    public async Task<NetboxCableDto> GetCableByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/cables/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el cable con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cableResult = JsonSerializer.Deserialize<NetboxCableResult>(json, options);

            if (cableResult == null)
            {
                throw new Exception("Error al deserializar el cable obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxCableDto>(cableResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el cable con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxCableDto> CreateCableAsync(CreateNetboxCableDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/cables/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdCableResult = JsonSerializer.Deserialize<NetboxCableResult>(jsonResponse, options);

            if (createdCableResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de cable.");
            }

            return _mapper.Map<NetboxCableDto>(createdCableResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el cable en Netbox", ex);
        }
    }

    public async Task<NetboxCableDto> UpdateCableAsync(int id, UpdateNetboxCableDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/cables/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el cable con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedCableResult = JsonSerializer.Deserialize<NetboxCableResult>(jsonResponse, options);

            if (updatedCableResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de cable.");
            }

            return _mapper.Map<NetboxCableDto>(updatedCableResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el cable con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteCableAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/cables/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el cable con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el cable con ID {id} en Netbox", ex);
        }
    }

    public async Task<NetboxSiteDto> GetSiteByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/sites/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el sitio con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var siteResult = JsonSerializer.Deserialize<NetboxSiteResult>(json, options);

            if (siteResult == null)
            {
                throw new Exception("Error al deserializar el sitio obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxSiteDto>(siteResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el sitio con ID {id} desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxSiteDto>> GetSitesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/sites/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxSiteDto> CreateSiteAsync(CreateNetboxSiteDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/sites/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdSiteResult = JsonSerializer.Deserialize<NetboxSiteResult>(jsonResponse, options);

            if (createdSiteResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de sitio.");
            }

            return _mapper.Map<NetboxSiteDto>(createdSiteResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el sitio en Netbox", ex);
        }
    }

    public async Task<NetboxSiteDto> UpdateSiteAsync(int id, UpdateNetboxSiteDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/sites/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el sitio con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedSiteResult = JsonSerializer.Deserialize<NetboxSiteResult>(jsonResponse, options);

            if (updatedSiteResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de sitio.");
            }

            return _mapper.Map<NetboxSiteDto>(updatedSiteResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el sitio con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteSiteAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/sites/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el sitio con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el sitio con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxModuleTypeProfileDto>> GetModuleTypeProfilesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/module-type-profiles/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var netboxResponse = JsonSerializer.Deserialize<NetboxModuleTypeProfileResponse>(json, options);

            if (netboxResponse?.Results == null)
            {
                return Enumerable.Empty<NetboxModuleTypeProfileDto>();
            }

            return _mapper.Map<List<NetboxModuleTypeProfileDto>>(netboxResponse.Results);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los perfiles de tipo de módulo desde Netbox", ex);
        }
    }

    public async Task<NetboxModuleTypeProfileDto> GetModuleTypeProfileByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/module-type-profiles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el perfil de tipo de módulo con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var profileResult = JsonSerializer.Deserialize<NetboxModuleTypeProfileResult>(json, options);

            if (profileResult == null)
            {
                throw new Exception("Error al deserializar el perfil de tipo de módulo obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxModuleTypeProfileDto>(profileResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el perfil de tipo de módulo con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxModuleTypeProfileDto> CreateModuleTypeProfileAsync(CreateNetboxModuleTypeProfileDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/module-type-profiles/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdProfileResult = JsonSerializer.Deserialize<NetboxModuleTypeProfileResult>(jsonResponse, options);

            if (createdProfileResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de perfil de tipo de módulo.");
            }

            return _mapper.Map<NetboxModuleTypeProfileDto>(createdProfileResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el perfil de tipo de módulo en Netbox", ex);
        }
    }

    public async Task<NetboxModuleTypeProfileDto> UpdateModuleTypeProfileAsync(int id, UpdateNetboxModuleTypeProfileDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/module-type-profiles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el perfil de tipo de módulo con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedProfileResult = JsonSerializer.Deserialize<NetboxModuleTypeProfileResult>(jsonResponse, options);

            if (updatedProfileResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de perfil de tipo de módulo.");
            }

            return _mapper.Map<NetboxModuleTypeProfileDto>(updatedProfileResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el perfil de tipo de módulo con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteModuleTypeProfileAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/module-type-profiles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el perfil de tipo de módulo con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el perfil de tipo de módulo con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxManufacturerDto>> GetManufacturersAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/manufacturers/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxManufacturerDto> GetManufacturerByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/manufacturers/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el fabricante con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var manufacturerResult = JsonSerializer.Deserialize<NetboxManufacturerResult>(json, options);

            if (manufacturerResult == null)
            {
                throw new Exception("Error al deserializar el fabricante obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxManufacturerDto>(manufacturerResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el fabricante con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxManufacturerDto> CreateManufacturerAsync(CreateNetboxManufacturerDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/manufacturers/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdManufacturerResult = JsonSerializer.Deserialize<NetboxManufacturerResult>(jsonResponse, options);

            if (createdManufacturerResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de fabricante.");
            }

            return _mapper.Map<NetboxManufacturerDto>(createdManufacturerResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el fabricante en Netbox", ex);
        }
    }

    public async Task<NetboxManufacturerDto> UpdateManufacturerAsync(int id, UpdateNetboxManufacturerDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/manufacturers/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el fabricante con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedManufacturerResult = JsonSerializer.Deserialize<NetboxManufacturerResult>(jsonResponse, options);

            if (updatedManufacturerResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de fabricante.");
            }

            return _mapper.Map<NetboxManufacturerDto>(updatedManufacturerResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el fabricante con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteManufacturerAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/manufacturers/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el fabricante con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el fabricante con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceRoleDto>> GetDeviceRolesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-roles/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxDeviceRoleDto> GetDeviceRoleByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-roles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el rol de dispositivo con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var roleResult = JsonSerializer.Deserialize<NetboxDeviceRoleResult>(json, options);

            if (roleResult == null)
            {
                throw new Exception("Error al deserializar el rol de dispositivo obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxDeviceRoleDto>(roleResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el rol de dispositivo con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxDeviceRoleDto> CreateDeviceRoleAsync(CreateNetboxDeviceRoleDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-roles/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdRoleResult = JsonSerializer.Deserialize<NetboxDeviceRoleResult>(jsonResponse, options);

            if (createdRoleResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de rol de dispositivo.");
            }

            return _mapper.Map<NetboxDeviceRoleDto>(createdRoleResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el rol de dispositivo en Netbox", ex);
        }
    }

    public async Task<NetboxDeviceRoleDto> UpdateDeviceRoleAsync(int id, UpdateNetboxDeviceRoleDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-roles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el rol de dispositivo con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedRoleResult = JsonSerializer.Deserialize<NetboxDeviceRoleResult>(jsonResponse, options);

            if (updatedRoleResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de rol de dispositivo.");
            }

            return _mapper.Map<NetboxDeviceRoleDto>(updatedRoleResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el rol de dispositivo con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteDeviceRoleAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-roles/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el rol de dispositivo con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el rol de dispositivo con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceTypeDto>> GetDeviceTypesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-types/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxDeviceTypeDto> GetDeviceTypeByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/device-types/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el tipo de dispositivo con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var deviceTypeResult = JsonSerializer.Deserialize<NetboxDeviceTypeResult>(json, options);

            if (deviceTypeResult == null)
            {
                throw new Exception("Error al deserializar el tipo de dispositivo obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxDeviceTypeDto>(deviceTypeResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el tipo de dispositivo con ID {id} desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxDeviceDto>> GetDevicesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/devices/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxDeviceDto> GetDeviceByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/devices/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el dispositivo con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var deviceResult = JsonSerializer.Deserialize<NetboxDeviceResult>(json, options);

            if (deviceResult == null)
            {
                throw new Exception("Error al deserializar el dispositivo obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxDeviceDto>(deviceResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el dispositivo con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxDeviceDto> CreateDeviceAsync(CreateNetboxDeviceDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/devices/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdDeviceResult = JsonSerializer.Deserialize<NetboxDeviceResult>(jsonResponse, options);

            if (createdDeviceResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de dispositivo.");
            }

            return _mapper.Map<NetboxDeviceDto>(createdDeviceResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el dispositivo en Netbox", ex);
        }
    }

    public async Task<NetboxDeviceDto> UpdateDeviceAsync(int id, UpdateNetboxDeviceDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/devices/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el dispositivo con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedDeviceResult = JsonSerializer.Deserialize<NetboxDeviceResult>(jsonResponse, options);

            if (updatedDeviceResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de dispositivo.");
            }

            return _mapper.Map<NetboxDeviceDto>(updatedDeviceResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el dispositivo con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/devices/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el dispositivo con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el dispositivo con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxRackDto>> GetRacksAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/racks/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxRackDto> GetRackByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/racks/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el rack con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var rackResult = JsonSerializer.Deserialize<NetboxRackResult>(json, options);

            if (rackResult == null)
            {
                throw new Exception("Error al deserializar el rack obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxRackDto>(rackResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el rack con ID {id} desde Netbox", ex);
        }
    }

    public async Task<NetboxRackDto> CreateRackAsync(CreateNetboxRackDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/racks/";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var createdRackResult = JsonSerializer.Deserialize<NetboxRackResult>(jsonResponse, options);

            if (createdRackResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de creación de rack.");
            }

            return _mapper.Map<NetboxRackDto>(createdRackResult);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el rack en Netbox", ex);
        }
    }

    public async Task<NetboxRackDto> UpdateRackAsync(int id, UpdateNetboxRackDto dto)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/racks/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var jsonPayload = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el rack con ID {id} para actualizar en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var updatedRackResult = JsonSerializer.Deserialize<NetboxRackResult>(jsonResponse, options);

            if (updatedRackResult == null)
            {
                throw new Exception("Error al deserializar la respuesta de actualización de rack.");
            }

            return _mapper.Map<NetboxRackDto>(updatedRackResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el rack con ID {id} en Netbox", ex);
        }
    }

    public async Task<bool> DeleteRackAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/dcim/racks/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"No se encontró el rack con ID {id} en Netbox.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox al eliminar: {errorContent}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el rack con ID {id} en Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxVirtualMachineDto>> GetVirtualMachinesAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/virtualization/virtual-machines/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxVirtualMachineDto> GetVirtualMachineByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/virtualization/virtual-machines/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró la máquina virtual con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var vmResult = JsonSerializer.Deserialize<NetboxVirtualMachineResult>(json, options);

            if (vmResult == null)
            {
                throw new Exception("Error al deserializar la máquina virtual obtenida desde Netbox.");
            }

            return _mapper.Map<NetboxVirtualMachineDto>(vmResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la máquina virtual con ID {id} desde Netbox", ex);
        }
    }

    public async Task<IEnumerable<NetboxClusterDto>> GetClustersAsync()
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/virtualization/clusters/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

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

    public async Task<NetboxClusterDto> GetClusterByIdAsync(int id)
    {
        try
        {
            var url = _config["Netbox:Url"];
            var token = _config["Netbox:Token"];

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("La URL de Netbox no está configurada en appsettings.json.");
            }

            var requestUrl = $"{url.TrimEnd('/')}/api/virtualization/clusters/{id}/";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"No se encontró el cluster con ID {id} en Netbox.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de Netbox: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var clusterResult = JsonSerializer.Deserialize<NetboxClusterResult>(json, options);

            if (clusterResult == null)
            {
                throw new Exception("Error al deserializar el cluster obtenido desde Netbox.");
            }

            return _mapper.Map<NetboxClusterDto>(clusterResult);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el cluster con ID {id} desde Netbox", ex);
        }
    }
}
