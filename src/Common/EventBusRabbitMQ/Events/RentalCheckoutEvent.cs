using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class RentalCheckoutEvent
    {
        public Guid RequestId { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public List<RentalItem> RentalItems { get; set; }

        public RentalCheckoutEvent()
        {
            RentalItems = new List<RentalItem>();
        }
    }

    public class RentalItem
    {
        public string MotorcycleId { get; set; }
        public string MotorcycleModel { get; set; }
        public decimal PricePerDay { get; set; }
        public int DaysRented { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
