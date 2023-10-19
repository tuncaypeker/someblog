using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Data.Keyword;
using SomeBlog.Infrastructure.Extensions;
using SomeBlog.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SomeBlog.Bots.SemrushUpdater
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var blogData = serviceProvider.GetService<BlogData>();
            var contentData = serviceProvider.GetService<ContentData>();
            var keywordData = serviceProvider.GetService<KeywordData>();
            var keywordSemrushData = serviceProvider.GetService<KeywordSemrushData>();
            var keywordRelatedData = serviceProvider.GetService<KeywordRelatedData>();
            var blogKeywordData = serviceProvider.GetService<BlogKeywordData>();
            var blogKeywordTrackData = serviceProvider.GetService<BlogKeywordTrackData>();
            var keywordSerpData = serviceProvider.GetService<KeywordSerpData>();
            var semrushService = new Integration.Semrush.Service(userId: 9913187, apiKey: "0f4d0ab99ff2f4984e74a0e43b4905da");
            var semrushSourceId = (int)SeoTools.Semrush;
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

            //amacimiz search query'leri guncellemek, moz update 6 ayda bir yapsak yeter
            while (true)
            {
                var blogs = blogData.GetAll();

                foreach (var blog in blogs)
                {
                    Console.Clear();
                    Console.Title = "Semrush:" + blog.Name + " ile çalışıyorum";

                    var blogKeywords = blogKeywordData.GetBy(x => x.BlogId == blog.Id);
                    var contents = contentData.GetBlogContentsAllIdSlug(blog.Id);

                    int count = 1;
                    var semrushOrganicPositionsResult = semrushService.GetOrganicPositions(blog.Name.ToLower(), count);
                    var semrushOrganicPositions = new List<Integration.Semrush.Dto.OrganicPositionsResult>();

                    while (semrushOrganicPositionsResult.result.Count > 0)
                    {
                        semrushOrganicPositions.AddRange(semrushOrganicPositionsResult.result);

                        count += 1;
                        if (count * 100 > semrushOrganicPositionsResult.Count)
                            break;

                        semrushOrganicPositionsResult = semrushService.GetOrganicPositions(blog.Name.ToLower(), count);
                    }

                    Console.Title = "Semrush:" + blog.Name + "=> " + semrushOrganicPositions.Count + " position";
                    int counter = 0;

                    foreach (var item in semrushOrganicPositions)
                    {
                        botProgramData.UpdatePulse("SomeBlog.Bots.SemrushUpdater");

                        counter += 1;

                        var itemDate = item.crawledTime.ToString().ToUnixTimeToDateTime();
                        var keywordInDb = keywordData.GetByQuery(item.phrase);
                        if (keywordInDb != null && (DateTime.Now - keywordInDb.SemrushUpdateDate).TotalDays < 7)
                            continue;

                        Console.WriteLine(counter + ":" + item.phrase + " ile çalışıyorum");
                        if (keywordInDb == null)
                        {
                            keywordInDb = new Model.Keyword()
                            {
                                CreateDate = DateTime.Now,
                                Query = item.phrase,
                                Source = semrushSourceId,
                            };

                            var dbKeywordInsertResult = keywordData.Insert(keywordInDb);
                            if (!dbKeywordInsertResult.IsSucceed)
                            {
                                Console.WriteLine("=> " + keywordInDb.Query + " insert edilirken hata oldu:" + dbKeywordInsertResult.Message);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("=> Keyword Insert Edildi");

                                //kelime yeni eklendi, semrush da ekleyelim
                                var keywordSemrushInDb = new Model.KeywordSemrush()
                                {
                                    CreateDate = DateTime.Now,
                                    Difficulty = item.keywordDifficulty,
                                    Volume = item.volume,
                                    //burada bir sürü bilgi var mutlaka bakilmali
                                };

                                var semrushInsertResult = keywordSemrushData.Insert(keywordSemrushInDb);
                            }
                        }
                        else//bu noktada keyword db;'de varmıs, peki keywordsemrush var mı
                        {
                            var dateNow = DateTime.Now.Date;
                            var keywordSemrushInDb = keywordSemrushData.GetByPage(x => x.KeywordId == keywordInDb.Id, 1, 1, "CreateDate", true).FirstOrDefault();
                            if (keywordSemrushInDb == null || keywordSemrushInDb.CreateDate.Date != dateNow)
                            {
                                //burada insert kesin    
                                keywordSemrushInDb = new Model.KeywordSemrush()
                                {
                                    CreateDate = DateTime.Now.Date,
                                    Difficulty = item.keywordDifficulty,
                                    KeywordId = keywordInDb.Id,
                                    Volume = item.volume
                                };
                                var dbKeywordSemrushInsertResult = keywordSemrushData.Insert(keywordSemrushInDb);
                            }
                        }

                        //blogkeyword var mi
                        var slugRegex = new Regex(blog.GetSlugRegex);
                        if (!slugRegex.IsMatch(item.url))
                        {
                            Console.WriteLine("=> " + item.url + " content ile uyumlu degil");
                            continue;
                        }

                        var slug = slugRegex.Match(item.url).Groups[1].Value;
                        var content = contents.FirstOrDefault(x => x.Slug == slug);
                        if (content == null)
                        {
                            Console.WriteLine("=> " + item.url + " content slug ile uyumlu degil");
                            continue;
                        }

                        var blogKeyword = blogKeywords.FirstOrDefault(x => x.BlogId == blog.Id && x.ContentId == content.Id && x.KeywordId == keywordInDb.Id);

                        if (blogKeyword == null)
                        {
                            blogKeyword = new Model.BlogKeyword()
                            {
                                BlogId = blog.Id,
                                ContentId = content.Id,
                                KeywordId = keywordInDb.Id,
                                LastPositionDInSC = -1,
                                LastPositionMInSC = -1,
                                LastUpdate = DateTime.Now,
                                PrevPositionDInSC = -1,
                                PrevPositionMInSC = -1,
                                Source = semrushSourceId
                            };

                            var blogKeywordInsertResult = blogKeywordData.Insert(blogKeyword);
                            if (!blogKeywordInsertResult.IsSucceed)
                            {
                                Console.WriteLine("=> " + keywordInDb.Query + " için blogKeyword insert edilirken hata oldu:" + blogKeywordInsertResult.Message);
                                continue;
                            }
                        }
                        else
                            Console.WriteLine("=> Blog Keyword Güncel Görünüyor..");

                        //position var mi
                        var itemDateStr = itemDate.Date.ToString("yyyy-MM-dd");
                        var blogKeywordTrack = blogKeywordTrackData.FirstOrDefault(x => x.BlogKeywordId == blogKeyword.Id && x.Date == itemDateStr && x.Device == "MOBILE" && x.Source == semrushSourceId);
                        if (blogKeywordTrack == null)
                        {
                            blogKeywordTrack = new Model.BlogKeywordTrack()
                            {
                                BlogKeywordId = blogKeyword.Id,
                                Clicks = -1,
                                Ctr = -1,
                                Date = itemDateStr,
                                Device = "MOBILE",
                                Impressions = -1,
                                Position = item.position,
                                Source = semrushSourceId
                            };
                            var blogKeywordTrackInsertResult = blogKeywordTrackData.Insert(blogKeywordTrack);
                            if (!blogKeywordTrackInsertResult.IsSucceed)
                            {
                                Console.WriteLine("=> " + keywordInDb.Query + " için blogKeywordTrack insert edilirken hata oldu:" + blogKeywordTrackInsertResult.Message);
                                continue;
                            }
                            else
                                Console.WriteLine("=> BlogKeywordTrack Eklendi");
                        }


                        //bu kelime ile ilgili daha fazla bilgi almaya calisalim
                        var keywordSummaryFromSemrush = semrushService.GetKeywordSummary(item.phrase);

                        var keywordRelateds = keywordSummaryFromSemrush.KeywordRelateds.result.Select(x => new Model.KeywordRelated()
                        {
                            Difficulty = x.difficulty,
                            KeywordId = keywordInDb.Id,
                            Query = x.phrase,
                            RelationLevel = x.relation_level,
                            Source = semrushSourceId,
                            Volume = x.volume
                        }).ToList();

                        var variations = keywordSummaryFromSemrush.KeywordVariations.result == null
                            ? new List<Model.KeywordRelated>()
                            : keywordSummaryFromSemrush.KeywordVariations.result.Select(x => new Model.KeywordRelated()
                            {
                                Difficulty = x.difficulty ?? 0,
                                KeywordId = keywordInDb.Id,
                                Query = x.phrase,
                                RelationLevel = -1,
                                Source = semrushSourceId,
                                Volume = x.volume
                            });

                        foreach (var variation in variations)
                        {
                            if (keywordRelateds.Any(x => x.Query == variation.Query))
                                continue;

                            keywordRelateds.Add(variation);
                        }

                        var relatedsInDb = keywordRelatedData.GetBy(x => x.KeywordId == keywordInDb.Id);
                        var insertBulkRelateds = new List<Model.KeywordRelated>();
                        foreach (var relatedFromSemrush in keywordRelateds)
                        {
                            var relatedInDb = relatedsInDb.FirstOrDefault(x => x.Query == relatedFromSemrush.Query);
                            if (relatedInDb == null)
                            {
                                insertBulkRelateds.Add(relatedFromSemrush);
                                continue;
                            }

                            if (relatedInDb.Volume != relatedFromSemrush.Volume || relatedInDb.Difficulty != relatedFromSemrush.Difficulty)
                            {
                                relatedInDb.Difficulty = relatedFromSemrush.Difficulty;
                                relatedInDb.Volume = relatedFromSemrush.Volume;

                                var updateResult = keywordRelatedData.Update(relatedInDb);
                            }
                        }

                        if (insertBulkRelateds.Count > 0)
                        {
                            var insertBulkResult = keywordRelatedData.InsertBulk(insertBulkRelateds);
                            Console.WriteLine(insertBulkResult.IsSucceed
                                ? $"=> {insertBulkRelateds.Count} related ekledim"
                                : $"=> related insert bulk hata oldu {insertBulkResult.Message}");
                        }

                        //serps
                        Console.WriteLine("Serps ile çalışıyorum...");
                        var keywordSerpsInDb = keywordSerpData.GetBy(x => x.KeywordId == keywordInDb.Id && x.Source == semrushSourceId);
                        var insertBulkSerpList = new List<Model.KeywordSerp>();
                        foreach (var serpFromSemrush in keywordSummaryFromSemrush.KeywordSerps.result)
                        {
                            var keywordSerpInDb = keywordSerpsInDb.FirstOrDefault(x => x.Rank == serpFromSemrush.position);
                            if (keywordSerpInDb == null)
                            {
                                insertBulkSerpList.Add(new Model.KeywordSerp()
                                {
                                    CreateDate = DateTime.Now,
                                    DomainAuthority = -1,
                                    KeywordId = keywordInDb.Id,
                                    LinkingDomainsToDomain = -1,
                                    LinkingDomainsToPage = -1,
                                    PageAuthority = -1,
                                    Rank = serpFromSemrush.position,
                                    Title = "",
                                    Type = "basic",
                                    Url = serpFromSemrush.url,
                                    Source = semrushSourceId
                                });

                                continue;
                            }

                            if (keywordSerpInDb.Url != serpFromSemrush.url)
                            {
                                keywordSerpInDb.CreateDate = DateTime.Now;
                                keywordSerpInDb.DomainAuthority = -1;
                                keywordSerpInDb.LinkingDomainsToDomain = -1;
                                keywordSerpInDb.LinkingDomainsToPage = -1;
                                keywordSerpInDb.PageAuthority = -1;
                                keywordSerpInDb.Title = "";
                                keywordSerpInDb.Type = "basic";
                                keywordSerpInDb.Url = serpFromSemrush.url;
                                keywordSerpInDb.Source = semrushSourceId;

                                var dbUpdateSerpResult = keywordSerpData.Update(keywordSerpInDb);
                            }
                        }

                        var insertBulkSerpResult = keywordSerpData.InsertBulk(insertBulkSerpList);
                        Console.WriteLine(insertBulkSerpResult.IsSucceed
                           ? $"=> {insertBulkSerpList.Count} serp ekledim"
                           : $"=> serp insert bulk hata oldu {insertBulkSerpResult.Message}");

                        //peki ya onceden rank'a eklenmis ama sonra yeni gelen listede olmayan varsa napcaz
                        //silebiliriz ya da 11 die sıralayabiliriz yanlis olur
                        //o yuzden silelim
                        var keywordSerpIdsNotExistNow = keywordSerpsInDb.Where(x => !keywordSummaryFromSemrush.KeywordSerps.result.Any(y => y.url == x.Url))
                            .Select(x => x.Id).ToArray();
                        if (keywordSerpIdsNotExistNow.Length > 0)
                        {

                        }

                        keywordSerpData.DeleteBulk(x => keywordSerpIdsNotExistNow.Contains(x.Id));

                        keywordSerpData.DetachAllEntities();
                        keywordRelatedData.DetachAllEntities();
                        blogKeywordTrackData.DetachAllEntities();

                        Console.Clear();
                    }
                }

                blogData.DetachAllEntities();
                contentData.DetachAllEntities();
                blogKeywordData.DetachAllEntities();
                keywordData.DetachAllEntities();
                keywordRelatedData.DetachAllEntities();
                keywordSerpData.DetachAllEntities();

                Console.Clear();
                Console.WriteLine("Tüm blogları gezdim, 23 saat beklyicem");
                System.Threading.Thread.Sleep(23 * 60 * 60 * 1000);
            }
        }
    }
}
