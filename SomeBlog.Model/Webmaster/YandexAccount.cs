using System;

namespace SomeBlog.Model
{
    public class YandexAccount : Core.ModelBase
    {
        public string Email { get; set; }
        public string WebmasterAccessToken { get; set; }
        public DateTime WebmasterAccessTokenCreate { get; set; }
        public DateTime WebmasterAccessTokenExpire { get; set; }
        public int PhoneNumberId { get; set; }

        public string Description { get; set; }
        public string Password { get; set; }
        public string RecoveryEmail { get; set; }
    }
}
