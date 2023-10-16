using System;

namespace SomeBlog.Model
{
    public class KeywordSemrush : Core.ModelBase
    {
        public KeywordSemrush()
        {
            Difficulty = 0;
            Volume = 0;
        }

        public int KeywordId { get; set; }

        public int Volume { get; set; }
        public int Difficulty { get; set; }

        public DateTime CreateDate   { get; set; }
    }
}
