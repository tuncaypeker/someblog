using System.Collections.Generic;

namespace SomeBlog.AdGuard.Dto
{
    public class ShouldShowAdDto
    {
        public ShouldShowAdDto(bool shouldShowAd)
        {
            AdCodeViewCounts = new Dictionary<int, int>();
            ShouldShowAd = shouldShowAd;
        }

        /// <summary>
        /// Reklam Gostermeli mi
        /// </summary>
        public bool ShouldShowAd { get; set; }

        /// <summary>
        /// Her bir adCode bugun bu ip icin kac kere gosterilmis
        /// AdViewLog bilgisi buarda alindigi icin, bu bilgi dto ile donuyor
        /// 
        /// [id,count],[id,count],[id,count],[id,count]
        /// 
        /// Bu bilgi, adCode bazli gosterim kisitlamasi icin kullaniliyor
        /// </summary>
        public Dictionary<int,int> AdCodeViewCounts { get; set; }
    }
}
