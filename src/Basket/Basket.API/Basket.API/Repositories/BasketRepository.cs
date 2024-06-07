using Basket.API.Repositories.Interfaces;
using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{


    public class BasketRepository : IBasketRepository
    {
        private readonly IMongoCollection<BasketCart> _basketCollection;

        public BasketRepository(IBasketContext context)
        {
            _basketCollection = context.BasketCarts ?? throw new ArgumentNullException(nameof(context.BasketCarts));
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            return await _basketCollection.Find(basket => basket.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var result = await _basketCollection.ReplaceOneAsync(
                filter: g => g.UserName == basket.UserName,
                replacement: basket,
                options: new ReplaceOptions { IsUpsert = true });

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return await GetBasket(basket.UserName);
            }
            return null;
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            var result = await _basketCollection.DeleteOneAsync(basket => basket.UserName == userName);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }

}
