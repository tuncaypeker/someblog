using Microsoft.Extensions.DependencyInjection;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Keyword;
using SomeBlog.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Bots.MozComUpdater
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var blogData = serviceProvider.GetService<BlogData>();
            var keywordData = serviceProvider.GetService<KeywordData>();
            var keywordMozData = serviceProvider.GetService<KeywordMozData>();
            var keywordSerpData = serviceProvider.GetService<KeywordSerpData>();
            var botProgramData = serviceProvider.GetService<Data.BotProgramData>();

            var mozService = new Integration.MozPro.Service();

            //amacimiz search query'leri guncellemek, moz update 6 ayda bir yapsak yeter
            while (true)
            {
                botProgramData.UpdatePulse("SomeBlog.Bots.MozComUpdater");

                var dateSixMonthsBefore = DateTime.Now.AddMonths(-6);
                var keywordInDb = keywordData.FirstOrDefault(x => x.MozUpdateDate < dateSixMonthsBefore);
                if (keywordInDb == null)
                {
                    Console.WriteLine("Veri alamadım ya da hepsi güncel, 40 sn bekliyorum...");
                    System.Threading.Thread.Sleep(40 * 1000 * 1);
                    continue;
                }

                var keywordMozInDb = keywordMozData.GetByPage(x => x.KeywordId == keywordInDb.Id, 1, 1, "CreateDate", isDesc: true).FirstOrDefault();
                var dateNow = DateTime.Now.Date;
                if (keywordMozInDb != null && keywordMozInDb.CreateDate.Date == dateNow)
                {
                    Console.WriteLine("\t bugun guncellenmiş kayit var");

                    keywordInDb.MozUpdateDate = DateTime.Now;
                    keywordData.Update(keywordInDb);

                    continue;
                }

                Console.WriteLine(keywordInDb.Query + " ile çalışıyorum");
                var result = mozService.AnalyisKeyword(keywordInDb.Query);
                if (result == null)
                {
                    Console.WriteLine("\t moz'dan cevap alamadim, 30 sn bekliyorum");
                    System.Threading.Thread.Sleep(30 * 1000 * 1);
                    continue;
                }

                keywordMozInDb = new Model.KeywordMoz();
                keywordMozInDb.Difficulty = result.Difficulty;
                keywordMozInDb.ExactVolume = result.ExactVolume;
                keywordMozInDb.Opportunity = result.Opportunity;
                keywordMozInDb.Potential = result.Potential;
                keywordMozInDb.CreateDate = DateTime.Now;
                var keywordMozInsert = keywordMozData.Insert(keywordMozInDb);

                if (keywordMozInsert.IsSucceed)
                {
                    keywordInDb.MozUpdateDate = DateTime.Now;
                    var dbResult = keywordData.Update(keywordInDb);
                    Console.WriteLine($"\tqueryUpdate: {dbResult.IsSucceed} / D:{keywordMozInDb.Difficulty} / P:{keywordMozInDb.Potential}");
                }

                var mozProSourceId = (int)SeoTools.MozPro;
                var keywordSerpsInDb = keywordSerpData.GetBy(x => x.KeywordId == keywordInDb.Id && x.Source == mozProSourceId);
                var insertBulkSerpList = new List<Model.KeywordSerp>();
                foreach (var serpFromMozpro in result.Serp.Results)
                {
                    var keywordSerpInDb = keywordSerpsInDb.FirstOrDefault(x => x.Rank == serpFromMozpro.Rank);
                    if (keywordSerpInDb == null)
                    {
                        insertBulkSerpList.Add(new Model.KeywordSerp()
                        {
                            CreateDate = DateTime.Now,
                            DomainAuthority = serpFromMozpro.DomainAuthority,
                            LinkingDomainsToDomain = serpFromMozpro.LinkingDomainsToDomain,
                            LinkingDomainsToPage = serpFromMozpro.LinkingDomainsToPage,
                            PageAuthority = serpFromMozpro.PageAuthority,
                            Rank = serpFromMozpro.Rank,
                            KeywordId = keywordInDb.Id,
                            Title = serpFromMozpro.Title,
                            Type = serpFromMozpro.Type,
                            Url = serpFromMozpro.Url,
                            Source = mozProSourceId
                        });

                        continue;
                    }

                    if (keywordSerpInDb.Url != serpFromMozpro.Url)
                    {
                        keywordSerpInDb.CreateDate = DateTime.Now;
                        keywordSerpInDb.DomainAuthority = serpFromMozpro.DomainAuthority;
                        keywordSerpInDb.LinkingDomainsToDomain = serpFromMozpro.LinkingDomainsToDomain;
                        keywordSerpInDb.LinkingDomainsToPage = serpFromMozpro.LinkingDomainsToPage;
                        keywordSerpInDb.PageAuthority = serpFromMozpro.PageAuthority;
                        keywordSerpInDb.Title = serpFromMozpro.Title;
                        keywordSerpInDb.Type = serpFromMozpro.Type;
                        keywordSerpInDb.Url = serpFromMozpro.Url;
                        keywordSerpInDb.Source = mozProSourceId;

                        var dbUpdateSerpResult = keywordSerpData.Update(keywordSerpInDb);
                    }
                }

                var insertBulkSerpResult = keywordSerpData.InsertBulk(insertBulkSerpList);
                Console.WriteLine(insertBulkSerpResult.IsSucceed
                   ? $"=> {insertBulkSerpList.Count} sertp ekledim"
                   : $"=> serp insert bulk hata oldu {insertBulkSerpResult.Message}");

                //peki ya onceden rank'a eklenmis ama sonra yeni gelen listede olmayan varsa napcaz
                //silebiliriz ya da 11 die sıralayabiliriz yanlis olur
                //o yuzden silelim
                var keywordSerpIdsNotExistNow = keywordSerpsInDb.Where(x => result.Serp.Results.Any(y => y.Url != x.Url))
                    .Select(x => x.Id).ToArray();
                keywordSerpData.DeleteBulk(x => keywordSerpIdsNotExistNow.Contains(x.Id));

                keywordSerpData.DetachAllEntities();
            }
        }
    }
}
