using System;

namespace SomeBlog.Model
{
    public class KeywordTempMovement : Core.ModelBase
    {
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }

        public int BlogId { get; set; }
        public int ContentId { get; set; }

        public string Query { get; set; }
        public double BeforeRank { get; set; }
        public double CurrentRank { get; set; }
        public string Date { get; set; }
        public string Device { get; set; }
        public int Direction { get; set; }
    }
}
