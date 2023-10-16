namespace SomeBlog.Model
{
    public class AdBannedIp : Core.ModelBase
    {
        public string IpAddress { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
