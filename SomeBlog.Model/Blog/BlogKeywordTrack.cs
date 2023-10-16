using System;

namespace SomeBlog.Model
{
    public class BlogKeywordTrack : Core.ModelBase
    {
        public string Date { get; set; }
        public int BlogKeywordId { get; set; }
        public double Position { get; set; }
        public double Impressions { get; set; }
        public double Clicks { get; set; }
        public double Ctr { get; set; }
        public string Device { get; set; }

        /// <summary>
        /// Enums.SeoTools
        /// </summary>
        public int Source { get; set; }
    }
}
