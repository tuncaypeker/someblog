using System;

namespace SomeBlog.Model
{
    public class WordpressSiteBotHistory : Core.ModelBase
    {
        public int WordpressSiteBotId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSucceed { get; set; }
        public string Description { get; set; }
    }
}
