using System;

namespace SomeBlog.Model
{
    /// <summary>
    /// hangi siteye hangi siteden hangi post ne zmn eklenmis
    /// </summary>
    public class SearchEngineBotLog : Core.ModelBase
    {
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public string IpAddress { get; set; }
        public int ContentId { get; set; }
        public int BlogId { get; set; }

        /// <summary>
        /// 1- Google Bot
        /// 2- Bing
        /// 3- Yandex
        /// </summary>
        public int SearchEngine { get; set; }
    }
}
