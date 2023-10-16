namespace SomeBlog.Model
{
    public class ChatGptAccount : Core.ModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string ApiKey { get; set; }
        public string Description { get; set; }
    }
}
