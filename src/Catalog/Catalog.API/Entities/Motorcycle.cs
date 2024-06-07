namespace Catalog.API.Entities
{
    public class Motorcycle
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public int Year { get; set; }
        public DateTime Created_at { get; set; }
    }


}
