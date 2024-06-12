using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Entities
{
    public class Order : Entity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public decimal TotalPrice { get; set; }
    }

    public class OrderItems
    {
        public int Id { get; set; }
        public string MotorcycleId { get; set; }
        public string MotorcycleModel { get; set; }
        public decimal PricePerDay { get; set; }
        public int DaysRented { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
