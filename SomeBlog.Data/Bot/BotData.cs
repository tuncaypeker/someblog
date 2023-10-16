namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;

    public class BotData : EntityBaseData<Model.Bot>
    {
        public BotCategoryMapData botCategoryMapData { get; set; }
        public BotHistoryData botHistoryData { get; set; }
        public BotHistoryLogData botHistoryLogData { get; set; }
        public BotPostHistoryData botPostHistoryData { get; set; }
        public BotRemoteCategoryData botRemoteCategoryData { get; set; }
        public BotRemoteTagData botRemoteTagData { get; set; }
        public BotRemoveRuleData botRemoveRuleData { get; set; }
        public BotReplaceRuleData botReplaceRuleData { get; set; }

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public BotData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public BotData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, BotCategoryMapData botCategoryMapData, BotHistoryData botHistoryData,
            BotHistoryLogData botHistoryLogData, BotPostHistoryData botPostHistoryData, BotRemoteCategoryData botRemoteCategoryData, BotRemoteTagData botRemoteTagData,
            BotRemoveRuleData botRemoveRuleData, BotReplaceRuleData botReplaceRuleData) : base(logger, serviceScopeFactory)
        {
            this.botCategoryMapData = botCategoryMapData;
            this.botHistoryData = botHistoryData;
            this.botHistoryLogData = botHistoryLogData;
            this.botPostHistoryData = botPostHistoryData;
            this.botRemoteCategoryData = botRemoteCategoryData;
            this.botRemoteTagData = botRemoteTagData;
            this.botRemoveRuleData = botRemoveRuleData;
            this.botReplaceRuleData = botReplaceRuleData;
        }

        public List<Model.Bot> GetBlogBots(int blogId)
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
            var dataResultCategoryMap = botCategoryMapData.DeleteBulk(x => x.BotId == id);
            var dataResultHistory = botHistoryData.DeleteBulk(x => x.BotId == id);
            var dataResultHistoryLog = botHistoryLogData.DeleteBulk(x => x.BotId == id);
            var dataResultPostHistory = botPostHistoryData.DeleteBulk(x => x.BotId == id);
            var dataResultPostRemoteCategory = botRemoteCategoryData.DeleteBulk(x => x.BotId == id);
            var dataResultPostRemoteTag = botRemoteTagData.DeleteBulk(x => x.BotId == id);
            var dataResultPostRemoveRule = botRemoveRuleData.DeleteBulk(x => x.BotId == id);
            var dataResultPostReplaceRule = botReplaceRuleData.DeleteBulk(x => x.BotId == id);

            return DeleteByKey(id);
        }
    }
}
