using AutoMapper;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Services.Monitoring;

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
}
