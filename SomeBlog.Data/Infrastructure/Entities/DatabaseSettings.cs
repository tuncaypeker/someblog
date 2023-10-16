namespace SomeBlog.Data.Infrastructure.Entities
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string PoolConnectionString { get; set; }
        public string ContentConnectionString { get; set; }
    }
}
