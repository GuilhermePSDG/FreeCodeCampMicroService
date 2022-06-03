using System.Linq.Expressions;
using Play.Common.Models;

namespace Play.Common.Repository
{
    public interface IRepository<Entity> where Entity : IEntity
    {
        public Task<List<Entity>> GetAllAsync();
        public Task<List<Entity>> GetAllAsync(Expression<Func<Entity,bool>> filter);
        public Task<Entity?> GetByIdAsync(Guid Id);
        public Task CreateAsync(Entity item);
        public Task<Entity> UpdateAsync(Entity item);
        public Task<bool> DeleteAsync(Guid Id);
    }
}