using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Implementations
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly SCDbContext _dbContext;

        public UserActivityRepository(SCDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<bool> RegisterLogin(UserActivity reg)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.UserActivities.Add(reg);
                    _dbContext.SaveChanges();
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                }
                return true;
            }
        }
    }
}
