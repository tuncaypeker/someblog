using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Infrastructure.Interfaces;
using SomeBlog.RabbitMQClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SomeBlog.Bots.RequestLogProcessor
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static ILogger<Program> logger;
        static Data.BotProgramData botProgramData;
        static Data.SearchEngineBotLogData searchEngineBotLogData;
        static Data.Content.ContentData contentData;
        static Data.Content.ContentHitHistoryData contentHistoryLogData;

        static List<Model.Blog> blogs;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .Enrich.WithProperty("Application", "SomeBlog.RequestLogProcessor")
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
              .MinimumLevel.Override("System", LogEventLevel.Verbose)
              .WriteTo.Elasticsearch(
                  new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                      new Uri("http://localhost:9200/"))
                  {
                      CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                      AutoRegisterTemplate = true,
                      TemplateName = "serilog-events-template",
                      IndexFormat = "someblog-log-{0:yyyy.MM.dd}"
                  })
              .CreateLogger();

            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var blogData = serviceProvider.GetService<BlogData>();
            contentData = serviceProvider.GetService<Data.Content.ContentData>();
            contentHistoryLogData = serviceProvider.GetService<Data.Content.ContentHitHistoryData>();
            searchEngineBotLogData = serviceProvider.GetService<SearchEngineBotLogData>();
            var requestIpBlackListData = serviceProvider.GetService<RequestIpBlackListData>();
            botProgramData = serviceProvider.GetService<Data.BotProgramData>();
            var iRabbitMQService = serviceProvider.GetService<IRabbitMQService>();

            logger = (ILogger<Program>)serviceProvider.GetService(typeof(ILogger<Program>));

            blogs = blogData.GetAll();

            SomeBlog.RabbitMQClient.Consumer<RequestLog> consumer = new RabbitMQClient.Consumer<RequestLog>("someblog", iRabbitMQService);
            consumer.ConsumeObject(Consumer_Received);

            Console.ReadLine();
        }

        private static async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());

            botProgramData.UpdatePulse("SomeBlog.Bots.RequestLogProcessor");

            //var body = ea.Body.ToArray();
            //var jsonified = Encoding.UTF8.GetString(body);
            RequestLog requestLog = JsonConvert.DeserializeObject<RequestLog>(message);

            Model.Content content = null;
            Model.Blog blog = blogs.FirstOrDefault(x => x.Id == requestLog.BlogId);

            Console.WriteLine($"{blog.Name}: {requestLog.Date} => {requestLog.Path} / {requestLog.IpAddress}");

            if (blog == null)
            {
                Console.WriteLine($"Blog Id Bulunamadı: {requestLog.BlogId}");

                logger.Error("BlogId: {blogId} bulunamadi..", requestLog.BlogId);
                return;
            }

            var path = blog.Url + requestLog.Path;
            var slugRegex = new Regex(blog.GetSlugRegex);
            if (slugRegex.IsMatch(path))
            {
                var slug = slugRegex.Match(path).Groups[1].Value;
                content = contentData.GetBySlug(slug);

                if (content != null)
                {
                    content.Hit = content.Hit + 1;
                    var contentHitUpdateDbResult = contentData.Update(content);

                    var dtNow = DateTime.Now;
                    var historyInDb = contentHistoryLogData.FirstOrDefault(x => x.CreateDate.Day == dtNow.Day && x.CreateDate.Month == dtNow.Month && x.CreateDate.Year == dtNow.Year && x.ContentId == content.Id);
                    if (historyInDb == null)
                    {
                        historyInDb = new Model.ContentHitHistory()
                        {
                            BlogId = content.BlogId,
                            ContentId = content.Id,
                            CreateDate = DateTime.Now.Date,
                            Hit = 1
                        };

                        var contentHistoryInsertDbResult = contentHistoryLogData.Insert(historyInDb);
                    }
                    else
                    {
                        historyInDb.Hit = historyInDb.Hit + 1;

                        var contentHistoryUpdateDbResult = contentHistoryLogData.Update(historyInDb);
                    }
                }
            }

            //acaba bu google bot mu
            //https://www.onely.com/blog/detect-verify-crawlers/
            if (!string.IsNullOrEmpty(requestLog.UserAgent) && requestLog.UserAgent.ToLower().Contains("googlebot"))
            {
                var lookup = new LookupClient();
                var ipLookup = lookup.QueryReverse(System.Net.IPAddress.Parse(requestLog.IpAddress));

                if (ipLookup.Answers != null && ipLookup.Answers.Count > 0)
                {
                    var answer = ((DnsClient.Protocol.PtrRecord)ipLookup.Answers[0]);
                    var domainName = answer.PtrDomainName.Value;
                    if (domainName.Contains("googlebot.com"))
                    {
                        var domainLookup = lookup.Query(domainName, QueryType.A);
                        if (domainLookup.Answers != null && domainLookup.Answers.Count > 0)
                        {
                            System.Net.IPAddress ipAddress = null;

                            try
                            {
                                ipAddress = ((DnsClient.Protocol.AddressRecord)domainLookup.Answers[0]).Address;
                            }
                            catch (Exception exc)
                            {
                            }

                            if (ipAddress != null && ipAddress.ToString() == requestLog.IpAddress)
                            {
                                //google bot
                                //genel log atalim
                                var googleBotLog = new Model.SearchEngineBotLog()
                                {
                                    Date = DateTime.Now,
                                    Path = requestLog.Path,
                                    IpAddress = requestLog.IpAddress,
                                    ContentId = content == null ? -1 : content.Id,
                                    BlogId = blog.Id,
                                    SearchEngine = 1
                                };
                                var dbResult = searchEngineBotLogData.Insert(googleBotLog);

                                if (content != null)
                                {
                                    content.LastGoogleBotDate = DateTime.Now;
                                    var contentLastGoogleBotUpdateResult = contentData.Update(content);
                                }
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(requestLog.UserAgent) && requestLog.UserAgent.ToLower().Contains("bingbot"))
            {
                var lookup = new LookupClient();
                var ipLookup = lookup.QueryReverse(System.Net.IPAddress.Parse(requestLog.IpAddress));

                if (ipLookup.Answers != null && ipLookup.Answers.Count > 0)
                {
                    var answer = ((DnsClient.Protocol.PtrRecord)ipLookup.Answers[0]);
                    var domainName = answer.PtrDomainName.Value;
                    if (domainName.Contains("search.msn.com"))
                    {
                        var domainLookup = lookup.Query(domainName, QueryType.A);
                        if (domainLookup.Answers != null && domainLookup.Answers.Count > 0)
                        {
                            System.Net.IPAddress ipAddress = null;

                            try
                            {
                                ipAddress = ((DnsClient.Protocol.AddressRecord)domainLookup.Answers[0]).Address;
                            }
                            catch (Exception exc)
                            {
                            }
                            if (ipAddress != null && ipAddress.ToString() == requestLog.IpAddress)
                            {
                                //google bot
                                //genel log atalim
                                var bingBotLog = new Model.SearchEngineBotLog()
                                {
                                    Date = DateTime.Now,
                                    Path = requestLog.Path,
                                    IpAddress = requestLog.IpAddress,
                                    ContentId = content == null ? -1 : content.Id,
                                    BlogId = blog.Id,
                                    SearchEngine = 2
                                };
                                var dbResult = searchEngineBotLogData.Insert(bingBotLog);
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(requestLog.UserAgent) && requestLog.UserAgent.ToLower().Contains("yandex"))
            {
                var lookup = new LookupClient();
                var ipLookup = lookup.QueryReverse(System.Net.IPAddress.Parse(requestLog.IpAddress));

                if (ipLookup.Answers != null && ipLookup.Answers.Count > 0)
                {
                    var answer = ((DnsClient.Protocol.PtrRecord)ipLookup.Answers[0]);
                    var domainName = answer.PtrDomainName.Value;
                    if (domainName.Contains("yandex.")) //yandex.com, yandex.ru, yandex.net
                    {
                        var domainLookup = lookup.Query(domainName, QueryType.A);
                        if (domainLookup.Answers != null && domainLookup.Answers.Count > 0)
                        {
                            var ipAddress = ((DnsClient.Protocol.AddressRecord)domainLookup.Answers[0]).Address;
                            if (ipAddress.ToString() == requestLog.IpAddress)
                            {
                                var yandexBotLog = new Model.SearchEngineBotLog()
                                {
                                    Date = DateTime.Now,
                                    Path = requestLog.Path,
                                    IpAddress = requestLog.IpAddress,
                                    ContentId = content == null ? -1 : content.Id,
                                    BlogId = blog.Id,
                                    SearchEngine = 3
                                };
                                var dbResult = searchEngineBotLogData.Insert(yandexBotLog);
                            }
                        }
                    }
                }
            }
            
            logger.Information("Request Kaydedildi: {ip}, {path}, {referer}, {useragent}, {blogId}", requestLog.IpAddress, requestLog.Path, requestLog.Referer, requestLog.UserAgent, requestLog.BlogId);

            //eger bu ip ile 15dk'da 10'dan fazla istek varsa black list'e ekle
            //15dk'da 10 istek cok cok az, cunku, resim istekleri, getallcomments istekleri vs buna dahil oluyorusa nerdeyse tek istekde 5-6 request log kaydedilir
            //o olmasa bile bir ikişi 15dk'da cok rahat 10 istek gonderir
            /*
            var requestOfThisIps = requestIpLogData.GetBy(x => x.IpAddress == requestLogToDb.IpAddress && x.BlogId == requestLogToDb.BlogId);
            var dt15Minutes = DateTime.Now.AddMinutes(-15);
            var dt15MinutesRequestCount = requestOfThisIps.Where(x => x.Date > dt15Minutes).Count();
            if (dt15MinutesRequestCount > 10) {
                var requestIpBlackListInDb = requestIpBlackListData.FirstOrDefault(x => x.IpAddress == requestLog.IpAddress);
                if (requestIpBlackListInDb == null)
                {
                    requestIpBlackListInDb = new Model.RequestIpBlackList()
                    {
                        IpAddress = requestLog.IpAddress,
                        AddedDate = DateTime.Now,
                        RemoveDate = DateTime.Now.AddDays(1)
                    };

                    var insertResult = requestIpBlackListData.Insert(requestIpBlackListInDb);
                }
            }
            */

        }
    }

    class RequestLog
    {
        public string IpAddress { get; set; }
        public string Path { get; set; }
        public int BlogId { get; set; }
        public DateTime Date { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }
    }
}
