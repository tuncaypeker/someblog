namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Data.Keyword;
    using SomeBlog.Infrastructure.Interfaces;

    public class BlogData : EntityBaseData<Model.Blog>
    {
        AdCodeData adCodeData;
        AdViewLogData adViewLogData;
        BlogFormData blogFormData;
        BlogKeywordData blogKeywordData;
        BlogMailData blogMailData;
        BlogRedirectData blogRedirectData;
        BlogRouteData blogRouteData;
        BlogUptimeData blogUptimeData;
        BotData botData;
        ScraperData scraperData;
        CategoryData categoryData;
        CommentData commentData;
        ContactMessageData contactMessageData;
        Data.Content.ContentData contentData;
        Data.Content.ContentStartTemplateData contentStartTemplateData;
        Data.Content.ContentStartTemplateCategoryData contentStartTemplateCategoryData;
        HttpErrorLogData httpErrorLogData;
        JobLogData jobLogData;
        KeywordTempMovementData keywordTempMovementData;
        MediaData mediaData;
        MediaSizeData mediaSizeData;
        MenuData menuData;
        MetaData metaData;
        PageData pageData;
        SearchEngineBotLogData searchEngineBotLogData;
        SearchHistoryData searchHistoryData;
        SitemapSubmitLogData sitemapSubmitLogData;
        TagData tagData;
        CustomBotPostHistoryData customBotPostHistoryData;
        CustomBotConfigData customBotConfigData;
        UserBlogData userBlogData;

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public BlogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public BlogData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, AdCodeData adCodeData,
            AdViewLogData adViewLogData, BlogFormData blogFormData, BlogKeywordData blogKeywordData, BlogMailData blogMailData,
            BlogRedirectData blogRedirectData, BlogRouteData blogRouteData, BlogUptimeData blogUptimeData, BotData botData,
            CategoryData categoryData, CommentData commentData, ContactMessageData contactMessageData, Data.Content.ContentData contentData, Data.Content.ContentStartTemplateData contentStartTemplateData,
            HttpErrorLogData httpErrorLogData, JobLogData jobLogData, KeywordTempMovementData keywordTempMovementData, MediaData mediaData, MediaSizeData mediaSizeData, MenuData menuData,
            MetaData metaData, PageData pageData, SearchEngineBotLogData searchEngineBotLogData, SearchHistoryData searchHistoryData, SitemapSubmitLogData sitemapSubmitLogData, TagData tagData,
            Data.Content.ContentStartTemplateCategoryData contentStartTemplateCategoryData, CustomBotConfigData customBotConfigData, CustomBotPostHistoryData customBotPostHistoryData, ScraperData scraperData, UserBlogData userBlogData) : base(logger, serviceScopeFactory)
        {
            this.adCodeData = adCodeData;
            this.adViewLogData = adViewLogData;
            this.blogFormData = blogFormData;
            this.blogKeywordData = blogKeywordData;
            this.blogMailData = blogMailData;
            this.blogRedirectData = blogRedirectData;
            this.blogRouteData = blogRouteData;
            this.blogUptimeData = blogUptimeData;
            this.botData = botData;
            this.categoryData = categoryData;
            this.commentData = commentData;
            this.contactMessageData = contactMessageData;
            this.contentData = contentData;
            this.contentStartTemplateData = contentStartTemplateData;
            this.httpErrorLogData = httpErrorLogData;
            this.jobLogData = jobLogData;
            this.keywordTempMovementData = keywordTempMovementData;
            this.mediaData = mediaData;
            this.mediaSizeData = mediaSizeData;
            this.menuData = menuData;
            this.metaData = metaData;
            this.pageData = pageData;
            this.searchEngineBotLogData = searchEngineBotLogData;
            this.searchHistoryData = searchHistoryData;
            this.sitemapSubmitLogData = sitemapSubmitLogData;
            this.tagData = tagData;
            this.contentStartTemplateCategoryData = contentStartTemplateCategoryData;
            this.customBotPostHistoryData = customBotPostHistoryData;
            this.customBotConfigData = customBotConfigData;
            this.scraperData = scraperData;
            this.userBlogData = userBlogData;
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultAdCode = adCodeData.DeleteBulk(x => x.BlogId == id);
            var dataResultAdViewLog = adViewLogData.DeleteBulk(x => x.BlogId == id);
            var dataResultBlogRedirect = blogRedirectData.DeleteBulk(x => x.BlogId == id);
            var dataResultBlogRoute = blogRouteData.DeleteBulk(x => x.BlogId == id);
            var dataResultBlogUptime = blogUptimeData.DeleteBulk(x => x.BlogId == id);
            var dataResultComment = commentData.DeleteBulk(x => x.BlogId == id);
            var dataResultContactMessage = contactMessageData.DeleteBulk(x => x.BlogId == id);
            var dataResultContentStartTemplate = contentStartTemplateData.DeleteBulk(x => x.BlogId == id);
            var dataResultContentStartTemplateCategory = contentStartTemplateCategoryData.DeleteBulk(x => x.BlogId == id);
            var dataResultHttpErrorLog = httpErrorLogData.DeleteBulk(x => x.BlogId == id);

            var dataResultKeywordTempMovement = keywordTempMovementData.DeleteBulk(x => x.BlogId == id);
            var dataResultMedia = mediaData.DeleteBulk(x => x.BlogId == id);
            var dataResultMediaSize = mediaSizeData.DeleteBulk(x => x.BlogId == id);

            var dataResultMeta = metaData.DeleteBulk(x => x.BlogId == id);
            var dataResultPage = pageData.DeleteBulk(x => x.BlogId == id);
            var dataResultSearchEngineBotLog = searchEngineBotLogData.DeleteBulk(x => x.BlogId == id);
            var dataResultSearchHistory = searchHistoryData.DeleteBulk(x => x.BlogId == id);
            var dataResultSitemapSubmitLog = sitemapSubmitLogData.DeleteBulk(x => x.BlogId == id);
            var dataResultTag = tagData.DeleteBulk(x => x.BlogId == id);
            var dataResultCustombotConfig = customBotConfigData.DeleteBulk(x => x.BlogId == id);
            var dataResultCustombotPostHistory = customBotPostHistoryData.DeleteBulk(x => x.BlogId == id);
            

            var dataResultContent = contentData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultCategory = categoryData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultBot = botData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultScraper = scraperData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultBlogForm = blogFormData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultBlogKeyword = blogKeywordData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultBlogMail = blogMailData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultJobLog = jobLogData.DeleteWithAllRelationsByBlog(blogId: id);
            var dataResultMenu = menuData.DeleteWithAllRelationsByBlog(blogId: id);

            var dataResultUserBlog = userBlogData.DeleteBulk(x => x.BlogId == id);

            return DeleteByKey(id);
        }
    }
}
