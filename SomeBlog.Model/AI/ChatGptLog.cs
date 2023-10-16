namespace SomeBlog.Model
{
    public class ChatGptLog : Core.ModelBase
    {
        public string Prompt { get; set; }
        public string Response { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
