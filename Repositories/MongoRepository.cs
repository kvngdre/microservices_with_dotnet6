using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories.Interfaces;

namespace Play.Catalog.Service.Repositories
{


    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase db, string collectionName)
        {
            dbCollection = db.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find<T>(filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(doc => doc.Id, id);

            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(doc => doc.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(doc => doc.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}