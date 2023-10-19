using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Data.Keyword;
using SomeBlog.Infrastructure.Interfaces;
using SomeBlog.Infrastructure.Logging.DummyLog;
using System;

namespace SomeBlog.Bots.Core
{
    public static class ServiceProviderHelper
    {
        public static ServiceProvider BuildServiceProvider()
        {
            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                //.AddJsonFile($"appsettings.{envName}.json", optional: true)
                .Build();

            return new ServiceCollection()
              .AddOptions()
              .AddSingleton<BlogData>()
              .AddSingleton<MediaData>()
              .AddSingleton<ContentData>()
              .AddSingleton<BotProgramData>()

              .AddSingleton<AdAccountData>()
              .AddSingleton<AdBannedIpData>()
              .AddSingleton<AdCodeData>()
              .AddSingleton<AdViewLogData>()
              .AddSingleton<ContentKeywordTailData>()
              .AddSingleton<DomainData>()
              .AddSingleton<DomainWhoisData>()
              .AddSingleton<RegistrarData>()
              .AddSingleton<RegistrarAccountData>()

              .AddTransient<MindmapData>()
              .AddTransient<BlogData>()
              .AddTransient<BlogRouteData>()
              .AddTransient<JobLogData>()
              .AddTransient<JobLogItemData>()
              .AddTransient<BlogRedirectData>()
              .AddTransient<BlogMailHistoryData>()

              .AddTransient<CategoryData>()
              .AddTransient<ContactMessageData>()
              .AddTransient<VersionData>()
              .AddTransient<VersionFileData>()
              .AddTransient<WikiData>()
              .AddTransient<BlogUptimeData>()
              .AddTransient<SitemapSubmitLogData>()
              .AddTransient<BlogMailData>()
              .AddTransient<PageData>()

              .AddTransient<AlexaSiteData>()
              .AddTransient<AlexaSiteKeywordData>()
              .AddTransient<AlexaSiteRelatedData>()

              .AddTransient<MediaData>()
              .AddTransient<MediaSizeData>()
              .AddTransient<MenuData>()
              .AddTransient<MenuItemData>()
              .AddTransient<ContentCategoryData>()
              .AddTransient<ContentTagData>()
              .AddTransient<ContentUpdateHistoryData>()
              .AddTransient<CommentData>()
              .AddTransient<TagData>()
              .AddTransient<MetaData>()
              .AddTransient<ContentMetaData>()
              .AddTransient<BotProgramData>()

              .AddTransient<SearchEngineBotLogData>()
              .AddTransient<SearchEngineSubmitLogData>()

              .AddTransient<BotData>()
              .AddTransient<BotHistoryData>()
              .AddTransient<BotHistoryLogData>()
              .AddTransient<BotRemoteCategoryData>()
              .AddTransient<BotRemoteTagData>()
              .AddTransient<BotRemoveRuleData>()
              .AddTransient<BotReplaceRuleData>()
              .AddTransient<ContentStartTemplateData>()
              .AddTransient<BotPostHistoryData>()
              .AddTransient<BotCategoryMapData>()

              .AddTransient<ScraperData>()
              .AddTransient<ScraperRemoveRuleData>()
              .AddTransient<ScraperReplaceRuleData>()
              .AddTransient<ScraperTemplateData>()

              .AddTransient<WordpressSiteData>()
              .AddTransient<WordpressSiteBotData>()
              .AddTransient<WordpressSiteBotHistoryData>()
              .AddTransient<WordpressSitePostHistoryData>()
              .AddTransient<WordpressContentStartTemplateData>()
              .AddTransient<WordpressCategoryData>()
              .AddTransient<WordpressSiteBotCategoryMapData>()

              .AddTransient<RoleData>()
              .AddTransient<RolePageData>()
              .AddTransient<UserData>()
              .AddTransient<ContentLighthouseAuditData>()
              .AddTransient<ContentAuditData>()

              .AddTransient<SearchHistoryData>()
              .AddTransient<BlogKeywordData>()
              .AddTransient<BlogKeywordTrackData>()
              .AddTransient<ContentHitHistoryData>()
              .AddTransient<HttpErrorLogData>()
              .AddTransient<ThemeData>()
              .AddTransient<ThemeVersionData>()
              .AddTransient<KeywordMozData>()
              .AddTransient<KeywordAhrefsData>()
              .AddTransient<KeywordSerpData>()
              .AddTransient<KeywordSemrushData>()
              .AddTransient<KeywordRelatedData>()
              .AddTransient<KeywordData>()

              .AddTransient<InstagramAccountData>()
              .AddTransient<InstagramPostData>()
              .AddTransient<InstagramPoolAccountData>()
              .AddTransient<InstagramPoolPostData>()
              .AddTransient<InstagramPoolCommentData>()
              .AddTransient<InstagramPoolPostImageData>()

              .AddTransient<PinterestAccountData>()
              .AddTransient<PinterestPostData>()
              .AddTransient<PinterestBoardData>()
              .AddTransient<PinterestPoolAccountData>()
              .AddTransient<PinterestPoolBoardData>()
              .AddTransient<ContentMediaData>()

              .AddTransient<RequestIpBlackListData>()

              .AddTransient<BlogFormData>()
              .AddTransient<BlogFormItemData>()
              .AddTransient<BlogFormFillData>()
              .AddTransient<BlogFormFillValueData>()

              .AddTransient<Data.Pool.PoolBlogImportRequestData>()
              .AddTransient<Data.Pool.PoolBlogImportRequestLogData>()
              .AddTransient<Data.Pool.PoolBlogImportRequestRemoveRuleData>()
              .AddTransient<Data.Pool.PoolBlogImportRequestReplaceRuleData>()
              .AddTransient<Data.Pool.PoolCategoryData>()
              .AddTransient<Data.Pool.PoolContentData>()
              .AddTransient<Data.Pool.PoolContentCommentData>()
              .AddTransient<Data.Pool.PoolBlogData>()
              .AddTransient<Data.Pool.PoolContentCategoryData>()
              .AddTransient<Data.Pool.PoolContentTranslateData>()
              .AddTransient<Data.Pool.PoolContentUsageData>()
              .AddTransient<Data.Pool.PoolTagData>()
              .AddTransient<Data.Pool.PoolContentTagData>()
              .AddTransient<Data.Pool.PoolMediaData>()
              .AddTransient<Data.Pool.PoolLanguageData>()
              .AddTransient<Data.Pool.PoolBlogUpdateLogData>()
              .AddTransient<Data.Pool.PoolBlogUpdateLogItemData>()

              .AddTransient<AhrefsAccountData>()
              .AddTransient<BingAccountData>()
              .AddTransient<YandexAccountData>()
              .AddTransient<GoogleAccountData>()

              .AddScoped<DataContext>(x => new DataContext(configuration["DatabaseSettings:ConnectionString"]))
              .AddDbContext<DataContext>(ServiceLifetime.Scoped)

              .AddScoped<ContentDataContext>(x => new ContentDataContext(configuration["DatabaseSettings:ContentConnectionString"]))
              .AddDbContext<ContentDataContext>(ServiceLifetime.Scoped)

              .AddScoped<PoolDataContext>(x => new PoolDataContext(configuration["DatabaseSettings:PoolConnectionString"]))
              .AddDbContext<PoolDataContext>(ServiceLifetime.Scoped)

              .AddTransient(typeof(ILogger<>), typeof(Logger<>))
               .BuildServiceProvider();
        }
    }
}
