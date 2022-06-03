using Play.Catalog.Service.Models;

namespace Play.Catalog.Service.Repository
{
    public interface IRepository<Entity> where Entity : IEntity
    {
        public Task<List<Entity>> GetAllAsync();
        public Task<Entity?> GetByIdAsync(Guid Id);
        public Task CreateAsync(Entity item);
        public Task<Entity> UpdateAsync(Entity item);
        public Task<bool> DeleteAsync(Guid Id);
    }
}