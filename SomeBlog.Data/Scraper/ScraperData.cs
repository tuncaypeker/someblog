namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;

    public class ScraperData : EntityBaseData<Model.Scraper>
    {
        public ScraperRemoveRuleData scraperRemoveRuleData { get; set; }
        public ScraperReplaceRuleData scraperReplaceRuleData { get; set; }
        public ScraperPostHistoryData scraperPostHistoryData { get; set; }

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public ScraperData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public ScraperData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, ScraperRemoveRuleData scraperRemoveRuleData
            , ScraperReplaceRuleData scraperReplaceRuleData, ScraperPostHistoryData scraperPostHistoryData) : base(logger, serviceScopeFactory)
        {
            this.scraperRemoveRuleData = scraperRemoveRuleData;
            this.scraperReplaceRuleData = scraperReplaceRuleData;
            this.scraperPostHistoryData = scraperPostHistoryData;
        }

        public List<Model.Scraper> GetBlogScrapers(int blogId)
        {
            return GetBy(x => x.BlogId == blogId && x.IsActive);
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var bots = GetBy(x => x.BlogId == blogId);
            foreach (var bot in bots)
                DeleteWithAllRelations(bot.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultPostRemoveRule = scraperRemoveRuleData.DeleteBulk(x => x.ScraperId == id);
            var dataResultPostReplaceRule = scraperReplaceRuleData.DeleteBulk(x => x.ScraperId == id);
            var dataResultPostHistory = scraperPostHistoryData.DeleteBulk(x => x.ScraperId == id);

            return DeleteByKey(id);
        }
    }
}
