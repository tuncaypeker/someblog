using HtmlAgilityPack;
using ImageMagick;
using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Infrastructure;
using SomeBlog.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SomeBlog.Bots.Pool.BlogImporter
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static MediaData _mediaData;
        static BlogData _blogData;
        static Data.Content.ContentMediaData _contentMediaData;
        static Data.Pool.PoolBlogImportRequestLogData _poolBlogImportRequestLogsData;
        static Data.Pool.PoolBlogImportRequestRemoveRuleData _poolBlogImportRequestRemoveRuleData;
        static Data.Pool.PoolBlogImportRequestReplaceRuleData _poolBlogImportRequestReplaceRuleData;
        static Encryption _enc;

        /// <summary>
        /// Havuzdaki bir blogdan terchilere gore icerigin, secilen blog'a aktarilmasi islemini yapar. Surekli ayakta kalmasina gerek yok 
        /// seklinde planlamışız. Ama bir an uzaktan import etmek istesek bu exe calismadigi icin import islemi bekleyecektir.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var _categoryData = serviceProvider.GetService<CategoryData>();
            var _contentCategoryData = serviceProvider.GetService<Data.Content.ContentCategoryData>();
            var _contentData = serviceProvider.GetService<Data.Content.ContentData>();
            var _contentStartTemplateData = serviceProvider.GetService<Data.Content.ContentStartTemplateData>();
            var _mediaSizeData = serviceProvider.GetService<MediaSizeData>();
            var _poolContentData = serviceProvider.GetService<Data.Pool.PoolContentData>();
            var _poolImportRequestData = serviceProvider.GetService<Data.Pool.PoolBlogImportRequestData>();
            var _poolContentCategoryData = serviceProvider.GetService<Data.Pool.PoolContentCategoryData>();
            var _poolContentTranslateData = serviceProvider.GetService<Data.Pool.PoolContentTranslateData>();
            var _poolCategoryData = serviceProvider.GetService<Data.Pool.PoolCategoryData>();
            var poolContentUsageData = serviceProvider.GetService<Data.Pool.PoolContentUsageData>();
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

            _poolBlogImportRequestLogsData = serviceProvider.GetService<Data.Pool.PoolBlogImportRequestLogData>();
            _poolBlogImportRequestRemoveRuleData = serviceProvider.GetService<Data.Pool.PoolBlogImportRequestRemoveRuleData>();
            _poolBlogImportRequestReplaceRuleData = serviceProvider.GetService<Data.Pool.PoolBlogImportRequestReplaceRuleData>();

            _mediaData = serviceProvider.GetService<MediaData>();
            _blogData = serviceProvider.GetService<BlogData>();
            _contentMediaData = serviceProvider.GetService<Data.Content.ContentMediaData>();
            _enc = new Encryption("62:.cH>6{H");

            var pool_import_requests = _poolImportRequestData.GetBy(x => !x.HasDone);

            foreach (var pool_import_request in pool_import_requests)
            {
                var blog = _blogData.FirstOrDefault(x => x.Id == pool_import_request.BlogId);
                if (blog == null)
                {
                    Console.WriteLine("Blog Bulunamadı");
                    continue;
                }

                var remove_rules = _poolBlogImportRequestRemoveRuleData.GetBy(x => x.PoolBlogImportRequestId == pool_import_request.Id, "Order", false);
                var replace_rules = _poolBlogImportRequestReplaceRuleData.GetBy(x => x.PoolBlogImportRequestId == pool_import_request.Id, "Id", false);

                var remove_rule_values = remove_rules.Select(x => x.Value).ToList();
                var remove_rule_isXPath = remove_rules.Select(x => x.IsXPath).ToList();

                var replace_rule_whats = replace_rules.Select(x => x.ReplaceWhat).ToList();
                var replace_rule_withs = replace_rules.Select(x => x.ReplaceWith).ToList();

                var categories_pool_blog = _poolCategoryData.GetBy(x => x.PoolBlogId == pool_import_request.PoolBlogId);
                var categories_blog = _categoryData.GetBy(x => x.BlogId == pool_import_request.BlogId);

                var category_mappings = new List<string>();
                if (!string.IsNullOrEmpty(pool_import_request.PostCategoryMapping))
                    category_mappings = pool_import_request.PostCategoryMapping.Split(';').ToList();

                var content_templates = new List<Model.ContentStartTemplate>();
                if (pool_import_request.PrependContentTemplate)
                    content_templates = _contentStartTemplateData.GetBy(x => x.BlogId == blog.Id);

                var appent_title_texts = new List<string>();
                if (!string.IsNullOrEmpty(pool_import_request.AppendToTitleText) && !string.IsNullOrWhiteSpace(pool_import_request.AppendToTitleText))
                    appent_title_texts = pool_import_request.AppendToTitleText.Split(',').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();

                var excludedIds = string.IsNullOrEmpty(pool_import_request.ExcludedIds)
                    ? new List<int>()
                    : pool_import_request.ExcludedIds.Split(',')
                    .Select(x => int.Parse(x))
                    .ToList();

                //PoolCategory tablosuna gore filtreleme yapar
                var only_these_category_ids = string.IsNullOrEmpty(pool_import_request.CategoryIds)
                    ? new List<int>()
                    : pool_import_request.CategoryIds.Split(',').Select(x => int.Parse(x)).ToList();
                var exclude_category_ids = string.IsNullOrEmpty(pool_import_request.ExcludeCategoryIds)
                    ? new List<int>()
                    : pool_import_request.ExcludeCategoryIds.Split(',').Select(x => int.Parse(x)).ToList();

                //Siralamada Rasgele secenegi var
                var pool_contents = _poolContentData.GetWithSkipAndTake(pool_import_request.PoolBlogId,
                    pool_import_request.SkipCount,
                    pool_import_request.TakeCount,
                    pool_import_request.OrderContentsBy == "Rasgele" ? "Id" : pool_import_request.OrderContentsBy);

                if (pool_import_request.OrderContentsBy == "Rasgele")
                    pool_contents = pool_contents.OrderBy(x => Guid.NewGuid()).ToList();

                var pool_import_logs = _poolBlogImportRequestLogsData.GetBy(x => x.PoolBlogId == pool_import_request.PoolBlogId);
                var publish_date = DateTime.Now;

                //hemen paylasilacak iceri sayisini kontrol etmek icin, her hemen paylas sonrasi artirilir ve kontrol edilir
                var postPublishCountNowCounter = 0;

                //zamanlanacak postlar icin oncelikle su saate ve dakikada paylas ayarı ilk seferde yapilir
                //ardindan kac saatte bir paylascak ayarina gore, publish date'ler ayarlanır
                bool has_content_publish_date_initialized = false;

                int counter = 0;
                foreach (var pool_content in pool_contents)
                {
                    botProgramData.UpdatePulse("SomeBlog.Bots.PoolBlogImporter");

                    if (!string.IsNullOrEmpty(pool_import_request.StopWords))
                    {
                        var stopWords = pool_import_request.StopWords.Split(',');
                        foreach (var stopWord in stopWords)
                        {
                            if (string.IsNullOrEmpty(stopWord) || string.IsNullOrWhiteSpace(stopWord))
                                continue;

                            if (pool_content.Title.Contains(stopWord))
                            {
                                Console.WriteLine($"Title Stop Word[{stopWord}] içeriyor. Atladım...");
                                continue;
                            }
                        }
                    }

                    //Check Categories Exclude or Selected Categories
                    //Request içerisinde bu kategırilerden alma ya da sadece bu kategorilerden al gibi ayarlar bulunuyor
                    //pool_content_categories ayni zamnada asagida category_mapping icin de kullanilacak
                    var pool_content_categories = _poolContentCategoryData.GetBy(x => x.PoolContentId == pool_content.Id);

                    var pool_category_local_ids = pool_content_categories.Select(x => x.PoolCategoryId);
                    var pool_categories_for_this_content = _poolCategoryData.GetBy(x => pool_category_local_ids.Contains(x.Id));

                    //sadece bu kategorlerden ekleme yap
                    if (only_these_category_ids.Count > 0)
                    {
                        if (!pool_categories_for_this_content.Any(x => only_these_category_ids.Contains(x.Id)))
                        {
                            Console.WriteLine($"Kategori filtresine takildim");
                            continue;
                        }
                    }

                    //exclude edilmis kategori'de ise devam etme
                    if (pool_categories_for_this_content.Any(x => exclude_category_ids.Contains(x.Id)))
                    {
                        Console.WriteLine($"Exclude edilen kategori vardi atladim");
                        continue;
                    }

                    // CHECK FILTER PROPERTIES
                    if (excludedIds.Any(x => x == pool_content.Id))
                        continue;

                    //Kelime sayısı filtresi varsa bunu burada kontrol ediyoruz, eğer 0 gonderildi ise dikkate alınmasın demektir
                    if (pool_import_request.MinWordCount > 0)
                    {
                        var word_count = pool_content.Content.ToWordCount();
                        if (pool_import_request.MinWordCount > word_count)
                        {
                            Console.WriteLine($"Min Word Count uygun degil [{word_count}/{pool_import_request.MinWordCount}]");
                            continue;
                        }
                    }

                    Console.Clear();

                    //acaba bu content bizim loglarda var mi? Yani bu import request, bu id ile, bu blog'a bu içeriği eklemiş mi
                    var poolImportLog = pool_import_logs.FirstOrDefault(x => x.PoolBlogImportRequestId == pool_import_request.Id && x.PoolContentId == pool_content.Id);
                    if (poolImportLog != null)
                    {
                        //bu icerik daha once bu request log kapsaminda ilgili siteye eklenmis, devam edip tekrar eklemenin anlami yok
                        //yeni referans publish date degerimde, bu makalenin paylasim tarhi olacaktir
                        publish_date = poolImportLog.PublishDate;
                        has_content_publish_date_initialized = true;
                        postPublishCountNowCounter += 1; //daha once paylasilmis sonucta, paylasilanlari bir arttirabiliriz.
                        continue;
                    }

                    var title = pool_content.Title;
                    var text = pool_content.Content;
                    var slug = pool_content.Title.ToSlug();

                    if (pool_import_request.PoolLanguageId > 0)
                    {
                        var poolContentTranslate = _poolContentTranslateData.FirstOrDefault(x => x.PoolContentId == pool_content.Id && x.PoolLanguageId == pool_import_request.PoolLanguageId);
                        if (poolContentTranslate != null)
                        {
                            title = poolContentTranslate.Title;
                            text = poolContentTranslate.Content;
                            slug = title.ToSlug();
                        }
                    }

                    var content_already_exist_in_db = _contentData.GetBy(x => x.Slug == slug && x.BlogId == pool_import_request.BlogId).FirstOrDefault();
                    if (content_already_exist_in_db != null) //slug eslesti, bot log yukarida kontrol edildiginde gore, gercekten sitede ayni slug ile icerik var.
                    {
                        //1- ya slug'i degistricez
                        //2- ya da continue diyip devam etcez
                        continue;
                    }

                    //varsa append to title text yapalim
                    if (appent_title_texts.Count > 0)
                    {
                        title += appent_title_texts.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                        slug = title.ToSlug();
                    }

                    //bu icerigin publish_date'ine karar ver, ya hemen yayinlanir, ya ilk secilen saat/dakika ile yayinlanir, ya da kac saatte bir paylas ayari ile ayarlanir
                    if (pool_import_request.PostPublishCountNow <= postPublishCountNowCounter)
                    {
                        if (!has_content_publish_date_initialized)
                        {
                            publish_date = new DateTime(publish_date.Year, publish_date.Month, publish_date.Day, pool_import_request.PostPublishHour, pool_import_request.PostPublishMinute, 00);
                            has_content_publish_date_initialized = true;
                        }
                        else //baslangic tarihi verildi ise, bundan sonrasi icin HourSpacing kadar öteleyerek devam edelim
                            publish_date = publish_date.AddHours(pool_import_request.AddPostHourSpacing);
                    }
                    else
                    {
                        //Hemen yayinlanacak icerik
                        publish_date = DateTime.Now;
                        postPublishCountNowCounter++;
                    }

                    Console.WriteLine(title + " ile calısıyorum => " + publish_date.ToString());

                    //Remove, Clear, Format Content
                    //-----------------------------------------------
                    if (pool_import_request.RemoveAnchors && pool_import_request.RemoveAnchorTagOnly)
                    {
                        var htmlDocumentForRemoveAnchorsOnly = new HtmlAgilityPack.HtmlDocument();
                        htmlDocumentForRemoveAnchorsOnly.LoadHtml(text);
                        var anchors = htmlDocumentForRemoveAnchorsOnly.DocumentNode.SelectNodes("//a");

                        if (anchors != null)
                        {
                            foreach (var anchor in anchors)
                            {
                                anchor.ParentNode.RemoveChild(anchor, true);
                            }
                        }

                        text = htmlDocumentForRemoveAnchorsOnly.DocumentNode.OuterHtml;
                    }
                    else if (pool_import_request.RemoveAnchors && !pool_import_request.RemoveAnchorTagOnly)
                    {
                        text = text.ToRemoveAnchors();
                    }

                    //remove attributes
                    if (!string.IsNullOrEmpty(pool_import_request.RemoveAttributes))
                    {
                        var attrArryToRemove = pool_import_request.RemoveAttributes.Split(',');
                        if (attrArryToRemove.Length > 0)
                        {
                            var htmlDocumentForRemoveAttrs = new HtmlAgilityPack.HtmlDocument();
                            htmlDocumentForRemoveAttrs.LoadHtml(text);

                            foreach (var attrToRemove in attrArryToRemove)
                            {
                                var attr = attrToRemove.Trim();
                                if (string.IsNullOrEmpty(attr))
                                    continue;

                                htmlDocumentForRemoveAttrs.IterateAllNodes(node =>
                                    {
                                        if (node.Attributes[attr] != null)
                                            node.Attributes[attr].Remove();
                                    }
                                );
                            }

                            text = htmlDocumentForRemoveAttrs.DocumentNode.OuterHtml;
                        }
                    }

                    //mix xpath
                    if (!string.IsNullOrEmpty(pool_import_request.MixXpath))
                    {
                        try
                        {
                            //xpath dogru mu
                            System.Xml.XPath.XPathExpression.Compile(pool_import_request.MixXpath);

                            var htmlDocumentForMix = new HtmlAgilityPack.HtmlDocument();
                            htmlDocumentForMix.LoadHtml(text);

                            var select_nodes = htmlDocumentForMix.DocumentNode.SelectNodes($"{pool_import_request.MixXpath}");
                            if (select_nodes != null && select_nodes.Count > 0)
                            {
                                var new_list = new List<HtmlNode>();
                                new_list.AddRange(select_nodes);
                                new_list = new_list.OrderBy(x => Guid.NewGuid()).ToList();

                                int item_count = 0;
                                foreach (HtmlNode node in select_nodes)
                                {
                                    var mixer_text = new StringBuilder(new_list[item_count].OuterHtml).ToString();

                                    try
                                    {
                                        node.ParentNode.ReplaceChild(HtmlNode.CreateNode(mixer_text), node);
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    item_count++;
                                }
                            }

                            text = htmlDocumentForMix.DocumentNode.OuterHtml;
                        }
                        catch (System.Xml.XPath.XPathException ex)
                        {

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    //eger template girilmeli secildi ise, rasgele bir tane ekle
                    if (pool_import_request.PrependContentTemplate && content_templates.Count > 0)
                    {
                        var botContentStartTemplate = content_templates.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                        var wordCount = text.Split(' ').Count();

                        //{page_title} {category} {publish_date} {read_in_minutes} {word_count}
                        var contentStartTemplate = botContentStartTemplate.Template
                            //.Replace("{category}", new_categories_to_insert.Count == 0 ? "genel" : new_categories_to_insert[0].Name)
                            .Replace("{page_title}", title)
                            .Replace("{publish_date}", publish_date.ToShortDateString())
                            .Replace("{read_in_minutes}", TimeSpan.FromSeconds(wordCount * 2).TotalMinutes.ToString())
                            .Replace("{word_count}", wordCount.ToString());

                        text = contentStartTemplate + " " + text;
                    }

                    var content = new Model.Content()
                    {
                        BlogId = pool_import_request.BlogId,
                        Excerpt = pool_content.Excerpt,
                        FeaturedMediaId = -1,
                        FocusKeyword = "",
                        HasFeatured = false,
                        HasUpdatedAfterBotInsert = true,
                        IsActive = pool_import_request.AddPostAsActive,
                        IsBotContent = true,
                        //excerpt translate edilmedigi icin description text'ten alinacak
                        MetaDescription = text.ToStripHtml().ToExecuteRemoveRules(remove_rule_values, remove_rule_isXPath)
                                    .ToExecuteReplaceRules(replace_rule_whats, replace_rule_withs).ToTrim(160),
                        MetaTitle = title.ToExecuteRemoveRules(remove_rule_values, remove_rule_isXPath)
                                    .ToExecuteReplaceRules(replace_rule_whats, replace_rule_withs),
                        //excerpt translate edilmedigi icin description text'ten alinacak
                        PageDescription = text.ToStripHtml().ToExecuteRemoveRules(remove_rule_values, remove_rule_isXPath)
                                    .ToExecuteReplaceRules(replace_rule_whats, replace_rule_withs).ToTrim(250),
                        PageTitle = title.ToExecuteRemoveRules(remove_rule_values, remove_rule_isXPath)
                                    .ToExecuteReplaceRules(replace_rule_whats, replace_rule_withs),
                        PublishDate = publish_date,
                        Slug = slug,
                        SourcePath = pool_content.Link,
                        AddType = Model.Enums.ContentAddType.PoolContentImport
                    };

                    content.Text = text.ToExecuteRemoveRules(remove_rule_values, remove_rule_isXPath)
                                   .ToExecuteReplaceRules(replace_rule_whats, replace_rule_withs);
                    //buradan itibaren artik text yerine content.Text kullanilmalidir cunku content'e ekleme islemi yapiliyor ve artik resmi olarak content.Text dolu
                    text = null;

                    var insert_content = _contentData.Insert(content);
                    if (!insert_content.IsSucceed)
                    {
                        Console.WriteLine("Icerik insert ederken hata oldu, : " + insert_content.Message);
                        System.Threading.Thread.Sleep(10 * 1000);
                        continue;
                    }

                    //# import yapildi log ekleyelim
                    var import_log = new Model.PoolBlogImportRequestLog()
                    {
                        BlogId = blog.Id,
                        ContentId = content.Id,
                        Created = DateTime.Now,
                        PoolContentId = pool_content.Id,
                        PublishDate = publish_date,
                        PoolBlogId = pool_import_request.PoolBlogId,
                        PoolBlogImportRequestId = pool_import_request.Id
                    };

                    var insert_log = _poolBlogImportRequestLogsData.Insert(import_log);
                    if (!insert_log.IsSucceed)
                    {
                        Console.WriteLine("Log kaydı ekleyemedim ama devam ediyorum, : " + insert_content.Message);
                        System.Threading.Thread.Sleep(10 * 1000);
                    }

                    //# icerik kullanim log'u ekleyelim
                    var contentUsage = new Model.PoolContentUsage()
                    {
                        BlogId = content.BlogId,
                        ContentId = content.Id,
                        CreateDate = DateTime.Now,
                        PoolContentId = pool_content.Id
                    };
                    var poolContenUsageResult = poolContentUsageData.Insert(contentUsage);

                    //# resimleri indirip, content'i guncelleyelim
                    var new_text = DownloadPostInImages(blog, content);
                    content.Text = new_text;
                    var contentUpdateResultAfterDownloadImages = _contentData.Update(content);

                    //kategori mapping yoksa devam etmeye gerek yok
                    if (category_mappings.Count > 0)
                    {
                        var content_category_list_to_insert_bulk = new List<Model.ContentCategory>();

                        foreach (var pc in pool_content_categories)
                        {
                            var pool_category = categories_pool_blog.Where(x => x.Id == pc.PoolCategoryId).FirstOrDefault();
                            if (pool_category == null)
                                continue;

                            var mapping = category_mappings.FirstOrDefault(x => x.Contains($"{pool_category.Id},"));
                            if (mapping != null)
                            {
                                var map_split = mapping.Split(',');
                                var category = categories_blog.FirstOrDefault(x => x.Id == Int32.Parse(map_split[1]));
                                if (category != null &&
                                    !content_category_list_to_insert_bulk.Any(x => x.CategoryId == category.Id && x.ContentId == content.Id))//iki kere ekleme ihtimali var mi
                                {
                                    content_category_list_to_insert_bulk.Add(new Model.ContentCategory()
                                    {
                                        CategoryId = category.Id,
                                        ContentId = content.Id,
                                    });
                                }
                            }
                        }
                        var insert_content_categories = _contentCategoryData.InsertBulk(content_category_list_to_insert_bulk);
                    }
                    else
                    {   //category mapping bulamadim, acaba request'te default category belirtildi mi
                        if (pool_import_request.DefaultCategoryId > 0)
                        {
                            var category = categories_blog.FirstOrDefault(x => x.Id == pool_import_request.DefaultCategoryId);
                            if (category != null)
                            {
                                _contentCategoryData.Insert(new Model.ContentCategory()
                                {
                                    CategoryId = category.Id,
                                    ContentId = content.Id,
                                });
                            }
                        }
                    }

                    Console.Title = $"PoolImportRequest {++counter}/{pool_contents.Count}";
                }

                pool_import_request.HasDone = true;
                pool_import_request.FinishDate = DateTime.Now;

                var requestUpdateResult = _poolImportRequestData.Update(pool_import_request);
            }

            return;
        }

        public static string DownloadPostInImages(Model.Blog blog, Model.Content content)
        {
            if (string.IsNullOrEmpty(content.Text))
                return "";

            var content_text = content.Text;
            var _mediaTools = new SomeBlog.Media.Tools();
            var _mediaConverters = new SomeBlog.Media.Converters();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content.Text);

            /* TODO: Resimler her zmn img etiketi ile gelmeyebilir mi? picture etiketi de kullanılmış olabilir */
            var imageNodes = doc.DocumentNode.SelectNodes("//img//@src");
            if (imageNodes == null || imageNodes.Count == 0)
                return content.Text;

            Console.WriteLine("=> " + imageNodes.Count + " image ile caliscam");

            var local_file_root_path = "wp-content\\uploads";
            if (!Directory.Exists(local_file_root_path))
                Directory.CreateDirectory(local_file_root_path);

            //FTP OPERATIONS AND FORMATS
            var ftpPassword = _enc.Decrypt(blog.FtpPassword);
            var ftpClient = new Infrastructure.Ftp.FtpFluentClient(blog.FtpAddress, blog.FtpPort, blog.FtpUserName, ftpPassword, stayConnected: true);

            int counter = 0;
            foreach (HtmlNode imageNode in imageNodes)
            {
                Console.WriteLine($"  => [{++counter}/{imageNodes.Count}] Resim ile calisiyorum..");

                //DOWNLOAD REMOTE IMAGE TO LOCAL AND DUPLICATE To ORIGINAL

                /*  Resim etiketi her zaman resmi "src" attribute ile saglamıyor. Bu yüzden burada küçük bir mantık ile ilerlemekte fayda var   */
                /*  Örnek olarak etiket icerisinde "lazy-src", "data-src" gibi bir attribute var mı, eğer varsa muhtemelen asıl resim adresi   */
                /*  bu olacaktır. Benzer şekilde "src" attribute'u "loading" içeriyorsa muhtemelen resmin ana adresi "src" içinde olmayacaktir */
                /*  zaten srcset attr'si varsa hiç başka attr bakmamıza da gerek kalmayacaktır.                                                 */
                string remote_media_path = "";
                string srcsetAttr = imageNode.GetAttributeValue("srcset", "");
                if (!string.IsNullOrEmpty(srcsetAttr) && srcsetAttr.Split(',').Length > 0)
                {
                    var srcSetArr = srcsetAttr.Split(',');
                    remote_media_path = srcSetArr[0].Split(' ')[0];
                }
                else
                {
                    var data_srcValue = imageNode.GetAttributeValue("data-src", "");
                    var lazy_srcValue = imageNode.GetAttributeValue("lazy-src", "");
                    var click_srcValue = imageNode.GetAttributeValue("click-src", "");
                    var srcValue = imageNode.GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(data_srcValue)) { remote_media_path = data_srcValue; }
                    else if (!string.IsNullOrEmpty(lazy_srcValue)) { remote_media_path = lazy_srcValue; }
                    else if (!string.IsNullOrEmpty(click_srcValue)) { remote_media_path = click_srcValue; }
                    else
                        remote_media_path = srcValue;
                }

                var file_name = Path.GetFileNameWithoutExtension(remote_media_path);
                var file_extension = Path.GetExtension(remote_media_path).TrimStart('.');
                var local_file_path = $"{local_file_root_path}\\{file_name}.{file_extension}";

                //oncelikle bu blog media'ları icinde boyle bir media var olabilir mi?
                //ayni isimle olabilir
                var mediaInDb = _mediaData.FirstOrDefault(x => x.ImageSlug == file_name && x.BlogId == blog.Id);
                int appendNumberToFileNameToMakeItUnique = 0;
                while (mediaInDb != null)
                {
                    appendNumberToFileNameToMakeItUnique += 1;

                    file_name = file_name + "-" + appendNumberToFileNameToMakeItUnique.ToString();
                    mediaInDb = _mediaData.FirstOrDefault(x => x.ImageSlug == file_name && x.BlogId == blog.Id);
                }

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");
                        wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                        wc.DownloadFile(remote_media_path, local_file_path);
                    }
                }
                catch (Exception ex)
                {
                    imageNode.ParentNode.RemoveChild(imageNode);
                    continue;
                }

                var orginal_file = $"{local_file_root_path}\\{file_name}-original.{file_extension}";
                File.Copy(local_file_path, $"{orginal_file}", overwrite: true);

                //SET NEW ATTRIBUTES HTML CONTENT
                var newSrc = $"{blog.Url}/wp-content/uploads/{file_name}.{file_extension}"
                    .Replace("\\", "/");

                var alt = imageNode.GetAttributeValue("alt", "");
                var title = imageNode.GetAttributeValue("title", "");

                //imagenode'un attr'leri degistirelim
                //aslinda bosa degistiriyoruz, cunku asagida <picture ile replace olmasini bekliyoruz
                imageNode.SetAttributeValue("src", newSrc);
                imageNode.SetAttributeValue("alt", string.IsNullOrEmpty(alt) ? content.PageTitle : "");
                imageNode.SetAttributeValue("title", string.IsNullOrEmpty(title) ? content.PageTitle : "");
                imageNode.SetAttributeValue("srcset", imageNode.GetAttributeValue("srcset", ""));
                imageNode.SetAttributeValue("sizes", imageNode.GetAttributeValue("sizes", ""));

                var media = new Model.Media()
                {
                    Alt = alt,
                    BlogId = blog.Id,
                    CreateDate = DateTime.Now,
                    ImageSlug = file_name,
                    MediaUrl = $"wp-content/uploads/{file_name}.{file_extension}",
                    Title = title,
                    CreatedById = -1,
                    HasAvif = false,
                    HasWebp = false,
                    Type = 1,
                };

                var dir_to_upload_ftp = $"wwwroot/wp-content/uploads/";

                _mediaTools.Optimize(local_file_path);
                var ftpUploadResult = ftpClient.Upload(local_file_path, dir_to_upload_ftp);
                if (!ftpUploadResult)
                    Console.WriteLine(" => Ana Dosya FTP upload edilirken hata oldu");

                var ftpUploadResultOriginal = ftpClient.Upload(orginal_file, dir_to_upload_ftp);
                if (!ftpUploadResultOriginal)
                    Console.WriteLine("       => Original Dosya FTP upload edilirken hata oldu");

                var webpPath = _mediaConverters.ToWebP(local_file_path);
                if (!string.IsNullOrEmpty(webpPath.Path))
                {
                    _mediaTools.Optimize(webpPath.Path);
                    var webpFtpUploadResult = ftpClient.Upload(webpPath.Path, dir_to_upload_ftp);
                    media.HasWebp = webpFtpUploadResult;
                    if (!webpFtpUploadResult)
                        Console.WriteLine("       => Webp FTP upload edilirken hata oldu");
                }
                else
                    Console.WriteLine("       => WebP Convert ederken hata oldu..");

                var avifPath = _mediaConverters.ToAvif(local_file_path);
                if (!string.IsNullOrEmpty(avifPath.Path))
                {
                    //cause used by another process error
                    //_mediaTools.Optimize(avifPath);

                    var avifFtpUploadResult = ftpClient.Upload(avifPath.Path, dir_to_upload_ftp);
                    media.HasAvif = avifFtpUploadResult;
                    if (!avifFtpUploadResult)
                        Console.WriteLine("=> Avif FTP upload edilirken hata oldu");
                }
                else
                    Console.WriteLine("       => Avif Convert ederken hata oldu..");

                if (File.Exists(orginal_file))
                {
                    try
                    {
                        using (var image = new MagickImage(orginal_file))
                        {
                            media.Width = image.Width;
                            media.Height = image.Height;
                        }
                    }
                    catch (Exception exc)
                    {
                    }
                }

                var insert_media = _mediaData.Insert(media);
                if (insert_media.IsSucceed)
                {
                    var insert_content_media = _contentMediaData.Insert(new Model.ContentMedia()
                    {
                        ContentId = content.Id,
                        IsExternal = false,
                        IsFeatured = false,
                        MediaId = media.Id,
                        Path = blog.Url + media.MediaUrl,
                    });
                    if (!insert_content_media.IsSucceed)
                    {
                        Console.WriteLine(" => Content Media Insert edilirken hata: " + insert_content_media.Message);
                        System.Threading.Thread.Sleep(2000);
                    }

                    var picture_image = new StringBuilder("<picture>");
                    var full_path = local_file_root_path.Replace("\\", "/") + "/" + file_name + "." + file_extension;

                    if (media.HasWebp)
                        picture_image.Append($"<source srcset='{blog.Url}/{full_path.Replace("." + file_extension, ".webp")}' type='image/webp' />");

                    if (media.HasAvif)
                        picture_image.Append($"<source srcset='{blog.Url}/{full_path.Replace("." + file_extension, ".avif")}' type='image/avif' />");

                    picture_image.Append($"<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+ip1sAAAAASUVORK5CYII='" +
                                                $"data-src='{blog.Url}/{media.MediaUrl}' alt='{media.Alt}' " +
                                                $"title='{media.Title}' class='w-100' width='{media.Width}' height='{media.Height}' />");

                    picture_image.Append("</picture>");
                    picture_image = picture_image.Replace("'", "\"");

                    imageNode.ParentNode.ReplaceChild(HtmlNode.CreateNode(picture_image.ToString()), imageNode);
                }

                Directory.Delete(local_file_root_path, recursive: true);
                Directory.CreateDirectory(local_file_root_path);
            }

            ftpClient.Disconnect();

            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                content_text = writer.ToString();
            }

            return content_text;
        }
    }

    public static class HtmlAgilityPackExtensions
    {
        public static void IterateAllNodes(this HtmlDocument doc, Action<HtmlNode> action)
        {
            foreach (var n in doc.DocumentNode.ChildNodes)
            {
                doIterateNode(n, action);
            }
        }

        private static void doIterateNode(HtmlNode node, Action<HtmlNode> action)
        {
            action?.Invoke(node);

            foreach (var n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                {
                    doIterateNode(n, action);
                }
            }
        }
    }
}
