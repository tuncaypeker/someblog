using HtmlAgilityPack;
using ImageMagick;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Infrastructure;
using SomeBlog.Infrastructure.Extensions;
using SomeBlog.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SomeBlog.Bots.ScrapeAndInsertBulkContent
{
    /// <summary>
    /// Geriye donuk olarak bir sitedeki icerikleri sitemap.xml gibi bir kaynaktan topluca alip sonrasinda
    /// scrape mantıgı ile sisteme eklemek
    /// 
    /// https://www.duzgunyolusec.com/mesafe/ku%C5%9Fadas%C4%B1/samsun-55-tr/ > arabablogu.com'a gibi
    /// </summary>
    /// <param name="args"></param>
    internal class Program
    {
        static ServiceProvider serviceProvider;
        static Data.MediaData _mediaData;
        static ContentMediaData _contentMediaData;
        static ContentData _contentData;

        static int blogId = 24;

        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            _mediaData = serviceProvider.GetService<MediaData>();
            _contentMediaData = serviceProvider.GetService<ContentMediaData>();
            _contentData = serviceProvider.GetService<ContentData>();

            var blogData = serviceProvider.GetService<BlogData>();
            var categoryData = serviceProvider.GetService<CategoryData>();
            var contentCategoryData = serviceProvider.GetService<ContentCategoryData>();
            var contentMetaData = serviceProvider.GetService<ContentMetaData>();

            //bu sayfadan once yetkili servis markalarinin listesini al
            //https://www.ototasarruf.com/yetkiliservisler.php
            var markaPaths = new List<string>() {

"https://www.ototasarruf.com/servisler.php?ustkatid=23",
"https://www.ototasarruf.com/servisler.php?ustkatid=14",
"https://www.ototasarruf.com/servisler.php?ustkatid=51",
"https://www.ototasarruf.com/servisler.php?ustkatid=81",
"https://www.ototasarruf.com/servisler.php?ustkatid=52",
"https://www.ototasarruf.com/servisler.php?ustkatid=26",
"https://www.ototasarruf.com/servisler.php?ustkatid=53",
"https://www.ototasarruf.com/servisler.php?ustkatid=36",
"https://www.ototasarruf.com/servisler.php?ustkatid=88",
"https://www.ototasarruf.com/servisler.php?ustkatid=44",
"https://www.ototasarruf.com/servisler.php?ustkatid=80",
"https://www.ototasarruf.com/servisler.php?ustkatid=27",
"https://www.ototasarruf.com/servisler.php?ustkatid=32",
"https://www.ototasarruf.com/servisler.php?ustkatid=83",
"https://www.ototasarruf.com/servisler.php?ustkatid=86",
"https://www.ototasarruf.com/servisler.php?ustkatid=54",
"https://www.ototasarruf.com/servisler.php?ustkatid=56",
"https://www.ototasarruf.com/servisler.php?ustkatid=40",
"https://www.ototasarruf.com/servisler.php?ustkatid=39",
"https://www.ototasarruf.com/servisler.php?ustkatid=79",
"https://www.ototasarruf.com/servisler.php?ustkatid=89",
"https://www.ototasarruf.com/servisler.php?ustkatid=57",
"https://www.ototasarruf.com/servisler.php?ustkatid=37",
"https://www.ototasarruf.com/servisler.php?ustkatid=90",
"https://www.ototasarruf.com/servisler.php?ustkatid=91",
"https://www.ototasarruf.com/servisler.php?ustkatid=24",
"https://www.ototasarruf.com/servisler.php?ustkatid=92",
"https://www.ototasarruf.com/servisler.php?ustkatid=25",
"https://www.ototasarruf.com/servisler.php?ustkatid=42",
"https://www.ototasarruf.com/servisler.php?ustkatid=38",
"https://www.ototasarruf.com/servisler.php?ustkatid=22",
"https://www.ototasarruf.com/servisler.php?ustkatid=45",
"https://www.ototasarruf.com/servisler.php?ustkatid=47",
"https://www.ototasarruf.com/servisler.php?ustkatid=28",
"https://www.ototasarruf.com/servisler.php?ustkatid=85",
"https://www.ototasarruf.com/servisler.php?ustkatid=49",
"https://www.ototasarruf.com/servisler.php?ustkatid=58",
"https://www.ototasarruf.com/servisler.php?ustkatid=59",
"https://www.ototasarruf.com/servisler.php?ustkatid=78",
"https://www.ototasarruf.com/servisler.php?ustkatid=60",
"https://www.ototasarruf.com/servisler.php?ustkatid=95",
"https://www.ototasarruf.com/servisler.php?ustkatid=77",
"https://www.ototasarruf.com/servisler.php?ustkatid=93",
"https://www.ototasarruf.com/servisler.php?ustkatid=61",
"https://www.ototasarruf.com/servisler.php?ustkatid=15",
"https://www.ototasarruf.com/servisler.php?ustkatid=35",
"https://www.ototasarruf.com/servisler.php?ustkatid=87",
"https://www.ototasarruf.com/servisler.php?ustkatid=69",
"https://www.ototasarruf.com/servisler.php?ustkatid=12",
"https://www.ototasarruf.com/servisler.php?ustkatid=30",
"https://www.ototasarruf.com/servisler.php?ustkatid=16",
"https://www.ototasarruf.com/servisler.php?ustkatid=94",
"https://www.ototasarruf.com/servisler.php?ustkatid=62",
"https://www.ototasarruf.com/servisler.php?ustkatid=43",
"https://www.ototasarruf.com/servisler.php?ustkatid=41",
"https://www.ototasarruf.com/servisler.php?ustkatid=13",
"https://www.ototasarruf.com/servisler.php?ustkatid=17",
"https://www.ototasarruf.com/servisler.php?ustkatid=63",
"https://www.ototasarruf.com/servisler.php?ustkatid=64",
"https://www.ototasarruf.com/servisler.php?ustkatid=82",
"https://www.ototasarruf.com/servisler.php?ustkatid=96",
"https://www.ototasarruf.com/servisler.php?ustkatid=99",
"https://www.ototasarruf.com/servisler.php?ustkatid=84",
"https://www.ototasarruf.com/servisler.php?ustkatid=65",
"https://www.ototasarruf.com/servisler.php?ustkatid=18",
"https://www.ototasarruf.com/servisler.php?ustkatid=48",
"https://www.ototasarruf.com/servisler.php?ustkatid=66",
"https://www.ototasarruf.com/servisler.php?ustkatid=67",
"https://www.ototasarruf.com/servisler.php?ustkatid=11",
"https://www.ototasarruf.com/servisler.php?ustkatid=68",
"https://www.ototasarruf.com/servisler.php?ustkatid=50",
"https://www.ototasarruf.com/servisler.php?ustkatid=31",
"https://www.ototasarruf.com/servisler.php?ustkatid=70",
"https://www.ototasarruf.com/servisler.php?ustkatid=71",
"https://www.ototasarruf.com/servisler.php?ustkatid=98",
"https://www.ototasarruf.com/servisler.php?ustkatid=46",
"https://www.ototasarruf.com/servisler.php?ustkatid=72",
"https://www.ototasarruf.com/servisler.php?ustkatid=97",
"https://www.ototasarruf.com/servisler.php?ustkatid=19",
"https://www.ototasarruf.com/servisler.php?ustkatid=73",
"https://www.ototasarruf.com/servisler.php?ustkatid=55",
"https://www.ototasarruf.com/servisler.php?ustkatid=74",
"https://www.ototasarruf.com/servisler.php?ustkatid=20",
"https://www.ototasarruf.com/servisler.php?ustkatid=21",
"https://www.ototasarruf.com/servisler.php?ustkatid=76",
"https://www.ototasarruf.com/servisler.php?ustkatid=75",
"https://www.ototasarruf.com/servisler.php?ustkatid=33",
"https://www.ototasarruf.com/servisler.php?ustkatid=29",
            };

            //sonra bu sayfalari gezerek icerdeki il bazinda listeiy al
            foreach (var markaPath in markaPaths)
            {
                var html = _getHtml(markaPath);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var anchors = htmlDocument.DocumentNode.SelectNodes("//div[@class='content']/div/a");
                var links = anchors.Select(x => x.Attributes["href"].Value);

                var workedLinks = File.ReadLines("worked-links.txt");
                int counter = 0;

                //bu alt kategorideki tum conent'leri aslinda almis olayim
                var contents = new List<Content>();

                foreach (var path in links)
                {
                    counter += 1;
                    Console.WriteLine($"{counter}: {path}");
                    if (counter == 100)
                        Console.Clear();

                    if (workedLinks.Contains("https://www.ototasarruf.com/" + path))
                        continue;

                    var content = _scrapeContent("https://www.ototasarruf.com/" + path);
                    if (content == null)
                        continue;


                    contents.Add(content);

                    //bu arada buraya gelen tüm liste için bir ust sayfa olusturmalı ve burada listeyi gostermelisin
                    //AKL Bayileri sayfasi > altında bu foreach ile gelenler diye dusnmelisin
                    //bazi bayiler gelmedigi icin once toplamalisin diyorum. yani buraya content'leri topla listeye al, disari cikip sonra ust ysyfayi olustur ve bulk content bas

                    //var takeSS = _screenshot($"https://www.google.com/maps/dir/{scrapeResult.From}/{scrapeResult.To}").Result;
                }

                if (contents.Count == 0)
                    continue;


                //Marka nın il il linklerini grostereceimiz sayfa olusuyor aslinda
                var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='yazi']");
                var title = titleNode.InnerText.Replace("\n", "").Replace("Servisler", "Servis").Replace("&amp;", "ve") + "in İllere Göre Listesi";
                var description = $"{titleNode.InnerText.Replace("\n", "").Replace("Servisler", "Servis").Replace("&amp;", "ve")}in türkiyede " +
                    $"illere göre listelerini sizler için derledik. Bulunduğunuz şehirdeki" +
                    $" {titleNode.InnerText.Replace("\n", "").Replace("Servisler", "Servis").Replace("&amp;", "ve")}ini listeden seçebilirsiniz.";

                var text = $"<p>{description}</p><ul>";

                foreach (var content in contents)
                    text += $"<li><a href='https://www.arabablogu.com/{content.Slug}/' title='{content.PageTitle}'>{content.PageTitle}</a></li>";

                text += "</ul>";

                var mainContent = new Content()
                {
                    AddType = Model.Enums.ContentAddType.BotAdd,
                    BlogId = 24,
                    CreateDate = DateTime.Now,
                    CreatedById = 1,
                    FocusKeyword = "",
                    FeaturedMediaId = -1,
                    HasFeatured = false,
                    HasUpdatedAfterBotInsert = true,
                    IsActive = true,
                    IsBotContent = true,
                    IsDelete = false,
                    IsIndexed = false,
                    IsIndexedCheckDate = DateTime.Now,
                    MetaDescription = description,
                    MetaTitle = title,
                    PageDescription = description,
                    PageTitle = title,
                    PublishDate = DateTime.Now,
                    Slug = title.Replace("in İllere Göre Listesi", "").ToSlug(),
                    SourcePath = markaPath,
                    Text = text,
                    UpdateDate = DateTime.Now,
                    UpdatedById = 1
                };

                var dbResul2 = _contentData.Insert(mainContent);

                //kategorileri ekleyelim
                var contentCategories2 = new Model.ContentCategory()
                {
                    CategoryId = 786,
                    ContentId = mainContent.Id
                };
                var dbResultContentCategories2 = contentCategoryData.Insert(contentCategories2);

                foreach (var content in contents)
                {
                    var dbResult = _contentData.Insert(content);

                    //kategorileri ekleyelim
                    var contentCategories = new Model.ContentCategory()
                    {
                        CategoryId = 786,
                        ContentId = content.Id
                    };
                    var dbResultContentCategories = contentCategoryData.Insert(contentCategories);

                    //burada da image'i ekleyelim feature olarak
                    //_addFeaturedImageFromScreenshotAndDelete(blog, scrapeResult.Content);

                    File.AppendAllLines("worked-links.txt", new List<string>()
                    {
                        content.SourcePath
                    });
                }
            }

            //sonra ic sayfalari gezerek html'leri al



            Console.WriteLine("done");
            Console.Read();
        }

        private static bool _addFeaturedImageFromScreenshotAndDelete(Model.Blog blog, Model.Content content)
        {
            var _enc = new Encryption("62:.cH>6{H");
            var _mediaTools = new SomeBlog.Media.Tools();
            var _mediaConverters = new SomeBlog.Media.Converters();

            //FTP OPERATIONS AND FORMATS
            var ftpPassword = _enc.Decrypt(blog.FtpPassword);
            var ftpClient = new Infrastructure.Ftp.FtpFluentClient(blog.FtpAddress, blog.FtpPort, blog.FtpUserName, ftpPassword, stayConnected: true);


            //DOWNLOAD REMOTE IMAGE TO LOCAL AND DUPLICATE To ORIGINAL

            /*  Resim etiketi her zaman resmi "src" attribute ile saglamıyor. Bu yüzden burada küçük bir mantık ile ilerlemekte fayda var   */
            /*  Örnek olarak etiket icerisinde "lazy-src", "data-src" gibi bir attribute var mı, eğer varsa muhtemelen asıl resim adresi   */
            /*  bu olacaktır. Benzer şekilde "src" attribute'u "loading" içeriyorsa muhtemelen resmin ana adresi "src" içinde olmayacaktir */
            /*  zaten srcset attr'si varsa hiç başka attr bakmamıza da gerek kalmayacaktır.                                                 */
            var file_name = content.PageTitle.ToSlug();
            var file_extension = "jpg";
            var local_file_path = $"{file_name}.{file_extension}";

            File.Copy("screenshot.jpg", local_file_path, overwrite: true);

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

            var orginal_file_path = $"{file_name}-original.{file_extension}";
            File.Copy(local_file_path, $"{orginal_file_path}", overwrite: true);

            //SET NEW ATTRIBUTES HTML CONTENT
            var newSrc = $"{blog.Url}/wp-content/uploads/{file_name}.{file_extension}"
                .Replace("\\", "/");

            var media = new Model.Media()
            {
                Alt = content.PageTitle,
                BlogId = blog.Id,
                CreateDate = DateTime.Now,
                ImageSlug = file_name,
                MediaUrl = $"wp-content/uploads/{file_name}.{file_extension}",
                Title = content.PageTitle,
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

            var ftpUploadResultOriginal = ftpClient.Upload(orginal_file_path, dir_to_upload_ftp);
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

            if (File.Exists(orginal_file_path))
            {
                try
                {
                    using (var image = new MagickImage(orginal_file_path))
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
                    IsFeatured = true,
                    MediaId = media.Id,
                    Path = blog.Url + media.MediaUrl,
                });

                if (!insert_content_media.IsSucceed)
                {
                    Console.WriteLine(" => Content Media Insert edilirken hata: " + insert_content_media.Message);
                    System.Threading.Thread.Sleep(2000);
                }

                content.FeaturedMediaId = media.Id;

                var postUpdateResult = _contentData.Update(content);
            }

            ftpClient.Disconnect();

            if (File.Exists(orginal_file_path)) File.Delete(orginal_file_path);
            if (File.Exists(webpPath.Path)) File.Delete(webpPath.Path);
            if (File.Exists(avifPath.Path)) File.Delete(avifPath.Path);
            if (File.Exists(local_file_path)) File.Delete(local_file_path);

            return true;
        }

        private static bool _addFeaturedImage(string remote_media_path, Model.Blog blog, Model.Content content)
        {
            var _enc = new Encryption("62:.cH>6{H");
            var _mediaTools = new SomeBlog.Media.Tools();
            var _mediaConverters = new SomeBlog.Media.Converters();

            var local_file_root_path = "wp-content\\uploads";
            if (!Directory.Exists(local_file_root_path))
                Directory.CreateDirectory(local_file_root_path);

            //FTP OPERATIONS AND FORMATS
            var ftpPassword = _enc.Decrypt(blog.FtpPassword);
            var ftpClient = new Infrastructure.Ftp.FtpFluentClient(blog.FtpAddress, blog.FtpPort, blog.FtpUserName, ftpPassword, stayConnected: true);


            //DOWNLOAD REMOTE IMAGE TO LOCAL AND DUPLICATE To ORIGINAL

            /*  Resim etiketi her zaman resmi "src" attribute ile saglamıyor. Bu yüzden burada küçük bir mantık ile ilerlemekte fayda var   */
            /*  Örnek olarak etiket icerisinde "lazy-src", "data-src" gibi bir attribute var mı, eğer varsa muhtemelen asıl resim adresi   */
            /*  bu olacaktır. Benzer şekilde "src" attribute'u "loading" içeriyorsa muhtemelen resmin ana adresi "src" içinde olmayacaktir */
            /*  zaten srcset attr'si varsa hiç başka attr bakmamıza da gerek kalmayacaktır.                                                 */
            var file_name = content.PageTitle.ToSlug();
            var file_extension = "png";
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
                return false; ;
            }

            var orginal_file_path = $"{local_file_root_path}\\{file_name}-original.{file_extension}";
            File.Copy(local_file_path, $"{orginal_file_path}", overwrite: true);

            //SET NEW ATTRIBUTES HTML CONTENT
            var newSrc = $"{blog.Url}/wp-content/uploads/{file_name}.{file_extension}"
                .Replace("\\", "/");

            var media = new Model.Media()
            {
                Alt = content.PageTitle,
                BlogId = blog.Id,
                CreateDate = DateTime.Now,
                ImageSlug = file_name,
                MediaUrl = $"wp-content/uploads/{file_name}.{file_extension}",
                Title = content.PageTitle,
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

            var ftpUploadResultOriginal = ftpClient.Upload(orginal_file_path, dir_to_upload_ftp);
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

            if (File.Exists(orginal_file_path))
            {
                try
                {
                    using (var image = new MagickImage(orginal_file_path))
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
                    IsFeatured = true,
                    MediaId = media.Id,
                    Path = blog.Url + media.MediaUrl,
                });

                if (!insert_content_media.IsSucceed)
                {
                    Console.WriteLine(" => Content Media Insert edilirken hata: " + insert_content_media.Message);
                    System.Threading.Thread.Sleep(2000);
                }

                content.FeaturedMediaId = media.Id;

                var postUpdateResult = _contentData.Update(content);

                Directory.Delete(local_file_root_path, recursive: true);
                Directory.CreateDirectory(local_file_root_path);
            }

            ftpClient.Disconnect();

            return true;
        }

        private static Content _scrapeContent(string path)
        {
            var html = _getHtml(path);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='yazi']");
            var title = titleNode.InnerText;
            var description = $"{title} ve bayilerin Adres, telefon bilgilerini bulabilirsiniz. {title}'nin tam listesi bu sayfada.";

            var table = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='content']/table");
            if (table == null)
            {
                table = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='content']/div/table");
                if (table == null)
                    return null;
            }

            var text = $"<p>Bu sayfamızda {title}'nin listesini sizler için derledik. <strong>{title}</strong>'ne ait telefon ve adres gibi iletişim bilgilerini ilçe " +
                $"bazında bulabilirsiniz. Aynı zamanda konum bilgisine sahip olduğumuz işletmeler için, adres bilgisi altında bulunan 'Harita'da Göster' bağlantısı " +
                $"ile {title} nin google haritalarda konumunu görebilir ve adres tarifi alabilirsiniz</p>";
            text += $"<p>Eğer {title} listesine ekleme, güncelleme yapmak isterseniz, ya da bu listeden bayi&yetkili servis bilgisi kaldırmak isterseniz, " +
                $"info_arabablogu.com mail adresine iletişim bilgileri ile birlikte güncelleme talebi gönderebilirsiniz.</p>";
            text += table.OuterHtml.Replace("Yol Tarifi Al", "Haritada Göster")
                .Replace("border=\"1\"","")
                .Replace("cellpadding=\"1\"","")
                .Replace("cellspacing=\"1\"","")
                .Replace("style=\"width:100%;\"","")
                .Replace("Adres", "Adresi")
                .Replace("Telefon", "Telefonu");
            text += "<style>table{ width:100% } td{ padding:3px; } table,td{ border-collapse:collapse;border:solid 1px #ccc }</style>";

            title = title + " Adres, Telefon Bilgileri";

            return new Content()
            {
                AccessibilityScore = 0,
                AddType = 0,
                BestPracticesScore = 0,
                BlogId = 24,
                CreateDate = DateTime.Now,
                CreatedById = 0,
                Excerpt = "",
                FocusKeyword = "",
                HasFeatured = false,
                HasUpdatedAfterBotInsert = true,
                Hit = 0,
                IsActive = true,
                IsBotContent = true,
                IsDelete = false,
                IsIndexed = false,
                IsIndexedCheckDate = DateTime.Now,
                LastAuditDate = DateTime.Now,
                LastGoogleBotDate = DateTime.Now,
                MetaDescription = description,
                MetaTitle = title,
                PageDescription = description,
                PageTitle = title,
                PublishDate = DateTime.Now,
                Slug = title.ToSlug(),
                Slug2 = null,
                Text = text,
                UpdateDate = DateTime.Now,
                UpdatedById = 0,
                SourcePath = path
            };
        }

        private static string _getHtml(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                    client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    client.Headers.Add("Accept-Language", "tr;q=0.7");
                    client.Encoding = System.Text.Encoding.GetEncoding("iso-8859-9");

                    var html = client.DownloadString(path);

                    return html;
                }
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        private static List<string> _readFromSitemapXml(string sitemapURL, bool downloadFromRemote = false)
        {
            string sitemapString = "";

            if (downloadFromRemote)
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    sitemapString = wc.DownloadString(sitemapURL);
                }
            }
            else
                sitemapString = File.ReadAllText(sitemapURL);

            var urlList = new List<string>();

            XmlDocument urldoc = new XmlDocument();
            /*Load the downloaded string as XML*/
            urldoc.LoadXml(sitemapString);
            /*Create an list of XML nodes from the url nodes in the sitemap*/
            XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("url");
            foreach (XmlNode node in xmlSitemapList)
            {
                if (node["loc"] != null)
                    urlList.Add(node["loc"].InnerText);
            }

            return urlList;
        }

        private static async Task<bool> _screenshot(string path)
        {
            using (Browser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (PuppeteerSharp.Page page = await browser.NewPageAsync())
                {
                    await page.SetViewportAsync(new ViewPortOptions
                    {
                        Width = 1920,
                        Height = 1080
                    });

                    await page.GoToAsync(path);

                    string content = await page.GetContentAsync();

                    await page.ScreenshotAsync($"screenshot.jpg");
                    await browser.CloseAsync();
                }
            }

            return true;
        }
    }
}
