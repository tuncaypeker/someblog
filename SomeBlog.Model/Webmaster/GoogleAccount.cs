namespace SomeBlog.Model
{
    public class GoogleAccount : Core.ModelBase
    {
        public string Email { get; set; }
        public string ServiceAccountCredentials { get; set; }
        public int PhoneNumberId { get; set; }

        public string Description { get; set; }
        public string Password { get; set; }
        public string RecoveryEmail { get; set; }
    }
}
