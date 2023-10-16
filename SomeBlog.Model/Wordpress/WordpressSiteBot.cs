namespace SomeBlog.Model
{
    public class WordpressSiteBot : Core.ModelBase
    {
        public int WordpressSiteId { get; set; }

        /// <summary>
        /// icerik alinacak wordpress sitenin ana adresi https://evdekorasyonlari.net gibi
        /// </summary>
        public string SitePath { get; set; }

        //* * * * * 2 gibi cron interval
        public string CronInterval { get; set; }

        /// <summary>
        /// Son kac icerik ile calismamiz gerekiyor
        /// </summary>
        public int LastPostCount { get; set; }

        public bool SpinWithOtospin { get; set; }

        /// <summary>
        /// Metin icerisinde bulunan linkleri kaldirayim mi
        /// </summary>
        public bool RemoveAnchors { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Sadece bu kategorilerden icerik alir
        /// </summary>
        public string CategoryIds { get; set; }
    }
}
