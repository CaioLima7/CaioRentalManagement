using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly IMongoDatabase _database;
        public BasketContext(IOptions<BasketDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);

            BasketCarts = _database.GetCollection<BasketCart>(settings.Value.CollectionName);
        }

        public IMongoCollection<BasketCart> BasketCarts { get; }
    }
}
