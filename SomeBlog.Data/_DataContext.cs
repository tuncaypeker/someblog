namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using SomeBlog.Data.Infrastructure;

    public class DataContext : DbContext
    {
        public DataContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySQL(connectionString).Options)
        {
        }

        public DbSet<Model.Blog> Blogs { get; set; }
        public DbSet<Model.BlogRoute> BlogRoutes { get; set; }
        public DbSet<Model.BlogRedirect> BlogRedirects { get; set; }
        public DbSet<Model.BlogMailHistory> BlogMailHistories { get; set; }
        public DbSet<Model.BlogMail> BlogMails { get; set; }
        public DbSet<Model.BlogKeywordTrack> BlogKeywordTracks { get; set; }
        public DbSet<Model.BlogKeyword> BlogKeywords { get; set; }
        public DbSet<Model.BlogUptime> BlogUptimes { get; set; }
        public DbSet<Model.BlogForm> BlogForms { get; set; }
        public DbSet<Model.BlogFormItem> BlogFormItems { get; set; }
        public DbSet<Model.BlogFormFill> BlogFormFills { get; set; }
        public DbSet<Model.BlogFormFillValue> BlogFormFillValues { get; set; }

		public DbSet<Model.BotProgram> BotPrograms { get; set; }
        public DbSet<Model.Bot> Bots { get; set; }
        public DbSet<Model.BotHistory> BotHistories { get; set; }
        public DbSet<Model.BotPostHistory> BotPostHistories { get; set; }
        public DbSet<Model.BotHistoryLog> BotHistoryLogs { get; set; }
        public DbSet<Model.BotRemoveRule> BotRemoveRules { get; set; }
        public DbSet<Model.BotReplaceRule> BotReplaceRules { get; set; }
        public DbSet<Model.BotRemoteTag> BotRemoteTags { get; set; }
        public DbSet<Model.BotRemoteCategory> BotRemoteCategories { get; set; }
        public DbSet<Model.BotCategoryMap> BotCategoryMaps { get; set; }

        public DbSet<Model.CustomBot> CustomBots { get; set; }
        public DbSet<Model.CustomBotConfig> CustomBotConfigs { get; set; }
        public DbSet<Model.CustomBotHistory> CustomBotHistories { get; set; }
        public DbSet<Model.CustomBotHistoryLog> CustomBotHistoryLogs { get; set; }
        public DbSet<Model.CustomBotPostHistory> CustomBotPostHistories { get; set; }

        public DbSet<Model.FeedlyBlog> FeedlyBlogs { get; set; }
        public DbSet<Model.FeedlyBlogTopic> FeedlyBlogTopics { get; set; }
        public DbSet<Model.FeedlyTopic> FeedlyTopics { get; set; }
        public DbSet<Model.FeedlyTopicRelated> FeedlyTopicRelateds { get; set; }

        public DbSet<Model.InstagramAccount> InstagramAccounts { get; set; }
        public DbSet<Model.InstagramPoolAccount> InstagramPoolAccounts { get; set; }
        public DbSet<Model.InstagramPoolPost> InstagramPoolPosts { get; set; }
        public DbSet<Model.InstagramPoolPostImage> InstagramPoolPostImages { get; set; }
        public DbSet<Model.InstagramPoolComment> InstagramPoolComments { get; set; }
        public DbSet<Model.InstagramPost> InstagramPosts { get; set; }

        public DbSet<Model.Keyword> Keywords { get; set; }
        public DbSet<Model.KeywordMoz> KeywordMozs { get; set; }
        public DbSet<Model.KeywordSemrush> KeywordSemrushes { get; set; }
        public DbSet<Model.KeywordAhrefs> KeywordAhrefs { get; set; }
        public DbSet<Model.KeywordSerp> KeywordSerps { get; set; }
        public DbSet<Model.KeywordEverywhere> KeywordEverywheres { get; set; }
        public DbSet<Model.KeywordEverywhereQuestion> KeywordEverywhereQuestions { get; set; }
        public DbSet<Model.KeywordRelated> KeywordRelateds { get; set; }
        public DbSet<Model.KeywordTempMovement> KeywordTempMovements { get; set; }

        public DbSet<Model.PinterestAccount> PinterestAccounts { get; set; }
        public DbSet<Model.PinterestPost> PinterestPosts { get; set; }
        public DbSet<Model.PinterestBoard> PinterestBoards { get; set; }
        public DbSet<Model.PinterestPoolAccount> PinterestPoolAccounts { get; set; }
        public DbSet<Model.PinterestPoolBoard> PinterestPoolBoards { get; set; }
        public DbSet<Model.PinterestPoolPost> PinterestPoolPosts { get; set; }

        public DbSet<Model.Scraper> Scrapers { get; set; }
        public DbSet<Model.ScraperTemplate> ScraperTemplates { get; set; }
        public DbSet<Model.ScraperRemoveRule> ScraperRemoveRules { get; set; }
        public DbSet<Model.ScraperReplaceRule> ScraperReplaceRules { get; set; }
        public DbSet<Model.ScraperPostHistory> ScraperPostHistories { get; set; }

        public DbSet<Model.Theme> Themes { get; set; }
        public DbSet<Model.ThemeVersion> ThemeVersions { get; set; }

        public DbSet<Model.WordpressSite> WordpressSites { get; set; }
        public DbSet<Model.WordpressSiteBot> WordpressSiteBots { get; set; }
        public DbSet<Model.WordpressSiteBotHistory> WordpressSiteBotHistories { get; set; }
        public DbSet<Model.WordpressSitePostHistory> WordpressSitePostHistories { get; set; }
        public DbSet<Model.WordpressContentStartTemplate> WordpressContentStartTemplates { get; set; }
        public DbSet<Model.WordpressCategory> WordpressCategories { get; set; }
        public DbSet<Model.WordpressSiteBotCategoryMap> WordpressSiteBotCategoryMaps { get; set; }

        public DbSet<Model.Wiki> Wikis { get; set; }
        public DbSet<Model.Meta> Metas { get; set; }
        public DbSet<Model.AdAccount> AdAccounts { get; set; }
        public DbSet<Model.Version> Versions { get; set; }
        public DbSet<Model.VersionFile> VersionFiles { get; set; }
        public DbSet<Model.Mindmap> Mindmaps { get; set; }
        public DbSet<Model.SitemapSubmitLog> SitemapSubmitLogs { get; set; }
        public DbSet<Model.Domain> Domains { get; set; }
        public DbSet<Model.DomainWhois> DomainWhoises { get; set; }
        public DbSet<Model.Registrar> Registrars { get; set; }
        public DbSet<Model.RegistrarAccount> RegistrarAccounts { get; set; }
        public DbSet<Model.Category> Categories { get; set; }
        public DbSet<Model.Media> Medias { get; set; }
        public DbSet<Model.MediaSize> MediaSizes { get; set; }
        public DbSet<Model.Menu> Menus { get; set; }
        public DbSet<Model.MenuItem> MenuItems { get; set; }
        public DbSet<Model.AdCode> AdCodes { get; set; }
        public DbSet<Model.AdBannedIp> AdBannedIps { get; set; }
        public DbSet<Model.AdViewLog> AdViewLog { get; set; }
        public DbSet<Model.Comment> Comments { get; set; }
        public DbSet<Model.ContactMessage> ContactMessages { get; set; }
        public DbSet<Model.Tag> Tags { get; set; }
        public DbSet<Model.Page> Pages { get; set; }
        public DbSet<Model.AlexaSite> AlexaSites { get; set; }
        public DbSet<Model.AlexaSiteKeyword> AlexaSiteKeywords { get; set; }
        public DbSet<Model.AlexaSiteRelated> AlexaSiteRelateds { get; set; }
        public DbSet<Model.User> Users { get; set; }
        public DbSet<Model.UserBlog> UserBlogs { get; set; }
        public DbSet<Model.Role> Roles { get; set; }
        public DbSet<Model.RolePage> RolePages { get; set; }
        public DbSet<Model.SearchEngineBotLog> SearchEngineBotLogs { get; set; }
        public DbSet<Model.SearchEngineSubmitLog> SearchEngineSubmitLogs { get; set; }
        public DbSet<Model.SearchHistory> SearchHistories { get; set; }
        public DbSet<Model.HttpErrorLog> HttpErrorLogs { get; set; }
        
        public DbSet<Model.JobLog> JobLogs { get; set; }
        public DbSet<Model.JobLogItem> JobLogItems { get; set; }

        public DbSet<Model.BacklinkComment> BacklinkComments { get; set; }
        public DbSet<Model.StopRequestWord> StopRequestWords { get; set; }

        //Webmaster
        public DbSet<Model.AhrefsAccount> AhrefsAccounts { get; set; }
        public DbSet<Model.BingAccount> BingAccounts { get; set; }
        public DbSet<Model.GoogleAccount> GoogleAccounts { get; set; }
        public DbSet<Model.PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Model.YandexAccount> YandexAccounts { get; set; }

        public DbSet<Model.ChatGptAccount> ChatGptAccounts { get; set; }
        public DbSet<Model.ChatGptPrompt> ChatGptPrompts { get; set; }
        public DbSet<Model.ChatGptLog> ChatGptLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model.FeedlyBlog>(entity => entity.ToTable("feedly_blogs"));
            builder.Entity<Model.FeedlyBlogTopic>(entity => entity.ToTable("feedly_blog_topics"));
            builder.Entity<Model.FeedlyTopic>(entity => entity.ToTable("feedly_topics"));
            builder.Entity<Model.FeedlyTopicRelated>(entity => entity.ToTable("feedly_topic_relateds"));

            builder.Entity<Model.BotProgram>(entity => entity.ToTable("bot_programs"));

            builder.Entity<Model.Wiki>(entity => entity.ToTable("wikis"));
            
            builder.Entity<Model.Mindmap>(entity => entity.ToTable("mindmaps"));
            builder.Entity<Model.Meta>(entity => entity.ToTable("metas"));
            builder.Entity<Model.AdAccount>(entity => entity.ToTable("ad_accounts"));
            builder.Entity<Model.Blog>(entity => entity.ToTable("blogs"));
            builder.Entity<Model.BlogRoute>(entity => entity.ToTable("blog_routes"));
            builder.Entity<Model.BlogRedirect>(entity => entity.ToTable("blog_redirects"));
            builder.Entity<Model.BlogMailHistory>(entity => entity.ToTable("blog_mail_histories"));
	
            builder.Entity<Model.Version>(entity => entity.ToTable("versions"));
            builder.Entity<Model.VersionFile>(entity => entity.ToTable("version_files"));
            builder.Entity<Model.SitemapSubmitLog>(entity => entity.ToTable("sitemap_submit_logs"));

            builder.Entity<Model.Domain>(entity => entity.ToTable("domains"));
            builder.Entity<Model.DomainWhois>(entity => entity.ToTable("domain_whoises"));
            builder.Entity<Model.Registrar>(entity => entity.ToTable("registrars"));
            builder.Entity<Model.RegistrarAccount>(entity => entity.ToTable("registrar_accounts"));

            builder.Entity<Model.Category>(entity => entity.ToTable("categories"));
            

            builder.Entity<Model.Media>(entity => entity.ToTable("medias"));
            

            builder.Entity<Model.RequestIpBlackList>(entity => entity.ToTable("request_ip_black_lists"));

            builder.Entity<Model.MediaSize>(entity => entity.ToTable("media_sizes"));
            builder.Entity<Model.Menu>(entity => entity.ToTable("menus"));
            builder.Entity<Model.MenuItem>(entity => entity.ToTable("menu_items"));
            builder.Entity<Model.AdCode>(entity => entity.ToTable("ad_codes"));
            builder.Entity<Model.AdBannedIp>(entity => entity.ToTable("ad_banned_ips"));
            builder.Entity<Model.AdViewLog>(entity => entity.ToTable("ad_view_logs"));
            builder.Entity<Model.Comment>(entity => entity.ToTable("comments"));
            builder.Entity<Model.ContactMessage>(entity => entity.ToTable("contact_messages"));
            

            builder.Entity<Model.Tag>(entity => entity.ToTable("tags"));
            
            builder.Entity<Model.BlogMail>(entity => entity.ToTable("blog_mails"));
            builder.Entity<Model.Page>(entity => entity.ToTable("pages"));

            builder.Entity<Model.AlexaSite>(entity => entity.ToTable("alexa_sites"));
            builder.Entity<Model.AlexaSiteKeyword>(entity => entity.ToTable("alexa_site_keywords"));
            builder.Entity<Model.AlexaSiteRelated>(entity => entity.ToTable("alexa_site_relateds"));

            builder.Entity<Model.Bot>(entity => entity.ToTable("bots"));
            builder.Entity<Model.BotHistory>(entity => entity.ToTable("bot_histories"));
            builder.Entity<Model.BotHistoryLog>(entity => entity.ToTable("bot_history_logs"));
            builder.Entity<Model.BotRemoveRule>(entity => entity.ToTable("bot_remove_rules"));
            builder.Entity<Model.BotReplaceRule>(entity => entity.ToTable("bot_replace_rules"));
            builder.Entity<Model.BotRemoteTag>(entity => entity.ToTable("bot_remote_tags"));
            builder.Entity<Model.BotRemoteCategory>(entity => entity.ToTable("bot_remote_categories"));
            builder.Entity<Model.BotPostHistory>(entity => entity.ToTable("bot_post_histories"));
            builder.Entity<Model.BotCategoryMap>(entity => entity.ToTable("bot_category_maps"));

            builder.Entity<Model.CustomBotConfig>(entity => entity.ToTable("custom_bot_configs"));
            builder.Entity<Model.CustomBot>(entity => entity.ToTable("custom_bots"));
            builder.Entity<Model.CustomBotHistory>(entity => entity.ToTable("custom_bot_histories"));
            builder.Entity<Model.CustomBotHistoryLog>(entity => entity.ToTable("custom_bot_history_logs"));
            builder.Entity<Model.CustomBotPostHistory>(entity => entity.ToTable("custom_bot_post_histories"));

            builder.Entity<Model.Scraper>(entity => entity.ToTable("scrapers"));
            builder.Entity<Model.ScraperTemplate>(entity => entity.ToTable("scraper_templates"));
            builder.Entity<Model.ScraperRemoveRule>(entity => entity.ToTable("scraper_remove_rules"));
            builder.Entity<Model.ScraperReplaceRule>(entity => entity.ToTable("scraper_replace_rules"));
            builder.Entity<Model.ScraperPostHistory>(entity => entity.ToTable("scraper_post_histories"));

            builder.Entity<Model.WordpressSite>(entity => entity.ToTable("wordpress_sites"));
            builder.Entity<Model.WordpressSiteBot>(entity => entity.ToTable("wordpress_site_bots"));
            builder.Entity<Model.WordpressSiteBotHistory>(entity => entity.ToTable("wordpress_site_bot_histories"));
            builder.Entity<Model.WordpressSitePostHistory>(entity => entity.ToTable("wordpress_site_post_histories"));
            builder.Entity<Model.WordpressContentStartTemplate>(entity => entity.ToTable("wordpress_content_start_templates"));
            builder.Entity<Model.WordpressCategory>(entity => entity.ToTable("wordpress_categories"));
            builder.Entity<Model.WordpressSiteBotCategoryMap>(entity => entity.ToTable("wordpress_site_bot_category_maps"));

            builder.Entity<Model.User>(entity => entity.ToTable("users"));
            builder.Entity<Model.UserBlog>(entity => entity.ToTable("user_blogs"));
            builder.Entity<Model.Role>(entity => entity.ToTable("roles"));
            builder.Entity<Model.RolePage>(entity => entity.ToTable("role_pages"));
            builder.Entity<Model.SearchEngineBotLog>(entity => entity.ToTable("search_engine_bot_logs"));
            builder.Entity<Model.SearchEngineSubmitLog>(entity => entity.ToTable("search_engine_submit_logs"));

            builder.Entity<Model.SearchHistory>(entity => entity.ToTable("search_histories"));

            builder.Entity<Model.Keyword>(entity => entity.ToTable("keywords"));
            builder.Entity<Model.KeywordMoz>(entity => entity.ToTable("keyword_mozs"));
            builder.Entity<Model.KeywordSemrush>(entity => entity.ToTable("keyword_semrushes"));
            builder.Entity<Model.KeywordAhrefs>(entity => entity.ToTable("keyword_ahrefs"));
            builder.Entity<Model.KeywordEverywhere>(entity => entity.ToTable("keyword_everywheres"));
            builder.Entity<Model.KeywordEverywhereQuestion>(entity => entity.ToTable("keyword_everywhere_questions"));
            builder.Entity<Model.KeywordRelated>(entity => entity.ToTable("keyword_relateds"));
            builder.Entity<Model.KeywordSerp>(entity => entity.ToTable("keyword_serps"));
            builder.Entity<Model.KeywordTempMovement>(entity => entity.ToTable("keyword_temp_movements"));
            builder.Entity<Model.BlogKeyword>(entity => entity.ToTable("blog_keywords"));
            builder.Entity<Model.BlogKeywordTrack>(entity => entity.ToTable("blog_keyword_tracks"));
            builder.Entity<Model.BlogUptime>(entity => entity.ToTable("blog_uptimes"));
            
            
            builder.Entity<Model.HttpErrorLog>(entity => entity.ToTable("http_error_logs"));
            builder.Entity<Model.Theme>(entity => entity.ToTable("themes"));
            builder.Entity<Model.ThemeVersion>(entity => entity.ToTable("theme_versions"));
            builder.Entity<Model.JobLog>(entity => entity.ToTable("job_logs"));
            builder.Entity<Model.JobLogItem>(entity => entity.ToTable("job_log_items"));

            builder.Entity<Model.InstagramAccount>(entity => entity.ToTable("instagram_accounts"));
            builder.Entity<Model.InstagramPoolAccount>(entity => entity.ToTable("instagram_pool_accounts"));
            builder.Entity<Model.InstagramPoolPost>(entity => entity.ToTable("instagram_pool_posts"));
            builder.Entity<Model.InstagramPoolPostImage>(entity => entity.ToTable("instagram_pool_post_images"));
            builder.Entity<Model.InstagramPoolComment>(entity => entity.ToTable("instagram_pool_comments"));
            builder.Entity<Model.InstagramPost>(entity => entity.ToTable("instagram_posts"));

            builder.Entity<Model.PinterestAccount>(entity => entity.ToTable("pinterest_accounts"));
            builder.Entity<Model.PinterestPost>(entity => entity.ToTable("pinterest_posts"));
            builder.Entity<Model.PinterestBoard>(entity => entity.ToTable("pinterest_boards"));
            builder.Entity<Model.PinterestPoolAccount>(entity => entity.ToTable("pinterest_pool_accounts"));
            builder.Entity<Model.PinterestPoolBoard>(entity => entity.ToTable("pinterest_pool_boards"));
            builder.Entity<Model.PinterestPoolPost>(entity => entity.ToTable("pinterest_pool_posts"));

            builder.Entity<Model.BlogForm>(entity => entity.ToTable("blog_forms"));
            builder.Entity<Model.BlogFormItem>(entity => entity.ToTable("blog_form_items"));
            builder.Entity<Model.BlogFormFill>(entity => entity.ToTable("blog_form_fills"));
            builder.Entity<Model.BlogFormFillValue>(entity => entity.ToTable("blog_form_fill_values"));

            builder.Entity<Model.BacklinkComment>(entity => entity.ToTable("backlink_comments"));

            builder.Entity<Model.StopRequestWord>(entity => entity.ToTable("stop_request_words"));

            //Webmaster
            builder.Entity<Model.AhrefsAccount>(entity => entity.ToTable("ahrefs_accounts"));
            builder.Entity<Model.BingAccount>(entity => entity.ToTable("bing_accounts"));
            builder.Entity<Model.GoogleAccount>(entity => entity.ToTable("google_accounts"));
            builder.Entity<Model.PhoneNumber>(entity => entity.ToTable("phone_numbers"));
            builder.Entity<Model.YandexAccount>(entity => entity.ToTable("yandex_accounts"));

            builder.Entity<Model.ChatGptAccount>(entity => entity.ToTable("chatgpt_accounts"));
            builder.Entity<Model.ChatGptPrompt>(entity => entity.ToTable("chatgpt_prompts"));
            builder.Entity<Model.ChatGptLog>(entity => entity.ToTable("chatgpt_logs"));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }
    }
}