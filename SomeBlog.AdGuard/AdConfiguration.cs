namespace SomeBlog.AdGuard
{
    public class AdConfiguration
    {
        public AdConfiguration()
        {
            ShowAds = false;
            FirstAdAfterHowManyRequest = 0;
            HowManyAdRequestShow = 0;
        }

        public bool ShowAds { get; set; }
        public int FirstAdAfterHowManyRequest { get; set; }
        public int HowManyAdRequestShow { get; set; }
    }
}
