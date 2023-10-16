using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Data.Infrastructure.Entities;
using SomeBlog.Infrastructure.Extensions;
using SomeBlog.Infrastructure.Interfaces;
using SomeBlog.Infrastructure.Logging.DummyLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SomeBlog.Importer.Blogspot
{
    class Program
    {
        static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            serviceProvider = new ServiceCollection()
             .AddOptions()
             .AddSingleton<BlogData>()
             .AddSingleton<ContentData>()
             .AddSingleton<CategoryData>()
             .AddSingleton<ContentCategoryData>()
             .AddScoped<DataContext>(x => new DataContext("server=*******;port=3307;database=db_someblog;userId=us_someblog;Password='******'"))
              .AddDbContext<DataContext>(ServiceLifetime.Scoped)
             .AddTransient(typeof(ILogger<>), typeof(Logger<>))
             .Configure<DatabaseSettings>(options
                 => options.ConnectionString = "server==*******;port=3307;database=db_someblog;userId=us_someblog;Password='=*******'")
              .BuildServiceProvider();

            var _blogData = serviceProvider.GetService<BlogData>();
            var _contentData = serviceProvider.GetService<ContentData>();
            var _categoryData = serviceProvider.GetService<CategoryData>();
            var _contentCategoryData = serviceProvider.GetService<ContentCategoryData>();
            
            var blog = _blogData.FirstOrDefault(x => x.Id == 19);
            if(blog == null)
            {
                Console.WriteLine("Blog bulamadım");
                Console.ReadLine();
                return;
            }

            var local_categories = _categoryData.GetBy(x => x.BlogId == blog.Id);
            var local_contents = _contentData.GetBy(x => x.BlogId == blog.Id);

            var site_id = "8160329715328649270";
            var post_count = "500";
            var api_key = "AIzaSyCrRH7jDQH3pB4xZonQFmBg-9F8JGbggVE";

            var url = $"https://www.googleapis.com/blogger/v3/blogs/{site_id}/posts?key={api_key}&maxResults={post_count}";

            var json_text = "";
            using (WebClient wc = new WebClient())
            {
                json_text = wc.DownloadString(url);
            }

            if (!string.IsNullOrEmpty(json_text))
            {
                var json_model = JsonConvert.DeserializeObject<Models.Post>(json_text);
                if(json_model != null && json_model.posts.Count > 0)
                {
                    foreach (var item in json_model.posts)
                    {
                        UpdateContentMediaId(item.content, "https://1.bp.blogspot.com/", blog.Url, blog.Id, item.url.Split('/').LastOrDefault());
                        continue;

                        var category_insert_list = new List<Model.Category>();

                        foreach (var cat in item.labels)
                        {
                            var category_control = local_categories.Where(x => x.Name == cat).FirstOrDefault();
                            if(category_control == null)
                            {
                                category_insert_list.Add(new Model.Category()
                                {
                                    BlogId = blog.Id,
                                    Hit = 0,
                                    ImagePath = "",
                                    MetaDescription = cat,
                                    MetaTitle = cat,
                                    Name = cat,
                                    PageDescription = cat,
                                    PageTitle = cat,
                                    ParentId = -1,
                                    Slug = cat.ToSlug(),
                                });
                            }
                        }

                        if (category_insert_list.Count > 0)
                        {
                            var insert_category = _categoryData.InsertBulk(category_insert_list);
                            if (insert_category.IsSucceed)
                            {
                                local_categories.AddRange(category_insert_list);
                                Console.WriteLine("Kategoriler eklendi.");
                            }
                        }

                        item.url = item.url.Split('/').LastOrDefault().Replace(".html","");
                        var content_exist = local_contents.Where(x => x.Slug == item.url).FirstOrDefault();
                        if (content_exist != null)
                        {
                            Console.WriteLine($"{item.url} Zaten ekli");
                            continue;
                        }

                        var new_post = new Model.Content()
                        {
                            AccessibilityScore = 0,
                            BestPracticesScore = 0,
                            BlogId = blog.Id,
                            CreateDate = item.published,
                            CreatedById = -1,
                            Excerpt = item.title,
                            FeaturedMediaId = -1,
                            HasUpdatedAfterBotInsert = true,
                            Hit = 0,
                            IsActive = true,
                            IsBotContent = true,
                            IsDelete = false,
                            LastAuditDate = new DateTime(1999, 1, 1),
                            MetaDescription = item.title,
                            MetaTitle = item.title,
                            PageTitle = item.title,
                            LastGoogleBotDate = new DateTime(1999, 1, 1),
                            PageDescription = item.title,
                            PerformanceScore = 0,
                            PublishDate = item.published,
                            SeoScore = 0,
                            Slug = item.url,
                            SourcePath = "",
                            Text = item.content,
                            UpdateDate = item.updated,
                            UpdatedById = -1,
                        };

                        var content_insert = _contentData.Insert(new_post);
                        if (content_insert.IsSucceed)
                        {
                            Console.WriteLine($"{new_post.PageTitle} Eklendi");
                            local_contents.Add(new_post);
                            var content_category_insert_list = new List<Model.ContentCategory>();

                            foreach (var cat in item.labels)
                            {
                                var category = local_categories.Where(x => x.Name == cat).FirstOrDefault();
                                if(category != null)
                                {
                                    content_category_insert_list.Add(new Model.ContentCategory()
                                    {
                                        CategoryId = category.Id,
                                        ContentId = new_post.Id,
                                    });
                                }
                            }

                            if (content_category_insert_list.Count > 0)
                            {
                                _contentCategoryData.InsertBulk(content_category_insert_list);
                                Console.WriteLine("Kategoriler eklendi.");
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        public static string DownloadPostInImages(string value, string _basePath, string url, int blogId)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            serviceProvider = new ServiceCollection()
             .AddOptions()
             .AddSingleton<BlogData>()
             .AddSingleton<MediaData>()
             .AddScoped<DataContext>(x => new DataContext("server==*******;port=3307;database=db_someblog;userId=us_someblog;Password='=*******'"))
              .AddDbContext<DataContext>(ServiceLifetime.Scoped)
             .AddTransient(typeof(ILogger<>), typeof(Logger<>))
             .Configure<DatabaseSettings>(options
                 => options.ConnectionString = "server==*******;port=3307;database=db_someblog;userId=us_someblog;Password='=*******'")
              .BuildServiceProvider();

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
                var local_media_path = remote_media_path.Replace(_basePath, "").TrimStart('/');
                var rootFolder = Directory.GetCurrentDirectory();
                string filePath = "wp-content\\uploads";

                var path = local_media_path.Split('/');

                var fileName = path.LastOrDefault();
                var resultPath = $"{filePath}\\{fileName}";

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");
                        wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                        wc.DownloadFile(remote_media_path, resultPath);

                        var newSrc = $"{filePath}/{fileName}";
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

        public static string UpdateContentMediaId(string value, string _basePath, string url, int blogId, string slug)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            serviceProvider = new ServiceCollection()
             .AddOptions()
             .AddSingleton<BlogData>()
             .AddSingleton<ContentData>()
             .AddSingleton<MediaData>()
             .AddScoped<DataContext>(x => new DataContext("server==*******;port=3307;database=db_someblog;userId=us_someblog;Password='=*******'"))
              .AddDbContext<DataContext>(ServiceLifetime.Scoped)
             .AddTransient(typeof(ILogger<>), typeof(Logger<>))
             .Configure<DatabaseSettings>(options
                 => options.ConnectionString = "server==*******;port=3307;database=db_someblog;userId=us_someblog;Password='=*******'")
              .BuildServiceProvider();

            value = value.ToRemoveAnchors();

            var _mediaData = serviceProvider.GetService<MediaData>();
            var _contentData = serviceProvider.GetService<ContentData>();
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
                var local_media_path = remote_media_path.Replace(_basePath, "").TrimStart('/');
                var rootFolder = Directory.GetCurrentDirectory();
                string filePath = "wp-content\\uploads";

                var path = local_media_path.Split('/');
                var media_slug = slug.Replace(".html", "");

                media_slug = media_slug.ToSlug();

                var extension = Path.GetExtension(path.LastOrDefault());
                var resultPath = $"{filePath}\\{media_slug}{extension}";

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");
                        wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                        wc.DownloadFile(remote_media_path, resultPath);

                        var newSrc = $"{filePath}/{media_slug}{extension}";
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
                            var db_path = Path.GetFileNameWithoutExtension(path.LastOrDefault());
                            var content_slug = slug.Replace(".html", "");
                            var media = _mediaData.GetBy(x => x.ImageSlug == db_path).FirstOrDefault();
                            var content = new Model.Content();

                            if(media != null)
                            {
                                using (StringWriter writer = new StringWriter())
                                {
                                    doc.Save(writer);
                                    value = writer.ToString();
                                }

                                media.ImageSlug = media_slug;
                                media.MediaUrl = media_src;
                                _mediaData.Update(media);

                                content = _contentData.GetBy(x => x.Slug == content_slug).FirstOrDefault();
                                content.FeaturedMediaId = media.Id;
                                content.Text = value;

                                _contentData.Update(content);

                                continue;
                            }

                            var new_media = new Model.Media()
                            {
                                Alt = alt,
                                BlogId = blogId,
                                CreateDate = DateTime.Now,
                                ImageSlug = media_slug,
                                MediaUrl = media_src,
                                Title = title,
                                Type = 1,
                            };

                            _mediaData.Insert(new_media);

                            using (StringWriter writer = new StringWriter())
                            {
                                doc.Save(writer);
                                value = writer.ToString();
                            }

                            content = _contentData.GetBy(x => x.Slug == content_slug).FirstOrDefault();
                            content.FeaturedMediaId = media.Id;
                            content.Text = value;

                            _contentData.Update(content);

                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return "";
        }
    }
}
