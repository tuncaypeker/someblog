using System;

namespace SomeBlog.Model
{
    public class JobLogItem : Core.ModelBase
    {
        public int JobLogId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
