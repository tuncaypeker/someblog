using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using System;
using System.Linq;

namespace SomeBlog.Bots.ContentAuditUpdater
{
    class Program
    {
        static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            Console.Title = "2021.09.17";

            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var contentData = serviceProvider.GetService<ContentData>();
            var blogData = serviceProvider.GetService<BlogData>();
            var contentLighthouseAuditData = serviceProvider.GetService<ContentLighthouseAuditData>();
            var contentAuditData = serviceProvider.GetService<ContentAuditData>();
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

            var blogs = blogData.GetBy(x => !x.IsDemoBlog);

            int currentId = -1;

            while (true)
            {
                botProgramData.UpdatePulse("SomeBlog.Bots.ContentAuditUpdater");

                var dateNow = DateTime.Now;
                var dateMonthAgo = DateTime.Now.AddDays(-7);
                var contentToAudit = contentData.FirstOrDefault(x => x.LastAuditDate <= dateMonthAgo && x.IsActive && !x.IsDelete && x.PublishDate < dateNow, "PublishDate", isDesc: true, asNoTracking: true);
                if (contentToAudit == null)
                {
                    Console.WriteLine("Content alamadım...");
                    System.Threading.Thread.Sleep(10 * 1000);
                    continue;
                }

                //TODO: bunu tekrar almisiz demek ki bi ibnelik olmus
                //tekrar tekrar takilmamak için bunu sanki audit edilmis gibi kaydetcem
                //aslinda olmasi gereken ignore ettgimizi belli etmek olur
                if (contentToAudit.Id == currentId) {
                    contentToAudit.LastAuditDate = DateTime.Now;

                    var contentDbUpdateX = contentData.Update(contentToAudit);
                    Console.WriteLine($"{contentToAudit.PageTitle} Ignore Edildi");
                }

                currentId = contentToAudit.Id;

                var blog = blogs.FirstOrDefault(x => x.Id == contentToAudit.BlogId);
                if (blog == null)
                {
                    contentToAudit.LastAuditDate = DateTime.Now;
                    var contentUpdateResult = contentData.Update(contentToAudit);

                    continue;
                }

                if (contentToAudit == null)
                {
                    Console.WriteLine("Veri bulamadım 1 dk bekliycem");
                    System.Threading.Thread.Sleep(1 * 1000 * 60);
                    continue;
                }

                Console.WriteLine(contentToAudit.PageTitle + " ile çalışıyorum...");

                var year = contentToAudit.PublishDate.ToString("yyyy");
                var month = contentToAudit.PublishDate.ToString("MM");
                var path = blog.Url + "/" + blog.ContentLinkTemplate
                    .Replace("{slug}", contentToAudit.Slug)
                    .Replace("{year}", year)
                    .Replace("{month}", month)
                    .TrimStart('/');
                
                var lightHousex = new SomeBlog.Integration.Google.LightHouse.Service();
                var result = lightHousex.RunAudit(path);

                if (result == null)
                {
                    Console.WriteLine("=> Audit Failed: 30 sn bekliycem");
                    System.Threading.Thread.Sleep(1 * 1000 * 30);
                    continue;
                }
                else
                {
                    Console.WriteLine($"=> Audit Succeed: [{ result.TotalDuration }]/[Acc.{result.Accessibility}][Bpr.{result.BestPractices}][Perf.{result.Performance}][Seo.{result.Seo}]");
                }

                contentToAudit.PerformanceScore = result.Performance.HasValue ? result.Performance.Value : 0;
                contentToAudit.AccessibilityScore = result.Accessibility.HasValue ? result.Accessibility.Value : 0;
                contentToAudit.BestPracticesScore = result.BestPractices.HasValue ? result.BestPractices.Value : 0;
                contentToAudit.SeoScore = result.Seo.HasValue ? result.Seo.Value : 0;
                contentToAudit.LastAuditDate = DateTime.Now;

                var contentDbUpdate = contentData.Update(contentToAudit);
                Console.WriteLine(contentDbUpdate.IsSucceed ? $"=> [{contentToAudit.Id}] Updated." : $"=> {contentToAudit.Id}: Update Failed.");

                var blogLightHouse = new Model.ContentLighthouseAudit()
                {
                    Accessibility = result.Accessibility.HasValue ? result.Accessibility.Value : 0,
                    BenchmarkIndex = result.BenchmarkIndex.HasValue ? result.BenchmarkIndex.Value : 0,
                    BestPractices = result.BestPractices.HasValue ? result.BestPractices.Value : 0,
                    ContentId = contentToAudit.Id,
                    CreateDate = DateTime.Now,
                    Performance = result.Performance.HasValue ? result.Performance.Value : 0,
                    Screenshot = result.FinalScreenshotBase64,
                    Seo = result.Seo.HasValue ? result.Seo.Value : 0,
                    TotalDuration = result.TotalDuration.HasValue ? result.TotalDuration.Value : 0,
                    Details = JsonConvert.SerializeObject(result.Details)
                };

                var insertDbResult = contentLighthouseAuditData.Insert(blogLightHouse);
                Console.WriteLine(contentDbUpdate.IsSucceed ? "=> Audit Record Inserted" : "=> Audit Record Failed");

                //customAudit yapalim
                var auditResult = SomeBlog.Audit.OnPageSeo.RunAudits(contentToAudit);
                var contentAudit = new Model.ContentAudit()
                {
                    AuditCount = auditResult.Count,
                    ContentId = contentToAudit.Id,
                    CreateDate = DateTime.Now,
                    FailedCount = auditResult.Where(x => !x.HasPassed).Count(),
                    PassedCount = auditResult.Where(x => x.HasPassed).Count(),
                    Results = JsonConvert.SerializeObject(auditResult)
                };
                var insertDbResult2 = contentAuditData.Insert(contentAudit);

                contentLighthouseAuditData.DetachAllEntities();
                contentData.DetachAllEntities();
            }
        }
    }
}
