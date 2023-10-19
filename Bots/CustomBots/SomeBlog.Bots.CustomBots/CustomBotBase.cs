using System.Net;
using System;
using SomeBlog.Infrastructure;

namespace SomeBlog.Bots.CustomBots
{
	public abstract class CustomBotBase : ICustomBot
	{
        protected readonly int _idInDb;

        public CustomBotBase(int idInDb)
        {
            _idInDb = idInDb;
        }

        public abstract void Execute(Model.Blog blog);

         /// <summary>
        /// uzaktaki resmi indirir ve hash degistirir
        /// </summary>
        /// <param name="remote_media_path"></param>
        /// <param name="local_media_path"></param>
        /// <returns></returns>
        protected bool DownloadRemoteImage(string remote_media_path, string local_media_path, bool changeHash = true)
        {
            HashHelper hashHelper = new HashHelper();

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");
                    wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

                    wc.DownloadFile(remote_media_path, local_media_path);

                    if (changeHash)
                        hashHelper.ChangeHash(local_media_path);

                    return true;
                }
            }
            catch (Exception exc)
            {
                return false;
            }
        }
	}
}
