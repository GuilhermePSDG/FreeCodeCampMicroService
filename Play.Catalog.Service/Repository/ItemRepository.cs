

using MongoDB.Driver;
using Play.Catalog.Service.Models;

namespace Play.Catalog.Service.Repository
{
    public class ItemRepository
    {
        private const string CollectionName = "items";
        private readonly IMongoCollection<Item> collection;
        private readonly FilterDefinitionBuilder<Item> filter = Builders<Item>.Filter;
        public ItemRepository(MongoClient client)
        {
            this.collection = client.GetDatabase("Catalog").GetCollection<Item>(CollectionName);
        }

        public async Task<List<Item>> GetAll()
        {
            return await this.collection.Find(filter.Empty).ToListAsync();
        }
        public async Task<Item?> GetById(Guid Id)
        {
            return await this.collection.Find(filter.Eq(x => x.Id,Id)).FirstOrDefaultAsync();
        }
        public async Task Create(Item item) 
        {
            await this.collection.InsertOneAsync(item);
        }
        public async Task<Item> Update(Item item)
        {
            return await this.collection.FindOneAndReplaceAsync(filter.Eq(x => x.Id, item.Id), item);
        }
        public async Task<bool> Delete(Guid Id)
        {
           return await this.collection.FindOneAndDeleteAsync(filter.Eq(x => x.Id, Id)) != null;
        }

    }
}
