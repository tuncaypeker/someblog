using Org.BouncyCastle.Asn1.Cms;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Model;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SomeBlog.Bots.CustomBots
{
	public class AkakceAktuelCustomBot : CustomBotBase
	{
		ContentData contentData;
		MediaData mediaData;
		CustomBotPostHistoryData customBotPostHistoryData;

		public AkakceAktuelCustomBot(ContentData contentData, CustomBotPostHistoryData customBotPostHistoryData, MediaData mediaData)
			: base(1)
		{
			this.contentData = contentData;
			this.customBotPostHistoryData = customBotPostHistoryData;
			this.mediaData = mediaData;
		}

		public override void Execute(Model.Blog blog)
		{
			var list = new List<string>() {
				"https://www.akakce.com/brosurler/a101",
				"https://www.akakce.com/brosurler/bim",
				"https://www.akakce.com/brosurler/sok"
			};

			foreach (var item in list)
			{
				var html = GetHtml(item);

				//burada linkleri alcaz icerisinde post paylasim gecenleri almayip digerlerini sirayla gezcez
				//content generatae edip liste olusturcaz yada db de var mi bakip direk bam bam bam ekliycez

				var document = new HtmlAgilityPack.HtmlDocument();
				document.LoadHtml(html);

				var brosurLinks = document.DocumentNode.SelectNodes("//ul[@id='BLI']/li/a");
				if (brosurLinks.Count == 0)
					continue;

				foreach (var brosurLink in brosurLinks)
				{
					var hrefAttribute = brosurLink.Attributes["href"];
					if (hrefAttribute == null || string.IsNullOrEmpty(hrefAttribute.Value) || hrefAttribute.Value.Contains("post") || hrefAttribute.Value.Contains("paylaşım"))
						continue;

					var brosurPath = "https://www.akakce.com" + hrefAttribute.Value;

					//bu brosur path source olarak kullaniliyor o yuzden bu blog icin sorgulayalim bakalim
					var contentInDb = contentData.FirstOrDefault(x => x.BlogId == blog.Id && x.SourcePath == brosurPath);
					if (contentInDb != null) //daha once db'ye eklenmis
						continue;

					var brosurHtml = GetHtml(brosurPath);
					var brosurDocument = new HtmlAgilityPack.HtmlDocument();
					brosurDocument.LoadHtml(brosurHtml);

					var imageNodes = brosurDocument.DocumentNode.SelectNodes("//li/div/span[@class='img_w_v8']/img");
					if (imageNodes == null || imageNodes.Count == 0)
						continue;

					//Content'i olusturmaya baslayalim
					var content = new Model.Content()
					{
						BlogId = blog.Id,
						SourcePath = brosurPath,
					};

					//bu resimlerin tamamini indirip sonrada uplaod etmeli ve metine eklemeliyiz
					StringBuilder sbContent = new StringBuilder();
					foreach (var imageNode in imageNodes)
					{
						var remote_media_path = imageNode.Attributes["src"].Value;
						var remote_file_extension = System.IO.Path.GetExtension(remote_media_path);

						var local_file_name_with_extension = post.Slug + remote_file_extension;
						var local_media_path = contentRootPath + "\\" + local_file_name_with_extension;

						DownloadRemoteImage(remote_media_path, local_media_path);

						var media = new Model.Media()
						{
							Alt = string.IsNullOrEmpty(remote_media.alt_text)
								? ""
								: post.MetaTitle,
							ImageSlug = Path.GetFileNameWithoutExtension(local_media_path),
							MediaUrl = "wp-content/uploads/" + local_file_name_with_extension,
							Title = post.PageTitle,
							Type = 1,
							BlogId = blog.Id,
						};

						var mediaInsertResult = mediaData.Insert(media);
						if (mediaInsertResult.IsSucceed)
						{
							post.FeaturedMediaId = media.Id;

							var postUpdateResult = contentData.Update(post);
						}
					}


					var customBotPostHistory = new Model.CustomBotPostHistory()
					{
						BlogId = blog.Id,
						CustomBotId = base._idInDb,
						CreateDate = System.DateTime.Now,
						Slug = hrefAttribute.Value
					};

					customBotPostHistoryData.Insert(customBotPostHistory);
				}
			}
		}

		public string GetHtml(string path)
		{
			using (var client = new WebClient())
			{
				return client.DownloadString(path);
			}
		}
	}
}
