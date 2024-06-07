using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }
        public List<MotorcycleRental> MotorcycleRentals { get; set; } = new List<MotorcycleRental>();
        public decimal TotalPrice => CalculateTotalPrice();

        public BasketCart(string userName)
        {
            UserName = userName;
        }

        private decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var rental in MotorcycleRentals)
            {
                totalPrice += rental.TotalPrice;
            }
            return totalPrice;
        }
    }
}
