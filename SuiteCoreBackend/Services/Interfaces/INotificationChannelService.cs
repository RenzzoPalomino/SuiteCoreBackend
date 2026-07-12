using System.Collections.Generic;
using System.Threading.Tasks;
using SuiteCoreBackend.DTOs.Notification;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface INotificationChannelService
    {
        /// <summary>
        /// Obtiene el listado de todos los canales de notificación registrados.
        /// </summary>
        /// <returns>Colección de canales de notificación.</returns>
        Task<IEnumerable<NotificationChannelDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un canal de notificación por su Id.
        /// </summary>
        /// <param name="id">Id del canal.</param>
        /// <returns>Canal encontrado, o null si no existe.</returns>
        Task<NotificationChannelDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo canal de notificación (Telegram: BotToken, ChatId, Name).
        /// </summary>
        /// <param name="dto">Datos del canal a crear.</param>
        /// <returns>Canal creado.</returns>
        Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelDto dto);

        /// <summary>
        /// Actualiza un canal de notificación existente.
        /// </summary>
        /// <param name="id">Id del canal a actualizar.</param>
        /// <param name="dto">Campos a modificar.</param>
        /// <returns>Canal actualizado, o null si no existe.</returns>
        Task<NotificationChannelDto?> UpdateAsync(int id, UpdateNotificationChannelDto dto);

        /// <summary>
        /// Elimina un canal de notificación.
        /// </summary>
        /// <param name="id">Id del canal a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Envía un mensaje de prueba a un canal ya registrado, usando su configuración almacenada.
        /// </summary>
        /// <param name="id">Id del canal a probar.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendTestNotificationAsync(int id);

        /// <summary>
        /// Envía un mensaje de prueba directo sin necesidad de un canal previamente registrado,
        /// usando el BotToken y ChatId provistos en el DTO.
        /// </summary>
        /// <param name="dto">BotToken, ChatId y mensaje de prueba.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendTestNotificationDirectAsync(TestNotificationDirectDto dto);
    }
}
