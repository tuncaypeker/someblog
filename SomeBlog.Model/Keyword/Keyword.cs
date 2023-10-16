using System;

namespace SomeBlog.Model
{
    public class Keyword : Core.ModelBase
    {
        public Keyword()
        {
            SemrushUpdateDate = new DateTime(1970, 1, 1);
            MozUpdateDate =  new DateTime(1970, 1, 1);
            AhrefsUpdateDate =  new DateTime(1970, 1, 1);
            EverywhereUpdateDate =  new DateTime(1970, 1, 1);
            Volume = 0;
            VolumeSource = -1;
        }

        public string Query { get; set; }

        public DateTime MozUpdateDate { get; set; }
        public DateTime SemrushUpdateDate { get; set; }
        public DateTime AhrefsUpdateDate { get; set; }
        public DateTime EverywhereUpdateDate { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 1 Search Console
        /// 2 Semrush
        /// 3 Mozpro
        /// 4 KeywordsEverywhere
        /// </summary>
        public int Source { get; set; }

        //Volume önemli bir veri, bu yuzden hizlica erisebilmem lazim
        //moz, keywordseverywhere, ahrefs nerden geliyorsa o en son günceller
        //ek olarak ztn kendi tablolarında da tutuyolar
        public int Volume { get; set; }
        public int VolumeSource { get; set; }
    }
}
