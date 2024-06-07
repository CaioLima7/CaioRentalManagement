namespace Basket.API.Settings
{
    public class BasketDatabaseSettings : IBasketDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
