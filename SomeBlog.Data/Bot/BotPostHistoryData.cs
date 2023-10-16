namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class BotPostHistoryData : EntityBaseData<Model.BotPostHistory>
    {
        public BotPostHistoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
