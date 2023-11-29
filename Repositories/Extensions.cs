using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories.Interfaces;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>()!;
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

                var client = new MongoClient(mongoDbSettings.ConnectionString);
                return client.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>()!;

                var db = serviceProvider.GetService<IMongoDatabase>()!;
                return new MongoRepository<T>(db, collectionName);
            });

            return services;
        }
    }
}