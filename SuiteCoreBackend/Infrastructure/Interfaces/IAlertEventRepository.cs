using System.Threading.Tasks;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Interfaces
{
    public interface IAlertEventRepository
    {
        Task<AlertEvent> CreateAsync(AlertEvent alertEvent);
    }
}
