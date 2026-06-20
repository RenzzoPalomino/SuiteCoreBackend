using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Interfaces
{
    public interface IUserActivityRepository
    {
        public Task<bool> RegisterLogin(UserActivity reg);
    }
}
