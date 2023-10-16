namespace SomeBlog.Integration.GoogleAnalytics.Dto
{
    public class VisitorSummaryDto
    {
        /// <summary>
        /// YYYY-DD-MM olarak gelir
        /// </summary>
        public string Date { get; set; }

        public string TotalUsers { get; set; }
        public string NewUsers { get; set; }
    }
}
