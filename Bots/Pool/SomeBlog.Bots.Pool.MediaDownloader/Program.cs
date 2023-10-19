using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using System;
using System.IO;
using System.Net;

namespace SomeBlog.Bots.PoolMediaDownloader
{
    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 5 * 1000;
            return w;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var _blogData = serviceProvider.GetService<Data.Pool.PoolBlogData>();
            var _contentData = serviceProvider.GetService<Data.Pool.PoolContentData>();
            var _mediaData = serviceProvider.GetService<Data.Pool.PoolMediaData>();
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

            while (true)
            {
                botProgramData.UpdatePulse("SomeBlog.Bots.PoolMediaDownloader");

                var pool_media_not_downladed = _mediaData.FirstOrDefault(x => !x.HasDownloaded && !x.HasError);
                if (pool_media_not_downladed == null)
                {
                    Console.WriteLine("media gelmedi, biraz bekliycem...");
                    System.Threading.Thread.Sleep(10 * 1000);
                    continue;
                }

                var poolContent = _contentData.GetByKey(pool_media_not_downladed.PoolContentId);
                if (poolContent == null) {
                    var dbDeleteResult = _mediaData.DeleteByKey(pool_media_not_downladed.Id);
                    Console.WriteLine($"Content[{pool_media_not_downladed.PoolContentId}] bulunamadi, media siliniyor: " + dbDeleteResult.IsSucceed);
                    continue;
                }
                var poolBlog = _blogData.GetByKey(poolContent.PoolBlogId);
                if (poolBlog == null) {
                    var dbDeleteResult = _mediaData.DeleteByKey(pool_media_not_downladed.Id);
                    Console.WriteLine($"Blog[{poolContent.PoolBlogId}] bulunamadi, media siliniyor: " + dbDeleteResult.IsSucceed);
                    continue;
                }

                if (!pool_media_not_downladed.RemotePath.StartsWith("http"))
                    pool_media_not_downladed.RemotePath = poolBlog.Path.TrimEnd('/') + "/" + pool_media_not_downladed.RemotePath.TrimStart('/');

                //download edelim
                var extension = Path.GetExtension(pool_media_not_downladed.RemotePath).Trim('.');
                if (string.IsNullOrEmpty(extension))
                    extension = "jpg";

                Directory.CreateDirectory($"PoolMedias/{poolBlog.Name}");

                var local_path = $"PoolMedias/{poolBlog.Name}/{pool_media_not_downladed.PoolContentId}_{pool_media_not_downladed.Id}.{extension}";
                using (var client = new MyWebClient())
                {
                    try
                    {
                        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
                        client.DownloadFile(pool_media_not_downladed.RemotePath, local_path);
                    }
                    catch (WebException exc)
                    {
                        HttpStatusCode? status = (exc.Response as HttpWebResponse)?.StatusCode;

                        pool_media_not_downladed.HasError = true;
                        pool_media_not_downladed.StatusCode = status.HasValue
                            ? (int)status
                            : -1;

                        var updateResultError = _mediaData.Update(pool_media_not_downladed);

                        Console.WriteLine("media download edilemedi, biraz bekliycem... : " + exc.Message);
                        continue;
                    }
                    catch (Exception exc)
                    {
                        //burada takilcaz heralde;

                        Console.WriteLine("media download edilemedi, biraz bekliycem... : " + exc.Message);
                        continue;
                    }
                }

                //modeli duzenleyelim
                pool_media_not_downladed.HasDownloaded = true;
                pool_media_not_downladed.LocalPath = local_path;

                var updateResult = _mediaData.Update(pool_media_not_downladed);
                Console.WriteLine($"{pool_media_not_downladed.Id} Download yapıldı, DbUpdate : " + updateResult.IsSucceed);
            }
        }
    }
}
