using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common.Models;
using Play.Common.Repository;

namespace Play.Common.MongoDb
{
    public class MongoRepository<Entity> : IRepository<Entity> where Entity : IEntity
    {
        private readonly IMongoCollection<Entity> collection;
        private readonly FilterDefinitionBuilder<Entity> filter = Builders<Entity>.Filter;
        public MongoRepository(IMongoDatabase database, string CollectionName)
        {
            collection = database.GetCollection<Entity>(CollectionName);
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await collection.Find(filter.Empty).ToListAsync();
        }
        
        public async Task<List<Entity>> GetAllAsync(Expression<Func<Entity, bool>> filter)
        {
            return  await collection.Find(filter).ToListAsync();
        }
        public async Task<Entity?> GetByIdAsync(Guid Id)
        {
            return await collection.Find(filter.Eq(x => x.Id, Id)).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Entity item)
        {
            await collection.InsertOneAsync(item);
        }
        public async Task<Entity> UpdateAsync(Entity item)
        {
            return await collection.FindOneAndReplaceAsync(filter.Eq(x => x.Id, item.Id), item);
        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
            return await collection.FindOneAndDeleteAsync(filter.Eq(x => x.Id, Id)) != null;
        }

        public async Task<Entity> GetAsync(Expression<Func<Entity, bool>> filter)
        {
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
