using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/network")]
    [Authorize]
    public class NetworkController : ControllerBase
    {
        private readonly INetworkService _network;

        public NetworkController(INetworkService network)
        {
            _network = network;
        }

        /// <summary>Resumen general del estado de la red: dispositivos, interfaces y alertas.</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var result = await _network.GetSummaryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el resumen de red: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de barras con las alertas de red por severidad.</summary>
        [HttpGet("charts/alerts-status")]
        public async Task<IActionResult> GetAlertsStatusChart()
        {
            try
            {
                var result = await _network.GetAlertsStatusChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de alertas de red: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de dona con el estado de los dispositivos de red.</summary>
        [HttpGet("charts/devices-status")]
        public async Task<IActionResult> GetDevicesStatusChart()
        {
            try
            {
                var result = await _network.GetDevicesStatusChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de dispositivos: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de dona con el estado de las interfaces de red.</summary>
        [HttpGet("charts/interfaces-status")]
        public async Task<IActionResult> GetInterfacesStatusChart()
        {
            try
            {
                var result = await _network.GetInterfacesStatusChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de interfaces: {ex.Message}" });
            }
        }

        /// <summary>Tabla de alertas de red activas.</summary>
        [HttpGet("tables/alerts")]
        public async Task<IActionResult> GetAlertsTable()
        {
            try
            {
                var result = await _network.GetAlertsTableAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener la tabla de alertas: {ex.Message}" });
            }
        }

        /// <summary>
        /// Tabla de dispositivos de red monitoreados. Actúa como proxy directo: reenvía el código de
        /// estado y el cuerpo exactos devueltos por el SCNO, sin deserializar la respuesta.
        /// </summary>
        [HttpGet("tables/devices")]
        public async Task<IActionResult> GetDevicesTable()
        {
            using var response = await _network.GetDevicesTableRawAsync();
            return await ProxyResponseAsync(response);
        }

        /// <summary>Tabla de interfaces de red monitoreadas.</summary>
        [HttpGet("tables/interfaces")]
        public async Task<IActionResult> GetInterfacesTable()
        {
            try
            {
                var result = await _network.GetInterfacesTableAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener la tabla de interfaces: {ex.Message}" });
            }
        }

        private async Task<IActionResult> ProxyResponseAsync(HttpResponseMessage response)
        {
            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            await response.Content.CopyToAsync(Response.Body);
            return new EmptyResult();
        }
    }
}
