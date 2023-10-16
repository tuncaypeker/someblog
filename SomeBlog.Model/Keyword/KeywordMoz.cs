using System;

namespace SomeBlog.Model
{
    public class KeywordMoz : Core.ModelBase
    {
        public KeywordMoz()
        {
            Difficulty = 0;
            Opportunity = 0;
            Potential = 0;
            ExactVolume = 0;
        }

        public int KeywordId { get; set; }

        public double Difficulty { get; set; }
        public double Opportunity { get; set; }
        public double Potential { get; set; }
        public double ExactVolume { get; set; }

        public DateTime CreateDate   { get; set; }
    }
}
