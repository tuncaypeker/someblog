namespace SomeBlog.Model
{
    public class AdViewLog : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime Date { get; set; }

        //Gosterilen AdCode'ların Id'leri , ile join
        public string AdCodeIds { get; set; }
    }
}
