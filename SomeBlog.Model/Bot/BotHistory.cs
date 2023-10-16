using System;

namespace SomeBlog.Model
{
    public class BotHistory : Core.ModelBase
    {
        public int BotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSucceed { get; set; }
    }
}
