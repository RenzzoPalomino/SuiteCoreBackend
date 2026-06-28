using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using SuiteCoreBackend.DTOs.Notification;
using SuiteCoreBackend.Helpers;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Implementations
{
    public class NotificationChannelService : INotificationChannelService
    {
        private readonly INotificationChannelRepository _repository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly DateTimeHelper _dateHelper = new DateTimeHelper();

        public NotificationChannelService(
            INotificationChannelRepository repository,
            IMapper mapper,
            HttpClient httpClient)
        {
            _repository = repository;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NotificationChannelDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<NotificationChannelDto>>(entities);
        }

        public async Task<NotificationChannelDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<NotificationChannelDto>(entity);
        }

        public async Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelDto dto)
        {
            var entity = _mapper.Map<NotificationChannel>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<NotificationChannelDto>(created);
        }

        public async Task<NotificationChannelDto?> UpdateAsync(int id, UpdateNotificationChannelDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            return _mapper.Map<NotificationChannelDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(entity);
            return true;
        }

        public async Task<bool> SendTestNotificationAsync(int id)
        {
            var channel = await _repository.GetByIdAsync(id);
            if (channel == null || !channel.IsActive) return false;

            return await SendTelegramMessageAsync(channel.BotToken, channel.ChatId, channel.Name);
        }

        public async Task<bool> SendTestNotificationDirectAsync(TestNotificationDirectDto dto)
        {
            var channelName = string.IsNullOrWhiteSpace(dto.Name) ? "Telegram NOC" : dto.Name;
            return await SendTelegramMessageAsync(dto.BotToken, dto.ChatId, channelName);
        }

        private async Task<bool> SendTelegramMessageAsync(string botToken, string chatId, string channelName)
        {
            try
            {
                var dateStr = _dateHelper.GetPeruDateTime().ToString("dd/MM/yyyy HH:mm");
                
                // Formato exacto requerido por el usuario:
                // ✅ Prueba de notificación
                // 
                // Canal: {channelName}
                // Fecha: {dateStr}
                // Sistema de Monitoreo Operativo.
                var messageText = $"✅ Prueba de notificación\n\nCanal: {channelName}\nFecha: {dateStr}\nSistema de Monitoreo Operativo.";

                var payload = new
                {
                    chat_id = chatId,
                    text = messageText,
                    parse_mode = "HTML"
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
                var response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
