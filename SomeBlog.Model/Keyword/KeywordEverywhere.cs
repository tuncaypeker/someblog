using System;

namespace SomeBlog.Model
{
    public class KeywordEverywhere : Core.ModelBase
    {
        public KeywordEverywhere()
        {
            Competition = 0;
            Volume = 0;
        }

        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }

        public decimal Competition { get; set; }
        public decimal CpcValue { get; set; }
        public string CpcCurrency { get; set; }
        public int Volume { get; set; }
        public string Trend { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
