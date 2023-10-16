namespace SomeBlog.Model
{
    public class HttpErrorLog : Core.ModelBase
    {
        public int StatusCode { get; set; }
        public string Path { get; set; }
        public System.DateTime LastHitDate { get; set; }
        public int BlogId { get; set; }
        public string IpAddress { get; set; }
        public int HitCount { get; set; }
    }
}
