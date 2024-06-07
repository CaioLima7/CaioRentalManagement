using Basket.API.Entities;
using MongoDB.Driver;

namespace Basket.API.Data.Interfaces
{
    public interface IBasketContext
    {
        IMongoCollection<BasketCart> BasketCarts { get; }
    }
}
