using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(Guid id)
        {
            return await dbCollection.Find(filterBuilder.Eq(item => item.id, id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var filter = filterBuilder.Eq(item => item.id, entity.id);
            var updates = Builders<Item>.Update.Combine(
                Builders<Item>.Update.Set("Description", entity.Description),
                Builders<Item>.Update.Set("Name", entity.Name),
                Builders<Item>.Update.Set("Price", entity.Price)
            );
            await dbCollection.UpdateOneAsync(filter, updates);
        }

        public async Task RemoveAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item=> item.id, id);
            var result = await dbCollection.DeleteOneAsync(filter);
        }
    }
}
