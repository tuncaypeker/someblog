namespace SomeBlog.Integration.Google.LightHouse
{
    using lighthouse.net;
    using lighthouse.net.Objects;
    using System.Linq;

    /// <summary>
    /// https://github.com/dima-horror/lighthouse.net
    /// Download Google Chrome for Desktop.
    /// Install the current Long-Term Support version of Node.
    /// Install Lighthouse.The -g flag installs it as a global module.npm install -g lighthouse
    /// Install lighthouse.net into your project via NuGet
    /// </summary>
    public class Service
    {
        public Dto.AuditResultDto RunAudit(string path)
        {
            //var result = lh.Run(path).Result;

            var lh = new Lighthouse();
            var ar = new AuditRequest(path)
            {
                OnlyCategories = new[]
                {
                    Category.Performance,
                    Category.Accessibility,
                    Category.BestPractices,
                    Category.SEO,
                },
                EnableLogging = true,
            };
            var res = lh.Run(ar).Result;
            if (res.Performance == null)
                return null;

            return new Dto.AuditResultDto()
            {
                Accessibility = res.Accessibility,
                BenchmarkIndex = res.BenchmarkIndex,
                BestPractices = res.BestPractices,
                Details = res.Details.Select(x => new Dto.AuditResultDetailsDto
                {
                    Name = x.Name,
                    NumericValue = x.NumericValue,
                    Score = x.Score
                }).ToList(),
                FinalScreenshotBase64 = res.FinalScreenshot.Base64Data,
                Performance = res.Performance,
                Seo = res.Seo,
                TotalDuration = res.TotalDuration,
                Thumbnails = res.Thumbnails.Select(x => x.Base64Data).ToList()
            };
        }
    }
}
