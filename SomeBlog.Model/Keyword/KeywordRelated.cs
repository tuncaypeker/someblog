using System;

namespace SomeBlog.Model
{
    public class KeywordRelated : Core.ModelBase
    {
        public int KeywordId { get; set; }
        public int Difficulty { get; set; }
        public double RelationLevel { get; set; }
        public int Volume { get; set; }
        public string Query { get; set; }

        /// <summary>
        /// 1 Search Console
        /// 2 Semrush
        /// </summary>
        public int Source { get; set; }

        public KeywordRelated ToList()
        {
            throw new NotImplementedException();
        }
    }
}
