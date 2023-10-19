using Google.Apis.Blogger.v3.Data;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Pool;
using SomeBlog.Infrastructure.Extensions;
using SomeBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Bots.Pool.ContentUpdater
{
	class Program
	{
		static ServiceProvider serviceProvider;
		static Data.Pool.PoolBlogData poolBlogData;
		static Data.Pool.PoolTagData poolTagData;
		static Data.Pool.PoolCategoryData poolCategoryData;
		static Data.Pool.PoolContentData poolContentData;
		static Data.Pool.PoolContentCommentData poolContentCommentData;
		static Data.Pool.PoolContentTagData poolContentTagData;
		static Data.Pool.PoolContentCategoryData poolContentCategoryData;
		static Data.Pool.PoolMediaData poolMediaData;
		static Data.Pool.PoolLanguageData poolLanguageData;
		static Data.Pool.PoolBlogUpdateLogData poolBlogUpdateLogData;
		static Data.Pool.PoolBlogUpdateLogItemData poolBlogUpdateLogItemData;

		static void Main(string[] args)
		{
			string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
				//.AddJsonFile($"appsettings.{envName}.json", optional: true)
				.Build();

			Console.Title = "2021.09.17";

			serviceProvider = ServiceProviderHelper.BuildServiceProvider();

			poolBlogData = serviceProvider.GetService<Data.Pool.PoolBlogData>();
			poolTagData = serviceProvider.GetService<Data.Pool.PoolTagData>();
			poolCategoryData = serviceProvider.GetService<Data.Pool.PoolCategoryData>();
			poolContentData = serviceProvider.GetService<Data.Pool.PoolContentData>();
			poolContentCommentData = serviceProvider.GetService<Data.Pool.PoolContentCommentData>();
			poolContentTagData = serviceProvider.GetService<Data.Pool.PoolContentTagData>();
			poolContentCategoryData = serviceProvider.GetService<Data.Pool.PoolContentCategoryData>();
			poolMediaData = serviceProvider.GetService<Data.Pool.PoolMediaData>();
			poolLanguageData = serviceProvider.GetService<Data.Pool.PoolLanguageData>();
			poolBlogUpdateLogData = serviceProvider.GetService<Data.Pool.PoolBlogUpdateLogData>();
			poolBlogUpdateLogItemData = serviceProvider.GetService<Data.Pool.PoolBlogUpdateLogItemData>();
			var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

			//hic import edilmemis varsa al ve ilk import'u yap
			//hepsi ilk import yapilmissa en son update olanlara bak
			//Bugun update edilmemis var ise update et
			while (true)
			{
				botProgramData.UpdatePulse("SomeBlog.Bots.ContentPoolUpdater");

				var poolBlogForFirstInitialize = poolBlogData.FirstOrDefault(x => !x.HasFirstImportDone);
				if (poolBlogForFirstInitialize != null)
				{
					var log = new PoolBlogUpdateLog()
					{
						Description = $"{poolBlogForFirstInitialize.Name}'i ilk kez import ediyorum",
						EndDate = new DateTime(1970, 1, 1),
						IsFinished = false,
						PoolBlogId = poolBlogForFirstInitialize.Id,
						StartDate = DateTime.Now
					};
					List<PoolBlogUpdateLogItem> logItems = new List<PoolBlogUpdateLogItem>();

					Console.WriteLine(poolBlogForFirstInitialize.Name + " import ediyorum... [" + DateTime.Now + "]");
					Console.Title = poolBlogForFirstInitialize.Name;

					//ilk initizalize
					if (poolBlogForFirstInitialize.FeedType == 1) logItems = FirstInitializeWpJson(poolBlogForFirstInitialize);
					else if (poolBlogForFirstInitialize.FeedType == 2) logItems = FirstInitializFeedRdf(poolBlogForFirstInitialize);
					else if (poolBlogForFirstInitialize.FeedType == 3) logItems = FirstInitializeBlogspot(poolBlogForFirstInitialize);

					log.EndDate = DateTime.Now;
					log.IsFinished = true;

					var dbLogInsertResult = poolBlogUpdateLogData.Insert(log);

					if (dbLogInsertResult.IsSucceed)
					{
						logItems.ForEach(x => x.PoolBlogUpdateLogId = log.Id);
						poolBlogUpdateLogItemData.InsertBulk(logItems);
					}

				}
				else
				{
					var dateWeekBefore = DateTime.Now.AddDays(-7);
					var poolBlogNotUpdatedForOneWeek = poolBlogData.FirstOrDefault(x => x.LastUpdate < dateWeekBefore && x.ShouldInsertNewContents);
					if (poolBlogNotUpdatedForOneWeek == null)
					{
						Console.Clear();
						Console.WriteLine("Update edecek içerik bulamadim, [20] sn bekliycem [" + DateTime.Now + "]");

						//aslinda bu noktada hemen comment toplayalim bir site ya da bir içerik için
						GetCommentsForAContent(5);

						System.Threading.Thread.Sleep(20 * 1000);

						continue;
					}

					var log = new PoolBlogUpdateLog()
					{
						Description = $"{poolBlogNotUpdatedForOneWeek.Name}'i update ediyorum",
						EndDate = new DateTime(1970, 1, 1),
						IsFinished = false,
						PoolBlogId = poolBlogNotUpdatedForOneWeek.Id,
						StartDate = DateTime.Now
					};
					List<PoolBlogUpdateLogItem> logItems = new List<PoolBlogUpdateLogItem>();

					Console.WriteLine(poolBlogNotUpdatedForOneWeek.Name + " update ediyorum... [" + DateTime.Now + "]");

					poolBlogNotUpdatedForOneWeek.IgnoreTags = true;
					Console.WriteLine("Etiket almak ciddi zaman kaybettiriyor, bu yuzden gecici de olsa, etiket alma isini ignore ediyorum!!");

					//Yeni iceikler
					if (poolBlogNotUpdatedForOneWeek.FeedType == 1) logItems = GetNewContentsWpJson(poolBlogNotUpdatedForOneWeek);
					else if (poolBlogNotUpdatedForOneWeek.FeedType == 2) logItems = GetNewContentsFeedRdf(poolBlogNotUpdatedForOneWeek);
					else if (poolBlogNotUpdatedForOneWeek.FeedType == 3) logItems = GetNewContentsBlogspot(poolBlogNotUpdatedForOneWeek);

					log.EndDate = DateTime.Now;
					log.IsFinished = true;

					var dbLogInsertResult = poolBlogUpdateLogData.Insert(log);

					if (dbLogInsertResult.IsSucceed)
					{
						logItems.ForEach(x => x.PoolBlogUpdateLogId = log.Id);
						poolBlogUpdateLogItemData.InsertBulk(logItems);
					}
				}

				Console.Clear();
			}
		}

		private static void GetCommentsForAContent(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var poolContent = poolContentData.GetByPage(x => !x.HasCommentProcessed, 1, 1).FirstOrDefault();
				if (poolContent == null)
					continue;

				Console.WriteLine(poolContent.Id + " için yorumlari aliyorum...");

				var poolBlog = poolBlogData.GetByKey(poolContent.PoolBlogId);
				//acaba bu blog wpjson retired mi, bu durumda bu sitenin tum iceriklerinin commentprocessed olmasi lazim ki tekrar tekrar gezmeyelim
				if (poolBlog.IsWpJsonRetired || poolBlog.IsBlogRetired)
				{
					poolContentData.UpdateBulk(x => x.PoolBlogId == poolBlog.Id, new List<string>() { "HasCommentProcessed" }, new List<object>() { true });
					continue;
				}

				//bunun commentlerini alalim
				var _apiHelper = new Wordpress.WpJson.Helper(poolBlog.Path);
				var wp_comments = _apiHelper.GetComments(poolContent.SiteKeyId);
				if (wp_comments.Count == 0)
				{
					Console.WriteLine("\t yorum bulamadim, geciyorum...");

					poolContent.HasCommentProcessed = true;
					var dbUpdateX = poolContentData.Update(poolContent);
					continue;
				}

				Console.WriteLine($"\t {wp_comments.Count} yorum buldum, isliyorum...");

				//veritabanında mevcut olanlari alalim
				var poolContentCommentListInDb = poolContentCommentData.GetBy(x => x.PoolContentId == poolContent.Id);

				//eklemeye baslayalim
				var comment_list_insert_bulk = new List<Model.PoolContentComment>();
				foreach (var comment in wp_comments)
				{
					//bu comment zaten varsa ekleme
					if (poolContentCommentListInDb.Any(x => x.SiteKeyId == comment.id))
						continue;

					var parent_id = poolContentCommentListInDb.Where(x => x.SiteKeyId == comment.parent).FirstOrDefault() != null
						? poolContentCommentListInDb.Where(x => x.SiteKeyId == comment.parent).FirstOrDefault().Id
						: -1;

					var new_comment = new Model.PoolContentComment()
					{
						Created = comment.date,
						Fullname = comment.author_name,
						Text = comment.content.rendered,
						SiteKeyId = comment.id,
						ParentId = parent_id,
						PoolBlogId = poolBlog.Id,
						PoolContentId = poolContent.Id,
						SiteContentKeyId = comment.post,
					};

					comment_list_insert_bulk.Add(new_comment);
				}

				if (comment_list_insert_bulk.Count > 0)
				{
					var insert_bulk_last = poolContentCommentData.InsertBulk(comment_list_insert_bulk);
					if (insert_bulk_last.IsSucceed)
					{
						Console.WriteLine($"{poolBlog.Name} için son yorumlar eklendi.");
					}
				}

				poolContent.HasCommentProcessed = true;
				var dbUpdate = poolContentData.Update(poolContent);
			}
		}

		static List<PoolBlogUpdateLogItem> GetNewContentsWpJson(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();
			var _apiHelper = new Wordpress.WpJson.Helper(poolBlog.Path);

			Console.WriteLine("=> WpJson Guncel Postları Alıyorum");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, "wpjson guncel postlari alıyorum"));
			var remote_posts = _apiHelper.GetPosts(1, poolBlog.ApiPerPage);
			if (remote_posts == null || remote_posts.Count == 0)
			{
				Console.WriteLine("=> Postlar null ya da 0 geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, "Postlar null geldi"));
				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.Count} Post Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, $"{remote_posts.Count} Post Aldım"));

			//bu noktada artik elimizde alinmis post oldugunu soyleyebiliriz
			var local_tags = poolTagData.GetBy(x => x.PoolBlogId == poolBlog.Id);
			var local_categories = poolCategoryData.GetBy(x => x.PoolBlogId == poolBlog.Id);

			Console.WriteLine($"=> Kategorileri Alıyorum db'de:{local_categories.Count} tane");
			var remote_categories = _apiHelper.GetCategories();
			Console.WriteLine($"=> {remote_categories.Count} Kategori Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, $"{remote_categories.Count} Kategori Aldım"));

			Console.WriteLine($"=> Etiketleri Alıyorum db'de:{local_tags.Count} tane / Ignore Tags:{poolBlog.IgnoreTags}");
			var remote_tags = poolBlog.IgnoreTags
				? new List<Wordpress.WpJson.Model.Tag>()
				: _apiHelper.GetTags();

			Console.WriteLine($"=> {remote_tags.Count} Etiket Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, $"{remote_tags.Count} Etiket Aldım"));

			var category_filtered_ids = string.IsNullOrEmpty(poolBlog.CategoryFilter)
				? new string[0]
				: poolBlog.CategoryFilter.Split(',');

			Console.Title = "U:" + poolBlog.Name + "[Page:1] [WP-JSON]";

			int postCounter = 0;
			foreach (var p in remote_posts)
			{
				postCounter += 1;
				var remote_post = p;

				if (remote_post.content == null)
				{
					remote_post = _apiHelper.GetPostById(remote_post.id);

					if (remote_post == null || remote_post.content == null)
					{
						Console.WriteLine($"remote_post boş geldi");
						continue;
					}
					else
					{
						Console.WriteLine("Ozellikle post sordum..");
					}
				}

				if (category_filtered_ids.Length > 0 && !p.categories.Any(x => category_filtered_ids.Contains(x.ToString())))
				{
					Console.WriteLine($"{postCounter}: Kategori Filtreye uygun değil");
					continue;
				}

				if (remote_post.title == null || string.IsNullOrEmpty(remote_post.title.rendered))
				{
					Console.WriteLine($"{postCounter}: Title bos geldi");
					continue;
				}

				var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == remote_post.slug && x.PoolBlogId == poolBlog.Id);
				if (post_in_db != null)
				{
					Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
					continue;
				}

				if (remote_post.categories == null) remote_post.categories = new int?[0];
				if (remote_post.tags == null) remote_post.tags = new int[0];

				//TAG'LER
				var content_tags = new List<Model.PoolTag>();
				var new_tags_to_insert = new List<Model.PoolTag>();
				foreach (var t in remote_post.tags)
				{
					var remote_tag = remote_tags.FirstOrDefault(x => x.id == t);
					if (remote_tag == null)
						continue;

					var local_tag = local_tags.FirstOrDefault(x => x.Slug == remote_tag.slug);
					if (local_tag != null)
					{
						content_tags.Add(local_tag);
					}
					else
					{
						new_tags_to_insert.Add(new Model.PoolTag()
						{
							PoolBlogId = poolBlog.Id,
							Name = remote_tag.name ?? "",
							Slug = remote_tag.slug ?? "",
						});
					}
				}

				if (new_tags_to_insert.Count > 0)
				{
					var insertBulkTag = poolTagData.InsertBulk(new_tags_to_insert);
					if (insertBulkTag.IsSucceed)
					{
						content_tags.AddRange(new_tags_to_insert);
						local_tags.AddRange(new_tags_to_insert); //initial listeye ekleyelim ki tekrar tekrar insert olmasın
					}
					else
					{

					}
				}

				//KATEGORILER
				var content_categories = new List<Model.PoolCategory>();
				var new_categories_to_insert = new List<Model.PoolCategory>();
				foreach (var c in remote_post.categories)
				{
					var remote_category = remote_categories.FirstOrDefault(x => x.id == c);
					if (remote_category == null)
						continue;

					var local_category = local_categories.FirstOrDefault(x => x.Slug == remote_category.slug);
					if (local_category != null)
					{
						content_categories.Add(local_category);
					}
					else
					{
						new_categories_to_insert.Add(new Model.PoolCategory()
						{
							PoolBlogId = poolBlog.Id,
							Name = remote_category.name ?? "",
							Slug = remote_category.slug ?? "",
							ParentId = -1,
						});
					}
				}
				if (new_categories_to_insert.Count > 0)
				{
					var insertBulkCategory = poolCategoryData.InsertBulk(new_categories_to_insert);
					if (insertBulkCategory.IsSucceed)
					{
						content_categories.AddRange(new_categories_to_insert);
						local_categories.AddRange(new_categories_to_insert);
					}
					else
					{

					}
				}

				var seo_description = "";
				var seo_title = "";

				if (!string.IsNullOrEmpty(remote_post.yoast_head))
				{
					var split = remote_post.yoast_head.Split("\n", StringSplitOptions.None);
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

				var poolLanguage = poolLanguageData.FirstOrDefault(x => x.Id == poolBlog.PoolLanguageId);
				var post = new Model.PoolContent()
				{
					PoolBlogId = poolBlog.Id,
					CreateDate = DateTime.Now,
					Content = remote_post.content.rendered,
					Excerpt = remote_post.excerpt == null ? "" : remote_post.excerpt.rendered.ToStripHtml().ToTrim(445),
					Date = remote_post.date,
					UpdateDate = remote_post.modified,
					Slug = remote_post.slug,
					AuthorId = remote_post.author,
					Modified = remote_post.modified,
					SiteKeyId = remote_post.id,
					Status = remote_post.status,
					Title = remote_post.title.rendered,
					Type = remote_post.type,
					Link = remote_post.link,
					HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
				};

				var insertContent = poolContentData.Insert(post);
				if (!insertContent.IsSucceed)
				{
					Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");
					continue;
				}

				Console.WriteLine($"{postCounter}: + {post.Title}");

				var contentTagsInsertResult = poolContentTagData.InsertBulk(content_tags.Select(x => new Model.PoolContentTag(contentId: post.Id, tagId: x.Id)).ToList());
				var contentcategoriesInsertResult = poolContentCategoryData.InsertBulk(content_categories.Select(x => new Model.PoolContentCategory(contentId: post.Id, categoryId: x.Id)).ToList());

				//media'lari db'ye alalim
				var mediasToInsert = new List<Model.PoolMedia>();
				if (!string.IsNullOrEmpty(remote_post.featured_media) || remote_post.featured_media != "0")
				{
					try
					{
						var media_remote = _apiHelper.GetMedia(remote_post.featured_media);
						if (media_remote != null)
						{
							var remote_media_path = media_remote.guid.rendered;

							/*
                            var local_media_path = remote_media_path.Replace(poolBlog.Path, "").TrimStart('/');
                            Directory.CreateDirectory(Path.GetDirectoryName(local_media_path));

                            using (var client = new WebClient())
                            {
                                client.DownloadFile(remote_media_path, local_media_path);
                            }
                            */

							var media = new Model.PoolMedia()
							{
								Alt = media_remote.alt_text ?? post.Title,
								Caption = media_remote.caption.rendered ?? "",
								Description = media_remote.description.rendered ?? "",
								PoolContentId = post.Id,
								RemotePath = remote_media_path,
								HasFeatured = true
							};

							mediasToInsert.Add(media);
						}
					}
					catch (Exception ex)
					{

					}
				}

				mediasToInsert.AddRange(ParseMediasFromHtmlContent(post.Content, post.Id));

				poolMediaData.InsertBulk(mediasToInsert);
			}

			poolBlog.LastUpdate = DateTime.Now;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<PoolBlogUpdateLogItem> GetNewContentsFeedRdf(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();
			var _apiHelper = new Wordpress.Feed.Rdf.Service(poolBlog.Path);

			Console.WriteLine("=> Feed Guncel Postları Alıyorum [RDF] [" + DateTime.Now + "]");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, "feed guncel postlari alıyorum"));
			var remote_posts = _apiHelper.GetPosts();
			if (remote_posts == null || remote_posts.Count == 0)
			{
				Console.WriteLine("=> Postlar null ya da 0 geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, remote_posts == null ? "Postlar null geldi geçiyorum" : "Postlar 0 tane geldi geçiyorum"));
				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.Count} Post Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, $"{remote_posts.Count} Post Aldım"));

			//bu noktada artik elimizde alinmis post oldugunu soyleyebiliriz
			var local_categories = poolCategoryData.GetBy(x => x.PoolBlogId == poolBlog.Id);

			Console.Title = "U:" + poolBlog.Name + "[Page:1]  [FEED-RDF]";

			int postCounter = 0;
			foreach (var p in remote_posts)
			{
				postCounter += 1;
				var remote_post = p;
				var slug = p.Link.Replace(poolBlog.Path, "").TrimEnd('/');

				var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == slug && x.PoolBlogId == poolBlog.Id);
				if (post_in_db != null)
				{
					Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
					continue;
				}

				if (remote_post.Categories == null) remote_post.Categories = new List<string>();


				//KATEGORILER
				var content_categories = new List<Model.PoolCategory>();
				var new_categories_to_insert = new List<Model.PoolCategory>();
				foreach (var category_slug in remote_post.Categories)
				{
					var local_category = local_categories.FirstOrDefault(x => x.Slug == category_slug);
					if (local_category != null)
					{
						content_categories.Add(local_category);
					}
					else
					{
						new_categories_to_insert.Add(new Model.PoolCategory()
						{
							PoolBlogId = poolBlog.Id,
							Name = category_slug ?? "",
							Slug = category_slug ?? "",
							ParentId = -1,
						});
					}
				}
				if (new_categories_to_insert.Count > 0)
				{
					var insertBulkCategory = poolCategoryData.InsertBulk(new_categories_to_insert);
					if (insertBulkCategory.IsSucceed)
					{
						content_categories.AddRange(new_categories_to_insert);
						local_categories.AddRange(new_categories_to_insert);
					}
					else
					{

					}
				}


				var post = new Model.PoolContent()
				{
					PoolBlogId = poolBlog.Id,
					CreateDate = DateTime.Now,
					Content = remote_post.Content,
					Excerpt = remote_post.Summary.ToStripHtml().ToTrim(445),
					Date = remote_post.PublishDate,
					UpdateDate = remote_post.PublishDate,
					Slug = slug,
					AuthorId = -1,
					PoolLanguageId = poolBlog.PoolLanguageId,
					Modified = remote_post.PublishDate,
					SiteKeyId = remote_post.Id.ToString(),
					Status = "pubşlish",
					Title = remote_post.Title,
					Type = "post",
					Link = remote_post.Link,
					HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
				};

				var insertContent = poolContentData.Insert(post);
				if (!insertContent.IsSucceed)
				{
					Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");

					continue;
				}

				var mediasToInsert = ParseMediasFromHtmlContent(post.Content, post.Id);

				poolMediaData.InsertBulk(mediasToInsert);

				Console.WriteLine($"{postCounter}: + {post.Title}");

				var contentcategoriesInsertResult = poolContentCategoryData.InsertBulk(content_categories.Select(x => new Model.PoolContentCategory(contentId: post.Id, categoryId: x.Id)).ToList());
			}

			poolBlog.LastUpdate = DateTime.Now;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<PoolBlogUpdateLogItem> FirstInitializeWpJson(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();

			//en son nerede kaldıysak oradan başlayalım, 4000 içerikten sonra hata oluyor
			//4000 içerik tekrar çekilmeye çalışılıyor
			int counter = poolBlog.FirstImportLastPage + 1;
			var _apiHelper = new Wordpress.WpJson.Helper(poolBlog.Path);

			Console.WriteLine("=> WpJson Ilk Postları Alıyorum");
			var remote_posts = _apiHelper.GetPosts(counter, poolBlog.ApiPerPage);

			if (remote_posts == null || remote_posts.Count == 0)
			{
				Console.WriteLine("=> Postlar null geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, remote_posts == null ? $"Postlar null geldi geçiyorum" : "Postlar 0 tane geldi geçiyorum"));

				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.ImportDate = DateTime.Now;
				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.Count} Post Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"{remote_posts.Count} Post Aldım"));

			//bu noktada artik elimizde alinmis post oldugunu soyleyebiliriz
			Console.WriteLine("=> Kategorileri Alıyorum");
			var remote_categories = _apiHelper.GetCategories();
			Console.WriteLine($"=> {remote_categories.Count} Kategori Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"{remote_categories.Count} Kategori Aldım"));

			Console.WriteLine($"=> Etiketleri Alıyorum, Ignore Tags:{poolBlog.IgnoreTags}");
			var remote_tags = poolBlog.IgnoreTags
				? new List<Wordpress.WpJson.Model.Tag>()
				: _apiHelper.GetTags();
			Console.WriteLine($"=> {remote_tags.Count} Etiket Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"{remote_tags.Count} Etiket Aldım"));

			var local_tags = poolTagData.GetBy(x => x.PoolBlogId == poolBlog.Id);
			var local_categories = poolCategoryData.GetBy(x => x.PoolBlogId == poolBlog.Id);

			var category_filtered_ids = string.IsNullOrEmpty(poolBlog.CategoryFilter)
				? new string[0]
				: poolBlog.CategoryFilter.Split(',');

			while (remote_posts != null)
			{
				Console.Title = "I:" + poolBlog.Name + "[Page:" + counter + "]" + "[MaxPage:" + poolBlog.MaxImportLastPage + "]  [WP-JSON]";

				int postCounter = 0;
				foreach (var p in remote_posts)
				{
					postCounter += 1;
					var remote_post = p;

					if (remote_post.content == null)
					{
						remote_post = _apiHelper.GetPostById(remote_post.id);

						if (remote_post == null || remote_post.content == null)
						{
							Console.WriteLine($"remote_post boş geldi");
							continue;
						}
						else
							Console.WriteLine("Ozellikle post sordum..");
					}

					if (category_filtered_ids.Length > 0 && !p.categories.Any(x => category_filtered_ids.Contains(x.ToString())))
					{
						Console.WriteLine($"{postCounter}: Kategori Filtreye uygun değil");
						continue;
					}

					if (remote_post.title == null || string.IsNullOrEmpty(remote_post.title.rendered))
					{
						Console.WriteLine($"{postCounter}: Title bos geldi");
						continue;
					}

					var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == remote_post.slug && x.PoolBlogId == poolBlog.Id);
					if (post_in_db != null)
					{
						Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
						continue;
					}

					if (remote_post.categories == null) remote_post.categories = new int?[0];
					if (remote_post.tags == null) remote_post.tags = new int[0];

					//TAG'LER
					var content_tags = new List<Model.PoolTag>();
					var new_tags_to_insert = new List<Model.PoolTag>();
					foreach (var t in remote_post.tags)
					{
						var remote_tag = remote_tags.FirstOrDefault(x => x.id == t);
						if (remote_tag == null)
							continue;

						var local_tag = local_tags.FirstOrDefault(x => x.Slug == remote_tag.slug);
						if (local_tag != null)
							content_tags.Add(local_tag);
						else
						{
							new_tags_to_insert.Add(new Model.PoolTag()
							{
								PoolBlogId = poolBlog.Id,
								Name = remote_tag.name ?? "",
								Slug = remote_tag.slug ?? "",
							});
						}
					}

					if (new_tags_to_insert.Count > 0)
					{
						var insertBulkTag = poolTagData.InsertBulk(new_tags_to_insert);
						if (insertBulkTag.IsSucceed)
						{
							content_tags.AddRange(new_tags_to_insert);
							local_tags.AddRange(new_tags_to_insert); //initial listeye ekleyelim ki tekrar tekrar insert olmasın
						}
						else
						{

						}
					}

					//KATEGORILER
					var content_categories = new List<Model.PoolCategory>();
					var new_categories_to_insert = new List<Model.PoolCategory>();
					foreach (var c in remote_post.categories)
					{
						var remote_category = remote_categories.FirstOrDefault(x => x.id == c);
						if (remote_category == null)
							continue;

						var local_category = local_categories.FirstOrDefault(x => x.Slug == remote_category.slug);
						if (local_category != null)
						{
							content_categories.Add(local_category);
						}
						else
						{
							new_categories_to_insert.Add(new Model.PoolCategory()
							{
								PoolBlogId = poolBlog.Id,
								Name = remote_category.name ?? "",
								Slug = remote_category.slug ?? "",
								ParentId = -1,
							});
						}
					}
					if (new_categories_to_insert.Count > 0)
					{
						var insertBulkCategory = poolCategoryData.InsertBulk(new_categories_to_insert);
						if (insertBulkCategory.IsSucceed)
						{
							content_categories.AddRange(new_categories_to_insert);
							local_categories.AddRange(new_categories_to_insert);
						}
						else
						{

						}
					}

					var seo_description = "";
					var seo_title = "";

					if (!string.IsNullOrEmpty(remote_post.yoast_head))
					{
						var split = remote_post.yoast_head.Split("\n", StringSplitOptions.None);
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

					var post = new Model.PoolContent()
					{
						PoolBlogId = poolBlog.Id,
						CreateDate = DateTime.Now,
						Content = remote_post.content.rendered,
						Excerpt = remote_post.excerpt == null
							? ""
							: remote_post.excerpt.rendered.ToStripHtml().ToTrim(445),
						Date = remote_post.date,
						UpdateDate = remote_post.modified,
						Slug = remote_post.slug,
						AuthorId = remote_post.author,
						PoolLanguageId = poolBlog.PoolLanguageId,
						Modified = remote_post.modified,
						SiteKeyId = remote_post.id,
						Status = remote_post.status,
						Title = remote_post.title.rendered,
						Type = remote_post.type,
						Link = remote_post.link,
						HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
					};

					var insertContent = poolContentData.Insert(post);
					if (!insertContent.IsSucceed)
					{
						Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");
						continue;
					}

					Console.WriteLine($"{postCounter}: + {post.Title}");

					var contentTagsInsertResult = poolContentTagData.InsertBulk(content_tags.Select(x => new Model.PoolContentTag(contentId: post.Id, tagId: x.Id)).ToList());
					var contentcategoriesInsertResult = poolContentCategoryData.InsertBulk(content_categories.Select(x => new Model.PoolContentCategory(contentId: post.Id, categoryId: x.Id)).ToList());

					//feature media indir, content ici medialari poolmedia' tablsouna al
					var mediasToInsert = new List<PoolMedia>();
					if (!string.IsNullOrEmpty(remote_post.featured_media) && remote_post.featured_media != "0")
					{
						try
						{
							var media_remote = _apiHelper.GetMedia(remote_post.featured_media);
							if (media_remote != null)
							{
								var remote_media_path = media_remote.guid.rendered;

								/*
                                var local_media_path = remote_media_path.Replace(poolBlog.Path, "").TrimStart('/');
                                Directory.CreateDirectory(Path.GetDirectoryName(local_media_path));

                                using (var client = new WebClient())
                                {
                                    client.DownloadFile(remote_media_path, local_media_path);
                                }
                                */

								var media = new Model.PoolMedia()
								{
									Alt = media_remote.alt_text ?? post.Title,
									Caption = media_remote.caption.rendered ?? "",
									Description = media_remote.description.rendered ?? "",
									PoolContentId = post.Id,
									RemotePath = remote_media_path,
									HasFeatured = true
								};

								mediasToInsert.Add(media);
							}
						}
						catch (Exception ex)
						{

						}
					}

					//yukarida feature once ekleniyor
					mediasToInsert.AddRange(ParseMediasFromHtmlContent(post.Content, post.Id));

					poolMediaData.InsertBulk(mediasToInsert);
				}

				//bu noktada poolblog ile ilgili bir update olabilir, firstimport kesilmiş olabilir web arayüzünden
				poolBlog = poolBlogData.GetByKey(poolBlog.Id);

				counter += 1;
				poolBlog.FirstImportLastPage = counter;
				var lastPageUpdateResult = poolBlogData.Update(poolBlog);
				if (!lastPageUpdateResult.IsSucceed)
				{
					Console.WriteLine("**");
					Console.WriteLine("**");
					Console.WriteLine("**");
					Console.WriteLine("Last Page guncelleyemedim");
					Console.WriteLine("**");
					Console.WriteLine("**");
					Console.WriteLine("**");
				}

				Console.Title = "I:" + poolBlog.Name + "[Page:" + counter + "]" + "[MaxPage:" + poolBlog.MaxImportLastPage + "]  [WP-JSON]";

				if (poolBlog.HasFirstImportDone)
				{
					poolBlog.ImportDate = DateTime.Now;
					poolBlog.LastUpdate = DateTime.Now;
					poolBlog.HasFirstImportDone = true;

					var poolBlogUpdateResultIm = poolBlogData.Update(poolBlog);

					return logItems;
				}

				//max page'e ulastik
				if (counter >= poolBlog.MaxImportLastPage)
				{
					poolBlog.ImportDate = DateTime.Now;
					poolBlog.LastUpdate = DateTime.Now;
					poolBlog.HasFirstImportDone = true;

					var poolBlogUpdateResultIm = poolBlogData.Update(poolBlog);

					return logItems;
				}

				var firstPostIdOfPreviousCall = remote_posts.FirstOrDefault().id;
				remote_posts = _apiHelper.GetPosts(counter, poolBlog.ApiPerPage);

				//kimi sitelerde belli bir sayfalamanin ustune hep ayni post'lari doner, bu da sonsuz donguye sebep olur.
				//asagidaki satir bunu engellemek icin eklendi
				if (remote_posts != null && remote_posts.Count > 0 && remote_posts.FirstOrDefault().id == firstPostIdOfPreviousCall)
					break;
			}

			poolBlog.ImportDate = DateTime.Now;
			poolBlog.LastUpdate = DateTime.Now;
			poolBlog.HasFirstImportDone = true;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<PoolBlogUpdateLogItem> FirstInitializFeedRdf(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();

			//en son nerede kaldıysak oradan başlayalım, 4000 içerikten sonra hata oluyor
			//4000 içerik tekrar çekilmeye çalışılıyor
			var _apiHelper = new Wordpress.Feed.Rdf.Service(poolBlog.Path);

			Console.WriteLine("=> Feed Ilk Postları Alıyorum [" + DateTime.Now + "]");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $" feed ilk Postları Alıyorum"));
			var remote_posts = _apiHelper.GetPosts();
			if (remote_posts == null)
			{
				Console.WriteLine("=> Postlar null geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"Postlar null geldi, geçiyorum"));
				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.ImportDate = DateTime.Now;
				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.Count} Post Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"{remote_posts.Count} Post Aldım"));

			//bu noktada artik elimizde alinmis post oldugunu soyleyebiliriz
			//KATEGORILER
			var local_categories = poolCategoryData.GetBy(x => x.PoolBlogId == poolBlog.Id);

			int postCounter = 0;
			foreach (var p in remote_posts)
			{
				postCounter += 1;
				var remote_post = p;
				var slug = p.Link.Replace(poolBlog.Path, "").TrimEnd('/');

				var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == slug && x.PoolBlogId == poolBlog.Id);
				if (post_in_db != null)
				{
					Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
					continue;
				}

				if (remote_post.Categories == null) remote_post.Categories = new List<string>();
				var content_categories = new List<Model.PoolCategory>();
				var new_categories_to_insert = new List<Model.PoolCategory>();
				foreach (var category_slug in remote_post.Categories)
				{
					var local_category = local_categories.FirstOrDefault(x => x.Slug == category_slug);
					if (local_category != null)
						content_categories.Add(local_category);
					else
					{
						new_categories_to_insert.Add(new Model.PoolCategory()
						{
							PoolBlogId = poolBlog.Id,
							Name = category_slug ?? "",
							Slug = category_slug ?? "",
							ParentId = -1,
						});
					}
				}
				if (new_categories_to_insert.Count > 0)
				{
					var insertBulkCategory = poolCategoryData.InsertBulk(new_categories_to_insert);
					if (insertBulkCategory.IsSucceed)
					{
						content_categories.AddRange(new_categories_to_insert);
						local_categories.AddRange(new_categories_to_insert);
					}
					else
					{

					}
				}

				var post = new Model.PoolContent()
				{
					PoolBlogId = poolBlog.Id,
					CreateDate = DateTime.Now,
					Content = remote_post.Content,
					Excerpt = remote_post.Summary.ToStripHtml().ToTrim(445),
					Date = remote_post.PublishDate,
					UpdateDate = remote_post.PublishDate,
					Slug = remote_post.Link.Replace(poolBlog.Path, "").TrimEnd('/'),
					AuthorId = -1,
					PoolLanguageId = poolBlog.PoolLanguageId,
					Modified = remote_post.PublishDate,
					SiteKeyId = remote_post.Id.ToString(),
					Status = "publish",
					Title = remote_post.Title,
					Type = "post",
					Link = remote_post.Link,
					HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
				};

				var insertContent = poolContentData.Insert(post);
				if (!insertContent.IsSucceed)
				{
					Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");
					continue;
				}

				//feature media indir, content ici medialari poolmedia' tablsouna al
				var mediasToInsert = ParseMediasFromHtmlContent(post.Content, post.Id);
				poolMediaData.InsertBulk(mediasToInsert);

				Console.WriteLine($"{postCounter}: + {post.Title}");

				var contentcategoriesInsertResult = poolContentCategoryData.InsertBulk(content_categories.Select(x => new Model.PoolContentCategory(contentId: post.Id, categoryId: x.Id)).ToList());
			}

			poolBlog.ImportDate = DateTime.Now;
			poolBlog.LastUpdate = DateTime.Now;
			poolBlog.HasFirstImportDone = true;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<PoolBlogUpdateLogItem> FirstInitializeBlogspot(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();

			var _apiHelper = new SomeBlog.Blogspot.Api.Service();

			Console.WriteLine("=> Blogspot Ilk Postları Alıyorum");
			var remote_posts = _apiHelper.GetPosts(poolBlog.BlogId);
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, " blogspot ilk postlari alıyorum"));

			if (remote_posts == null)
			{
				Console.WriteLine("=> Postlar null geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"Postlar null geldi geçiyorum"));

				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.ImportDate = DateTime.Now;
				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.posts.Count} POst Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(DateTime.Now, $"{remote_posts.posts.Count} Post Aldım"));

			Console.Title = "I:" + poolBlog.Name + "[MaxPage:" + poolBlog.MaxImportLastPage + "]  [BLOGSPOT]";

			int postCounter = 0;
			foreach (var remote_post in remote_posts.posts)
			{
				postCounter += 1;

				if (remote_post.title == null || string.IsNullOrEmpty(remote_post.title))
				{
					Console.WriteLine($"{postCounter}: Title bos geldi");
					continue;
				}

				var slug = System.IO.Path.GetFileNameWithoutExtension(remote_post.url);

				var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == slug && x.PoolBlogId == poolBlog.Id);
				if (post_in_db != null)
				{
					Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
					continue;
				}

				//TAG'LER
				//tag yok

				//KATEGORILER
				//kategori yok

				//aq bunu int yapmisiz, string yaptim mysql dayanamior patlior
				//blogspot'ta da jhayvan gibi uzun id, artik oldugu kadar
				int authorId = -1;
				if (!int.TryParse(remote_post.author.id, out authorId))
					authorId = -1;

				var post = new Model.PoolContent()
				{
					PoolBlogId = poolBlog.Id,
					CreateDate = DateTime.Now,
					Content = remote_post.content,
					Excerpt = remote_post.content.ToStripHtml().ToTrim(445),
					Date = remote_post.published,
					UpdateDate = DateTime.Now,
					Slug = slug,
					AuthorId = authorId,
					PoolLanguageId = poolBlog.PoolLanguageId,
					Modified = remote_post.updated,
					SiteKeyId = remote_post.id,
					Status = "publish",
					Title = remote_post.title,
					Type = "post",
					Link = remote_post.url,
					HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
				};

				var insertContent = poolContentData.Insert(post);
				if (!insertContent.IsSucceed)
				{
					Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");
					continue;
				}

				Console.WriteLine($"{postCounter}: + {post.Title}");

				//feature media indir, content ici medialari poolmedia' tablsouna al
				var mediasToInsert = ParseMediasFromHtmlContent(post.Content, post.Id);
				poolMediaData.InsertBulk(mediasToInsert);
			}

			//bu noktada poolblog ile ilgili bir update olabilir, firstimport kesilmiş olabilir web arayüzünden
			poolBlog = poolBlogData.GetByKey(poolBlog.Id);

			poolBlog.FirstImportLastPage = 1;
			var lastPageUpdateResult = poolBlogData.Update(poolBlog);
			if (!lastPageUpdateResult.IsSucceed)
			{
				Console.WriteLine("**");
				Console.WriteLine("**");
				Console.WriteLine("**");
				Console.WriteLine("Last Page guncelleyemedim");
				Console.WriteLine("**");
				Console.WriteLine("**");
				Console.WriteLine("**");
			}

			poolBlog.ImportDate = DateTime.Now;
			poolBlog.LastUpdate = DateTime.Now;
			poolBlog.HasFirstImportDone = true;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<PoolBlogUpdateLogItem> GetNewContentsBlogspot(PoolBlog poolBlog)
		{
			var logItems = new List<PoolBlogUpdateLogItem>();
			var _apiHelper = new SomeBlog.Blogspot.Api.Service();

			Console.WriteLine("=> Blogspot guncel Postları Alıyorum");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, "Blogspot guncel postlari alıyorum"));
			var remote_posts = _apiHelper.GetPosts(poolBlog.BlogId);
			if (remote_posts == null)
			{
				Console.WriteLine("=> Postlar null geldi, geçiyorum");
				logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, "Postlar null geldi"));
				System.Threading.Thread.Sleep(10 * 1000);

				poolBlog.LastUpdate = DateTime.Now;
				poolBlog.HasFirstImportDone = true;

				var poolBlogUpdateResult1 = poolBlogData.Update(poolBlog);

				return logItems;
			}
			Console.WriteLine($"=> {remote_posts.posts.Count} Post Aldım");
			logItems.Add(new PoolBlogUpdateLogItem(System.DateTime.Now, $"{remote_posts.posts.Count} Post Aldım"));

			var category_filtered_ids = string.IsNullOrEmpty(poolBlog.CategoryFilter)
				? new string[0]
				: poolBlog.CategoryFilter.Split(',');

			Console.Title = "U:" + poolBlog.Name + "[Page:1] [BLOGSPOT]";

			int postCounter = 0;
			foreach (var remote_post in remote_posts.posts)
			{
				postCounter += 1;
				if (remote_post.title == null || string.IsNullOrEmpty(remote_post.title))
				{
					Console.WriteLine($"{postCounter}: Title bos geldi");
					continue;
				}

				var slug = System.IO.Path.GetFileNameWithoutExtension(remote_post.url);
				var post_in_db = poolContentData.FirstOrDefault(x => x.Slug == slug && x.PoolBlogId == poolBlog.Id);
				if (post_in_db != null)
				{
					Console.WriteLine($"{postCounter}:  x {post_in_db.Title} => var atladim");
					continue;
				}

				//TAG'LER
				//tag yok


				//KATEGORILER
				//kategori yok

				int authorId = -1;
				if (!int.TryParse(remote_post.author.id, out authorId))
					authorId = -1;

				var poolLanguage = poolLanguageData.FirstOrDefault(x => x.Id == poolBlog.PoolLanguageId);
				var post = new Model.PoolContent()
				{
					PoolBlogId = poolBlog.Id,
					CreateDate = DateTime.Now,
					Content = remote_post.content,
					Excerpt = remote_post.content.ToStripHtml().ToTrim(445),
					Date = remote_post.published,
					UpdateDate = DateTime.Now,
					Slug = slug,
					AuthorId = authorId,
					Modified = remote_post.updated,
					SiteKeyId = remote_post.id,
					Status = "published",
					Title = remote_post.title,
					Type = "post",
					Link = remote_post.url,
					HasMediaProcessed = true //asagida process edilecek, true eklemekte sakinca yok
				};

				var insertContent = poolContentData.Insert(post);
				if (!insertContent.IsSucceed)
				{
					Console.WriteLine($"{postCounter}: - {post.Title} eklenirken hata {insertContent.Message}");
					continue;
				}

				Console.WriteLine($"{postCounter}: + {post.Title}");

				//media'lari db'ye alalim
				var mediasToInsert = ParseMediasFromHtmlContent(post.Content, post.Id);

				poolMediaData.InsertBulk(mediasToInsert);
			}

			poolBlog.LastUpdate = DateTime.Now;

			var poolBlogUpdateResult = poolBlogData.Update(poolBlog);

			return logItems;
		}

		static List<Model.PoolMedia> ParseMediasFromHtmlContent(string content, int postId)
		{
			var mediasToInsert = new List<PoolMedia>();

			HtmlDocument htmldoc = new HtmlDocument();
			htmldoc.LoadHtml(content);

			var imageNodes = htmldoc.DocumentNode.SelectNodes("//img//@src");
			if (imageNodes != null && imageNodes.Count > 0)
			{
				foreach (HtmlNode imageNode in imageNodes)
				{
					var alt = imageNode.GetAttributeValue("alt", "");
					var title = imageNode.GetAttributeValue("title", "");
					var remote_media_path = imageNode.GetAttributeValue("src", "");
					if (string.IsNullOrEmpty(remote_media_path))
						remote_media_path = imageNode.GetAttributeValue("data-src", "");

					if (string.IsNullOrEmpty(remote_media_path))
						continue;

					mediasToInsert.Add(new Model.PoolMedia()
					{
						Alt = alt,
						Caption = title,
						Description = "",
						HasFeatured = false,
						PoolContentId = postId,
						RemotePath = remote_media_path,
					});
				}
			}

			return mediasToInsert;
		}
	}
}