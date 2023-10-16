namespace SomeBlog.Integration.Google.LightHouse.Dto
{
    using System.Collections.Generic;

    public class AuditResultDto
    {
        public decimal? Performance { get; set; }
        public decimal? Accessibility { get; set; }
        public decimal? BestPractices { get; set; }
        public decimal? Seo { get; set; }
        public decimal? BenchmarkIndex { get; set; }
        public List<AuditResultDetailsDto> Details { get; set; }
        public decimal? TotalDuration { get; set; }
        public string FinalScreenshotBase64 { get; set; }
        public IReadOnlyList<string> Thumbnails { get; set; }
    }

    public class AuditResultDetailsDto
    {
        public string Name { get; set; }
        public decimal? Score { get; set; }
        public decimal? NumericValue { get; set; }
    }
}
