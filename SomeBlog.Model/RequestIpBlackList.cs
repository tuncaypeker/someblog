using System;

namespace SomeBlog.Model
{
    public class RequestIpBlackList : Core.ModelBase
    {
        public string IpAddress { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime RemoveDate { get; set; }
    }
}
