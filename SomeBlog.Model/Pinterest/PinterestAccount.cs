namespace SomeBlog.Model
{
    public class PinterestAccount : Core.ModelBase
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ProfilePicture { get; set; }
        public string StateData { get; set; }
        public System.DateTime LoginDate { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Followers { get; set; }
    }
}
