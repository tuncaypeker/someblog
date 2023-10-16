using System;

namespace SomeBlog.Model
{
    public class SearchEngineSubmitLog : Core.ModelBase
    {
        public SearchEngineSubmitLog()
        {
            SubmitBy = 1;
        }

        /// <summary>
        /// 1- Google Bot
        /// 2- Bing
        /// 3- Yandex
        /// </summary>
        public int SearchEngine { get; set; }
        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 1- Manuel
        /// 2- Otomatik
        /// </summary>
        public int SubmitBy { get; set; }
    }
}
