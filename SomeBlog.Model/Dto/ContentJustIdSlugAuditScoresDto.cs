namespace SomeBlog.Model.Dto
{
    public class ContentJustIdSlugAuditScoresDto
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public decimal PerformanceScore { get; set; }
        public decimal SeoScore { get; set; }
        public decimal BestPracticesScore { get; set; }
        public decimal AccessibilityScore { get; set; }
        public int BlogId { get; set; }
    }
}
