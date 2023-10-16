namespace SomeBlog.Model
{
    public class WordpressSite : Core.ModelBase
    {
        public string Path { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
