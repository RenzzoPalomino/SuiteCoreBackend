using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Notification;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/notification-channels")]
    public class NotificationChannelController : ControllerBase
    {
        private readonly INotificationChannelService _service;

        public NotificationChannelController(INotificationChannelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound(new { message = "Canal de notificación no encontrado" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationChannelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNotificationChannelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { message = "Canal de notificación no encontrado" });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound(new { message = "Canal de notificación no encontrado o no se pudo eliminar" });
            return NoContent();
        }

        [HttpPost("{id}/test")]
        public async Task<IActionResult> SendTestNotification(int id)
        {
            var success = await _service.SendTestNotificationAsync(id);
            if (!success) return BadRequest(new { message = "No se pudo enviar la prueba de notificación. Verifique que el canal esté activo y las credenciales sean correctas." });
            return Ok(new { message = "Notificación de prueba enviada con éxito" });
        }

        [HttpPost("test-direct")]
        public async Task<IActionResult> SendTestNotificationDirect([FromBody] TestNotificationDirectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _service.SendTestNotificationDirectAsync(dto);
            if (!success) return BadRequest(new { message = "No se pudo enviar la prueba de notificación. Verifique el bot token y chat id." });
            return Ok(new { message = "Notificación de prueba enviada con éxito" });
        }
    }
}
