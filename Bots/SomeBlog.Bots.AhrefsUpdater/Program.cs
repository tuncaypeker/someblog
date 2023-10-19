using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Content;
using SomeBlog.Data.Keyword;
using SomeBlog.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SomeBlog.Bots.AhrefsUpdater
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var blogData = serviceProvider.GetService<BlogData>();
            var ahrefsAccountData = serviceProvider.GetService<AhrefsAccountData>();
            var contentData = serviceProvider.GetService<ContentData>();
            var keywordData = serviceProvider.GetService<KeywordData>();
            var keywordAhrefsData = serviceProvider.GetService<KeywordAhrefsData>();
            var blogKeywordData = serviceProvider.GetService<BlogKeywordData>();
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();
            var blogKeywordTrackData = serviceProvider.GetService<BlogKeywordTrackData>();
            var ahrefsSourceId = (int)SeoTools.Ahrefs;

            while (true)
            {
                var blogs = blogData.GetAll();
                foreach (var blog in blogs)
                {
                    if (blog.AhrefsAccountId == -1)
                    {
                        Console.WriteLine($"{blog} Ahref Account Id: false");
                        continue;
                    }

                    var ahrefsAccount = ahrefsAccountData.GetByKey(blog.AhrefsAccountId);
                    if (ahrefsAccount == null)
                    {
                        Console.WriteLine($"{blog} Ahref Account Id bulamadim....");
                        continue;
                    }
                    if (string.IsNullOrEmpty(ahrefsAccount.Cookie))
                    {
                        Console.WriteLine($"{blog} Ahref Account cookie kayitli degil....");
                        continue;
                    }

                    var ahrefService = new Integration.Ahrefs.Service(ahrefsAccount.Cookie);

                    Console.Clear();
                    Console.Title = "Ahrefs:" + blog.Name + " ile çalışıyorum";

                    int page = 1;

                    var organicPositionsResult = ahrefService.GetOrganicKeywords(blog.Name.ToLower(), page, 50);
                    if (!organicPositionsResult.IsSucceed)
                    {
                        Console.WriteLine($"{blog.Name}: false, {organicPositionsResult.Message}");
                        continue;
                    }

                    var organicPositions = new List<Integration.Ahrefs.Dto.GetOrganicKeywordRowDto>();

                    while (organicPositionsResult.Rows.Count > 0)
                    {
                        if (page > 50)
                            break;

                        if (organicPositionsResult.Rows.Count == 0)
                            break;

                        organicPositions.AddRange(organicPositionsResult.Rows);

                        page += 1;

                        organicPositionsResult = ahrefService.GetOrganicKeywords(blog.Name.ToLower(), page, 50);

                        System.Threading.Thread.Sleep(5 * 1000);
                    }

                    Console.Title = "Ahrefs:" + blog.Name + "=> " + organicPositions.Count + " position";

                    var blogKeywords = blogKeywordData.GetBy(x => x.BlogId == blog.Id);
                    var contents = contentData.GetBlogContentsAllIdSlug(blog.Id);

                    foreach (var item in organicPositions)
                    {
                        botProgramData.UpdatePulse("SomeBlog.Bots.AhrefsUpdater");

                        var itemDate = item.LastUpdate;
                        var keywordInDb = keywordData.GetByQuery(item.Keyword);
                        if (keywordInDb != null && (DateTime.Now - keywordInDb.AhrefsUpdateDate).TotalDays < 7)
                        {
                            Console.WriteLine(keywordInDb.Query + " guncel gorunuyor");
                            continue;
                        }

                        Console.WriteLine(item.Keyword + " ile çalışıyorum");
                        if (keywordInDb == null)
                        {
                            keywordInDb = new Model.Keyword()
                            {
                                CreateDate = DateTime.Now,
                                Query = item.Keyword,
                                Source = ahrefsSourceId,
                                AhrefsUpdateDate = DateTime.Now,
                                Volume = item.Volume,
                                VolumeSource = ahrefsSourceId
                            };

                            var dbKeywordInsertResult = keywordData.Insert(keywordInDb);
                            if (!dbKeywordInsertResult.IsSucceed)
                            {
                                Console.WriteLine("=> " + keywordInDb.Query + " insert edilirken hata oldu:" + dbKeywordInsertResult.Message);
                                continue;
                            }
                            else
                            {
                                var keywordAhrefs = new Model.KeywordAhrefs()
                                {
                                    CreateDate = DateTime.Now,
                                    Difficulty = item.Difficulty,
                                    KeywordId = keywordInDb.Id,
                                    Volume = item.Volume
                                };
                                var ahrefsInsert = keywordAhrefsData.Insert(keywordAhrefs);

                                Console.WriteLine("=> Keyword Insert Edildi");
                            }
                        }
                        else
                        {
                            var dateNow = DateTime.Now.Date;
                            var keywordAhrefsInDb = keywordAhrefsData.GetByPage(x => x.KeywordId == keywordInDb.Id, 1, 1, "CreateDate", true).FirstOrDefault();
                            if (keywordAhrefsInDb == null)
                            {
                                //burada insert kesin    
                                keywordAhrefsInDb = new Model.KeywordAhrefs()
                                {
                                    CreateDate = DateTime.Now.Date,
                                    Difficulty = item.Difficulty,
                                    KeywordId = keywordInDb.Id,
                                    Volume = item.Volume
                                };

                                var dbKeywordAhrefsInsertResult = keywordAhrefsData.Insert(keywordAhrefsInDb);
                            }
                            //null degil ama guncel mi
                            else if (keywordAhrefsInDb.Difficulty != item.Difficulty || keywordAhrefsInDb.Volume != item.Volume)
                            {
                                keywordAhrefsInDb.Difficulty = item.Difficulty;
                                keywordAhrefsInDb.Volume = item.Volume;

                                var dbKeywordAhrefsUpdateResult = keywordAhrefsData.Update(keywordAhrefsInDb);
                            }

                            //bu sirada keyword volume bilgisini de guncelleyelim
                            if (keywordInDb.Volume != item.Volume)
                            {
                                keywordInDb.Volume = item.Volume;
                                keywordInDb.VolumeSource = ahrefsSourceId;

                                keywordData.Update(keywordInDb);
                            }
                        }

                        //blogkeyword var mi
                        var slugRegex = new Regex(blog.GetSlugRegex);
                        if (!slugRegex.IsMatch(item.Url))
                        {
                            Console.WriteLine("=> " + item.Url + " content ile uyumlu degil");
                            continue;
                        }

                        var slug = slugRegex.Match(item.Url).Groups[1].Value;
                        var content = contents.FirstOrDefault(x => x.Slug == slug);
                        if (content == null)
                        {
                            Console.WriteLine("=> " + item.Url + " content slug ile uyumlu degil");
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
                                LastPositionDInSC = -1, //sadece search console
                                LastPositionMInSC = -1,
                                LastUpdate = DateTime.Now,
                                PrevPositionDInSC = -1,
                                PrevPositionMInSC = -1,
                                Source = ahrefsSourceId
                            };

                            var blogKeywordInsertResult = blogKeywordData.Insert(blogKeyword);
                            if (!blogKeywordInsertResult.IsSucceed)
                            {
                                Console.WriteLine("=> " + keywordInDb.Query + " için blogKeyword insert edilirken hata oldu:" + blogKeywordInsertResult.Message);
                                continue;
                            }
                        }

                        //position var mi
                        var itemDateStr = itemDate.Date.ToString("yyyy-MM-dd");
                        var blogKeywordTrack = blogKeywordTrackData.FirstOrDefault(x => x.BlogKeywordId == blogKeyword.Id && x.Date == itemDateStr && x.Device == "MOBILE" && x.Source == ahrefsSourceId);
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
                                Position = item.BestPosition,
                                Source = ahrefsSourceId
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

                        blogKeywordTrackData.DetachAllEntities();

                        Console.Clear();
                    }
                }

                Console.Clear();
                Console.WriteLine("Tüm blogları gezdim, 23 saat beklyicem");
                System.Threading.Thread.Sleep(23 * 60 * 60 * 1000);
            }
        }
    }
}
