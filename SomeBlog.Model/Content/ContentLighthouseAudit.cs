using System;

namespace SomeBlog.Model
{
    public class ContentLighthouseAudit : Core.ModelBase
    {
        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }

        public decimal Accessibility { get; set; }
        public decimal BenchmarkIndex { get; set; }
        public decimal BestPractices { get; set; }
        public decimal Performance { get; set; }
        public decimal Seo { get; set; }
        public decimal TotalDuration { get; set; }
        public string Screenshot { get; set; }
        public string Details { get; set; }
    }
}
