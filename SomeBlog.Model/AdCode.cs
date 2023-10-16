namespace SomeBlog.Model
{
    public class AdCode : Core.ModelBase
    {
        public int BlogId { get; set; }
        public int AdAccountId { get; set; } 
        public string Code { get; set; }
        public string Key { get; set; } //fore ex. ad_m_300x600

        public bool MobileOnly { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Gun icerisinde ip bazli olarak kac kez gosterilecegini ayarlar
        /// 0 set edilirse code bazli limit yok kabul edilir
        /// 
        /// Or: Mobil buyuk reklam sayfa gezinmesini etkiledigi icin gunde sadece 1 kere goster ayarini adCode bazli yapariz
        /// Bu durumda Config dosyasinda 10 request'e kadar reklam goster olayindan bagimsiz olarak sadece bu reklam kodunu 1 kez goster
        /// Ayarlamasina imkan verir
        /// </summary>
        public int DailyViewCount { get; set; }
    }
}
