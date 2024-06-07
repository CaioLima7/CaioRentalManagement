namespace Basket.API.Entities
{
    public class MotorcycleRental
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public Motorcycle Motorcycle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerDay { get; set; }
        public int DaysRented => (EndDate - StartDate).Days;
        public decimal TotalPrice => PricePerDay * DaysRented;
    }

}
