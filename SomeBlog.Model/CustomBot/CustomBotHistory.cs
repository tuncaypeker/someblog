using System;

namespace SomeBlog.Model
{
    public class CustomBotHistory : Core.ModelBase
    {
        public int CustomBotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSucceed { get; set; }
    }
}
