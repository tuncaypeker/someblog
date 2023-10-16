using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Data.Infrastructure.Entities;
using SomeBlog.Data.Keyword;
using SomeBlog.Infrastructure.Extensions;
using SomeBlog.Infrastructure.Interfaces;
using SomeBlog.Infrastructure.Logging.DummyLog;
using SomeBlog.Wordpress.XmlRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SomeBlog.Importer.Wordpress
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static int _blogId = 33;
        static string _basePath = "https://finansweb.net/";
        static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                //.AddJsonFile($"appsettings.{envName}.json", optional: true)
                .Build();

            serviceProvider = _buildProvider();

            int ok = 0, no = 0, counter = 1;

            var _apiHelper = new SomeBlog.Wordpress.WpJson.Helper(_basePath);
            var _contentData = serviceProvider.GetService<ContentData>();
            var _commentData = serviceProvider.GetService<CommentData>();
            var _categoryData = serviceProvider.GetService<CategoryData>();
            var _contentCategoryData = serviceProvider.GetService<ContentCategoryData>();
            var _contentTagData = serviceProvider.GetService<ContentTagData>();
            var _mediaData = serviceProvider.GetService<MediaData>();
            var _tagData = serviceProvider.GetService<TagData>();
            var _blogData = serviceProvider.GetService<BlogData>();

            var remote_categories = _apiHelper.GetCategories();
            var remote_tags = _apiHelper.GetTags();
            var remote_posts = _apiHelper.GetPosts(counter);

            var local_tags = _tagData.GetBy(x => x.BlogId == _blogId);
            var local_categories = _categoryData.GetBy(x => x.BlogId == _blogId);

            var blog = _blogData.GetByKey(_blogId);

            while (remote_posts != null)
            {
                foreach (var p in remote_posts)
                {
                    var post_in_db = _contentData.GetBy(x => x.Slug == p.slug && x.BlogId == _blogId).FirstOrDefault();
                    if (post_in_db != null)
                    {
                        Console.WriteLine($"x {p.slug} Veritabanında var, atlıyorum");
                        continue;
                    }


                    //TAG'LER
                    var content_tags = new List<Model.Tag>();
                    var new_tags_to_insert = new List<Model.Tag>();
                    var keywords = "";
                    foreach (var t in p.tags)
                    {
                        var remote_tag = remote_tags.FirstOrDefault(x => x.id == t);
                        if (remote_tag == null)
                            return;

                        keywords += remote_tag.name + ",";

                        var local_tag = local_tags.FirstOrDefault(x => x.Slug == remote_tag.slug);
                        if (local_tag != null)
                        {
                            content_tags.Add(local_tag);
                        }
                        else
                        {
                            new_tags_to_insert.Add(new Model.Tag()
                            {
                                BlogId = _blogId,
                                MetaDescription = remote_tag.description.ToStripHtml() ?? "",
                                MetaTitle = remote_tag.name ?? "",
                                Name = remote_tag.name ?? "",
                                PageDescription = remote_tag.description.ToStripHtml() ?? "",
                                PageTitle = remote_tag.name ?? "",
                                Slug = remote_tag.slug ?? "",
                            });
                        }
                    }

                    if (new_tags_to_insert.Count > 0)
                    {
                        var insertBulkTag = _tagData.InsertBulk(new_tags_to_insert);
                        if (insertBulkTag.IsSucceed)
                        {
                            content_tags.AddRange(new_tags_to_insert);
                            local_tags.AddRange(new_tags_to_insert); //initial listeye ekleyelim ki tekrar tekrar insert olmasın
                        }
                    }

                    //KATEGORILER
                    var content_categories = new List<Model.Category>();
                    var new_categories_to_insert = new List<Model.Category>();
                    foreach (var c in p.categories)
                    {
                        var remote_category = remote_categories.FirstOrDefault(x => x.id == c);
                        if (remote_category == null)
                            return;

                        var local_category = local_categories.FirstOrDefault(x => x.Slug == remote_category.slug);
                        if (local_category != null)
                        {
                            content_categories.Add(local_category);
                        }
                        else
                        {
                            new_categories_to_insert.Add(new Model.Category()
                            {
                                BlogId = _blogId,
                                MetaDescription = remote_category.description.ToStripHtml() ?? "",
                                MetaTitle = remote_category.name ?? "",
                                Name = remote_category.name ?? "",
                                PageDescription = remote_category.description.ToStripHtml() ?? "",
                                PageTitle = remote_category.name ?? "",
                                Slug = remote_category.slug ?? "",
                                Hit = 0,
                                ImagePath = "",
                                ParentId = -1,
                            });
                        }
                    }
                    if (new_categories_to_insert.Count > 0)
                    {
                        var insertBulkCategory = _categoryData.InsertBulk(new_categories_to_insert);
                        if (insertBulkCategory.IsSucceed)
                        {
                            content_categories.AddRange(new_categories_to_insert);
                            local_categories.AddRange(new_categories_to_insert);
                        }
                    }

                    var seo_description = "";
                    var seo_title = "";

                    if (!string.IsNullOrEmpty(p.yoast_head))
                    {
                        var split = p.yoast_head.Split("\n", StringSplitOptions.None);
                        foreach (var item in split)
                        {
                            if (item.Contains("og:description"))
                            {
                                var description_split = item.Split("content=", StringSplitOptions.None);
                                seo_description = description_split[1].Replace("\"", "").Replace("/>", "");
                            }

                            if (item.Contains("og:title"))
                            {
                                var title_split = item.Split("content=", StringSplitOptions.None);
                                seo_title = title_split[1].Replace("\"", "").Replace("/>", "");
                            }
                        }
                    }

                    var post = new Model.Content()
                    {
                        BlogId = _blogId,
                        CreateDate = DateTime.Now,
                        FocusKeyword = "",
                        HasFeatured = false,
                        Hit = 0,
                        IsActive = true,
                        IsDelete = false,
                        MetaDescription = seo_description ?? p.title.rendered,
                        MetaTitle = seo_title ?? p.title.rendered,
                        PageDescription = p.title.rendered,
                        PageTitle = p.title.rendered,
                        Text = p.content.rendered,
                        Excerpt = p.excerpt.rendered.ToStripHtml(),
                        PublishDate = p.date,
                        UpdateDate = p.modified,
                        Slug = p.slug,
                        Tags = keywords.TrimEnd(','),
                        AccessibilityScore = 0,
                        BestPracticesScore = 0,
                        CreatedById = -1,
                        HasUpdatedAfterBotInsert = true,
                    };

                    //post.Text = post.Text.ToRemoveAnchors();
                    post.Text = DownloadPostInImages(post.Text, _basePath, blog.Url, _blogId);

                    var insertContent = _contentData.Insert(post);
                    if (!insertContent.IsSucceed)
                    {
                        Console.WriteLine($"- {post.PageTitle} eklenirken hata {insertContent.Message}");
                        no++;
                        continue;
                    }

                    Console.WriteLine("+ " + post.PageTitle);

                    var contentTagsInsertResult = _contentTagData.InsertBulk(content_tags.Select(x => new Model.ContentTag(contentId: post.Id, tagId: x.Id)).ToList());
                    var contentcategoriesInsertResult = _contentCategoryData.InsertBulk(content_categories.Select(x => new Model.ContentCategory(contentId: post.Id, categoryId: x.Id)).ToList());

                    //feature media indir
                    if (!string.IsNullOrEmpty(p.featured_media))
                    {
                        try
                        {
                            var mediaJson = _apiHelper.GetMedia(p.featured_media);

                            if (mediaJson != null)
                            {
                                var remote_media_path = mediaJson.media_url;
                                var local_media_path = remote_media_path.Replace(_basePath, "").TrimStart('/');

                                Directory.CreateDirectory(Path.GetDirectoryName(local_media_path));

                                using (var client = new WebClient())
                                {
                                    client.DownloadFile(remote_media_path, local_media_path);
                                }

                                var media = new Model.Media()
                                {
                                    Alt = mediaJson.alt_text ?? post.MetaTitle,
                                    ImageSlug = Path.GetFileNameWithoutExtension(local_media_path),
                                    MediaUrl = local_media_path,
                                    Title = post.PageTitle,
                                    Type = 1,
                                    CreateDate = DateTime.Now,
                                    BlogId = _blogId
                                };

                                var mediaInsertResult = _mediaData.Insert(media);

                                if (mediaInsertResult.IsSucceed)
                                {
                                    post.FeaturedMediaId = media.Id;

                                    _contentData.Update(post);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                counter += 1;
                remote_posts = _apiHelper.GetPosts(counter);
            }

            var wp_comments = _apiHelper.GetComments();
            var local_comments = _commentData.GetBy(x => x.BlogId == _blogId);
            var local_contents = _contentData.GetBy(x => x.BlogId == _blogId);
            var new_comments_to_insert = new List<Model.Comment>();

            foreach (var item in wp_comments)
            {
                //https://lolpingtesti.com/lol-skin-indir-2020-guncel/#comment-510
                var content_slug = item.link.Replace($"/#comment-{item.id}", "").Replace($"{_basePath}/", "");
                var is_have_content = local_contents.FirstOrDefault(x => x.Slug == content_slug);

                var local_comment = local_comments.FirstOrDefault(x => x.Name == item.author_name && x.Text == item.content.rendered);
                if (local_comment != null)
                    continue;

                new_comments_to_insert.Add(new Model.Comment()
                {
                    BlogId = _blogId,
                    ContentId = is_have_content != null ? is_have_content.Id : -1,
                    Created = item.date,
                    Email = "",
                    IsApproved = true,
                    Name = item.author_name,
                    Text = item.content.rendered,
                });
            }

            var insertBulkComments = _commentData.InsertBulk(new_comments_to_insert);

            Console.WriteLine($"Bitti=> {ok} ok / {no} no");
            Console.ReadLine();
        }

        private static ServiceProvider _buildProvider()
        {
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
              .AddTransient<ContentData>()
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
              .AddTransient<KeywordData>()
              .AddTransient<KeywordTempMovementData>()
              .AddTransient<KeywordAhrefsData>()
              .AddTransient<KeywordEverywhereData>()
              .AddTransient<KeywordMozData>()
              .AddTransient<KeywordSemrushData>()
              .AddTransient<BlogKeywordData>()
              .AddTransient<BlogKeywordTrackData>()
              .AddTransient<KeywordSerpData>()
              .AddTransient<KeywordRelatedData>()
              .AddTransient<ContentHitHistoryData>()
              .AddTransient<HttpErrorLogData>()
              .AddTransient<ThemeData>()
              .AddTransient<ThemeVersionData>()
              .AddTransient<ContentStartTemplateCategoryData>()

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
             .AddSingleton<SomeBlog.Wordpress.WpJson.Helper>()
             .AddScoped<DataContext>(x => new DataContext(configuration["DatabaseSettings:ConnectionString"]))
              .AddDbContext<DataContext>(ServiceLifetime.Scoped)
              .AddScoped<ContentDataContext>(x => new ContentDataContext(configuration["DatabaseSettings:ContentConnectionString"]))
              .AddDbContext<ContentDataContext>(ServiceLifetime.Scoped)
             .AddTransient(typeof(ILogger<>), typeof(Logger<>))
             .Configure<DatabaseSettings>(options
                 => options.ConnectionString = configuration["DatabaseSettings:ConnectionString"])
              .BuildServiceProvider();
        }

        public static string DownloadPostInImages(string value, string _basePath, string url, int blogId)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            serviceProvider = _buildProvider();

            var _mediaData = serviceProvider.GetService<MediaData>();
            var _blogData = serviceProvider.GetService<BlogData>();
            var blog = _blogData.GetByKey(blogId);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            var imageNodes = doc.DocumentNode.SelectNodes("//img//@src");
            if (imageNodes == null || imageNodes.Count == 0)
                return value;

            foreach (HtmlNode imageNode in imageNodes)
            {
                var remote_media_path = imageNode.Attributes["src"].Value;
                if (remote_media_path.StartsWith("data:image"))
                    continue;

                if (string.IsNullOrEmpty(remote_media_path))
                    continue;

                var local_media_path = remote_media_path.Replace(_basePath, "").TrimStart('/');
                if (local_media_path.StartsWith("https://"))
                    continue;

                var rootFolder = Directory.GetCurrentDirectory();
                string filePath = "wp-content\\uploads";

                var path = local_media_path.Split('/');
                var count = path.Count();

                var yearFile = "";
                var monthFile = "";
                if (count == 8)
                {
                    yearFile = path[5];
                    monthFile = path[6];
                }
                else
                {
                    yearFile = path[2];
                    monthFile = path[3];
                }

                if (!Directory.Exists($"{filePath}\\{yearFile}"))
                    Directory.CreateDirectory($"{filePath}\\{yearFile}");

                if (!Directory.Exists($"{filePath}\\{yearFile}\\{monthFile}"))
                    Directory.CreateDirectory($"{filePath}\\{yearFile}\\{monthFile}");

                var fileName = path.LastOrDefault();
                var resultPath = $"{filePath}\\{yearFile}\\{monthFile}\\{fileName}";

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");
                        wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                        wc.DownloadFile(remote_media_path, resultPath);

                        var newSrc = $"{filePath}/{yearFile}/{monthFile}/{fileName}";
                        newSrc = newSrc.Replace("\\", "/");
                        var media_src = $"/{newSrc}";
                        newSrc = $"{url}/{newSrc}";

                        var alt = "";
                        var title = "";

                        //imagenode'un attr'leri degistirelim
                        imageNode.SetAttributeValue("src", newSrc);

                        if (imageNode.Attributes["alt"] != null)
                        {
                            alt = imageNode.Attributes["alt"].Value;
                            imageNode.SetAttributeValue("alt", alt);
                        }

                        if (imageNode.Attributes["title"] != null)
                        {
                            title = imageNode.Attributes["title"].Value;
                            imageNode.SetAttributeValue("title", title);
                        }

                        if (imageNode.Attributes["srcset"] != null)
                            imageNode.Attributes.Remove(imageNode.Attributes["srcset"]);

                        if (imageNode.Attributes["sizes"] != null)
                            imageNode.Attributes.Remove(imageNode.Attributes["sizes"]);

                        if (blog != null)
                        {
                            var new_media = new Model.Media()
                            {
                                Alt = alt,
                                BlogId = blogId,
                                CreateDate = DateTime.Now,
                                ImageSlug = Path.GetFileNameWithoutExtension(fileName),
                                MediaUrl = media_src,
                                Title = title,
                                Type = 1,
                            };

                            _mediaData.Insert(new_media);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                value = writer.ToString();
            }

            return value;
        }

        public static void XmlRpcImporter()
        {
            serviceProvider = new ServiceCollection()
              .AddOptions()
              .AddSingleton<BlogData>()
              .AddSingleton<ContentData>()
              .AddSingleton<CommentData>()
              .AddSingleton<CategoryData>()
              .AddSingleton<MediaData>()
              .AddSingleton<TagData>()
              .AddSingleton<ContentCategoryData>()
              .AddSingleton<SomeBlog.Wordpress.WpJson.Helper>()
              .AddSingleton<ContentTagData>()
              .AddTransient(typeof(ILogger<>), typeof(Logger<>))
              .Configure<DatabaseSettings>(options
                  => options.ConnectionString = configuration["DatabaseSettings:ConnectionString"])
               .BuildServiceProvider();

            var _contentData = serviceProvider.GetService<ContentData>();
            var _commentData = serviceProvider.GetService<CommentData>();
            var _categoryData = serviceProvider.GetService<CategoryData>();
            var _contentCategoryData = serviceProvider.GetService<ContentCategoryData>();
            var _contentTagData = serviceProvider.GetService<ContentTagData>();
            var _mediaData = serviceProvider.GetService<MediaData>();
            var _tagData = serviceProvider.GetService<TagData>();
            var _blogData = serviceProvider.GetService<BlogData>();
            var _apiHelper = new SomeBlog.Wordpress.WpJson.Helper(_basePath);

            int ok = 0, no = 0, counter = 1;

            var xmlrpc_wp_config = new WordPressSiteConfig() { BaseUrl = _basePath, Username = "admin", Password = "Saglikliol123." };
            var xmlrpc_filter = new SomeBlog.Wordpress.XmlRpc.Models.PostFilter()
            {
                PostStatus = "draft",
                Offset = 0,
            };

            var posts = new WordPressClient(xmlrpc_wp_config).GetPosts(xmlrpc_filter);

            while (posts != null)
            {
                foreach (var p in posts)
                {
                    var featured = p.FeaturedImage as CookComputing.XmlRpc.XmlRpcStruct;
                    var featured_image = featured["link"].ToString();

                    var post_in_db = _contentData.GetBy(x => x.Slug == p.Title.ToSlug() && x.BlogId == _blogId).FirstOrDefault();
                    if (post_in_db != null)
                        continue;

                    var post = new Model.Content()
                    {
                        BlogId = _blogId,
                        CreateDate = DateTime.Now,
                        FocusKeyword = "",
                        HasFeatured = false,
                        Hit = 0,
                        IsActive = true,
                        IsDelete = false,
                        MetaDescription = p.Title,
                        MetaTitle = p.Title,
                        PageDescription = p.Title,
                        PageTitle = p.Title,
                        Text = p.Content,
                        Excerpt = p.Exerpt.ToStripHtml(),
                        PublishDate = p.ModifiedDateTime.AddYears(1),
                        UpdateDate = p.ModifiedDateTime,
                        Slug = p.Title.ToSlug(),
                        Tags = "",
                    };

                    var blog = _blogData.GetByKey(_blogId);
                    post.Text = post.Text.ToRemoveAnchors();
                    post.Text = DownloadPostInImages(post.Text, _basePath, blog.Url, _blogId);

                    var insertContent = _contentData.Insert(post);
                    if (!insertContent.IsSucceed)
                    {
                        Console.WriteLine($"- {post.PageTitle} eklenirken hata {insertContent.Message}");
                        no++;
                        continue;
                    }

                    Console.WriteLine("+ " + post.PageTitle);
                    ok++;

                    //feature media indir
                    if (!string.IsNullOrEmpty(featured_image))
                    {
                        try
                        {
                            var remote_media_path = featured_image;
                            var local_media_path = remote_media_path.Replace(_basePath, "").TrimStart('/');

                            Directory.CreateDirectory(Path.GetDirectoryName(local_media_path));

                            using (var client = new WebClient())
                            {
                                client.DownloadFile(remote_media_path, local_media_path);
                            }

                            var media = new Model.Media()
                            {
                                Alt = post.PageTitle ?? "",
                                ImageSlug = Path.GetFileNameWithoutExtension(local_media_path),
                                MediaUrl = local_media_path,
                                Title = post.PageTitle ?? "",
                                Type = 1,
                                CreateDate = DateTime.Now,
                                BlogId = _blogId
                            };

                            var mediaInsertResult = _mediaData.Insert(media);

                            if (mediaInsertResult.IsSucceed)
                            {
                                post.FeaturedMediaId = media.Id;

                                _contentData.Update(post);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

                xmlrpc_filter.Offset = counter * 10;
                posts = new WordPressClient(xmlrpc_wp_config).GetPosts(xmlrpc_filter);
                counter++;
            }

            Console.WriteLine($"Bitti=> {ok} ok / {no} no");
            Console.ReadLine();
        }
    }

    public class ObjectTest
    {
        public string attachment_id { get; set; }
    }
}