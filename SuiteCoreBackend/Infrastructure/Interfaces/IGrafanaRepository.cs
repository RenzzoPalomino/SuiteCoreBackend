using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Interfaces
{
    public interface IGrafanaRepository
    {
        public Task<List<GrafanaPanel>?> GetAll();
    }
}
