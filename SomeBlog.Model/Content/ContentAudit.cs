using System;

namespace SomeBlog.Model
{
    public class ContentAudit : Core.ModelBase
    {
        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }

        public int AuditCount { get; set; }
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public string Results { get; set; }
    }
}
