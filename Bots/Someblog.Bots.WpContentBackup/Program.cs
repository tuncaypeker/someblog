using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using System;
using System.IO;
using System.Net;

namespace Someblog.Bots.WpContentBackup
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static MediaData mediaData;

        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            mediaData = serviceProvider.GetService<MediaData>();
            var blogData = serviceProvider.GetService<BlogData>();
            var contentData = serviceProvider.GetService<ContentData>();
            var botProgramData = serviceProvider.GetService<BotProgramData>();

            var backupFolder = "Backup";
            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            while (true)
            {
                var blogs = blogData.GetBy(x => !x.IsDemoBlog);
                foreach (var blog in blogs)
                {
                    Console.Clear();

                    var blogBackupFolder = backupFolder + "/" + blog.Name;
                    if (!Directory.Exists(blogBackupFolder))
                        Directory.CreateDirectory(blogBackupFolder);

                    var medias = mediaData.GetBy(x => x.BlogId == blog.Id);
                    Console.Title = blog.Name + "[" + medias.Count + " Medias]";
                    foreach (var media in medias)
                    {
                        botProgramData.UpdatePulse("Someblog.Bots.WpContentBackup");

                        if (media.MediaUrl.Contains("image/gif"))
                            continue;

                        if (media.MediaUrl.StartsWith("https://"))
                            continue;

                        var mediaPath = blogBackupFolder + "/" + media.MediaUrl;
                        if (!File.Exists(mediaPath))
                        {
                            Console.WriteLine(media.MediaUrl + " downloading...");
                            Directory.CreateDirectory(Path.GetDirectoryName(mediaPath));

                            using (var client = new WebClient())
                            {
                                try
                                {
                                    client.DownloadFile(blog.Url + "/" + media.MediaUrl, mediaPath);

                                    if (media.Width <= 1)
                                        UpdateWidthAndHeight(media, mediaPath);
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine("!!!" + exc.Message);
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                        }
                        else
                        {
                            if (media.Width <= 1)
                                UpdateWidthAndHeight(media, mediaPath);

                            Console.WriteLine(media.MediaUrl + " skipping...");
                        }
                    }
                }

                Console.WriteLine($"{DateTime.Now} 23 saat bekliycem.. Buraya hangfire koyalim bence");
                System.Threading.Thread.Sleep(23 * 60 * 60 * 1000);
            }
        }

        static void UpdateWidthAndHeight(SomeBlog.Model.Media media, string mediaPath)
        {
            using (Image image = Image.Load(mediaPath))
            {
                media.Width = image.Width;
                media.Height = image.Height;

                var dbUpdate = mediaData.Update(media);
            }
        }
    }
}
