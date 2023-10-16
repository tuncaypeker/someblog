using System;

namespace SomeBlog.Model
{
    public class KeywordEverywhereQuestion : Core.ModelBase
    {
        public int KeywordEverywhereId { get; set; }
        public string Question { get; set; }
        public int Volume { get; set; }
    }
}
