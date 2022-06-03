

using MongoDB.Driver;
using Play.Catalog.Service.Models;

namespace Play.Catalog.Service.Repository
{
    public class MongoRepository<Entity> : IRepository<Entity> where Entity : IEntity
    {
        private readonly IMongoCollection<Entity> collection;
        private readonly FilterDefinitionBuilder<Entity> filter = Builders<Entity>.Filter;
        public MongoRepository(IMongoDatabase database,string CollectionName)
        {
            this.collection = database.GetCollection<Entity>(CollectionName);
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await this.collection.Find(filter.Empty).ToListAsync();
        }
        public async Task<Entity?> GetByIdAsync(Guid Id)
        {
            return await this.collection.Find(filter.Eq(x => x.Id,Id)).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Entity item) 
        {
            await this.collection.InsertOneAsync(item);
        }
        public async Task<Entity> UpdateAsync(Entity item)
        {
            return await this.collection.FindOneAndReplaceAsync(filter.Eq(x => x.Id, item.Id), item);
        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
           return await this.collection.FindOneAndDeleteAsync(filter.Eq(x => x.Id, Id)) != null;
        }

    }
}
