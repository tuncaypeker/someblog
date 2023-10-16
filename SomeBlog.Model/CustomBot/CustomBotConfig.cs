namespace SomeBlog.Model
{
    public class CustomBotConfig : Core.ModelBase
    {
        public int BlogId { get; set; }
        public int CustomBotId { get; set; }

        //hangi kategoriye eklenecek
        public int CategoryId { get; set; }
    }
}
