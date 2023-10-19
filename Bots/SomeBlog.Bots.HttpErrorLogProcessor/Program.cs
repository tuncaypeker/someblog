using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using SomeBlog.Bots.Core;
using SomeBlog.Data;
using SomeBlog.Data.Infrastructure.Entities;
using SomeBlog.Data.Keyword;
using SomeBlog.Infrastructure.Interfaces;
using SomeBlog.Infrastructure.Logging.DummyLog;
using SomeBlog.Model;
using SomeBlog.RabbitMQClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBlog.Bots.HttpErrorLogProcessor
{
    class Program
    {
        static ServiceProvider serviceProvider;
        static Data.BotProgramData botProgramData;
        static Data.HttpErrorLogData httpErrorLogData;
        static List<Model.Blog> blogs;

        static void Main(string[] args)
        {
            serviceProvider = ServiceProviderHelper.BuildServiceProvider();

            var blogData = serviceProvider.GetService<BlogData>();
            httpErrorLogData = serviceProvider.GetService<HttpErrorLogData>();
            botProgramData = serviceProvider.GetService<Data.BotProgramData>();
            var iRabbitMQService = serviceProvider.GetService<IRabbitMQService>();

            blogs = blogData.GetAll();

            SomeBlog.RabbitMQClient.Consumer<Model.HttpErrorLog> consumer = new RabbitMQClient.Consumer<Model.HttpErrorLog>("httperrors", iRabbitMQService);
            consumer.ConsumeObject(Consumer_Received);

            Console.ReadLine();
        }

        private static async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            botProgramData.UpdatePulse("SomeBlog.Bots.HttpErrorLogProcessor");

            var message = Encoding.UTF8.GetString(@event.Body.ToArray());
            var httpErrorLog = JsonConvert.DeserializeObject<HttpErrorLog>(message);

            var httpErrorLogInDb = httpErrorLogData.FirstOrDefault(x => x.BlogId == httpErrorLog.BlogId && x.Path == httpErrorLog.Path && x.StatusCode == httpErrorLog.StatusCode);
            DataResult dbResult;
            if (httpErrorLogInDb != null)
            {
                httpErrorLogInDb.LastHitDate = DateTime.Now;
                httpErrorLogInDb.HitCount += 1;

                dbResult = httpErrorLogData.Update(httpErrorLogInDb);
            }
            else
            {
                httpErrorLog.HitCount = 1;

                dbResult = httpErrorLogData.Insert(httpErrorLog);
            }

            Model.Blog blog = blogs.FirstOrDefault(x => x.Id == httpErrorLog.BlogId);

            Console.WriteLine($"{dbResult.IsSucceed}: {blog.Name} / {httpErrorLog.LastHitDate} => {httpErrorLog.StatusCode} / {httpErrorLog.IpAddress} / {httpErrorLog.Path}");
            if (!dbResult.IsSucceed)
                Console.WriteLine(dbResult.Message);

            if (blog == null)
            {
                Console.WriteLine($"Blog Id Bulunamadı: {httpErrorLog.BlogId}");
                return;
            }
        }
    }
}
