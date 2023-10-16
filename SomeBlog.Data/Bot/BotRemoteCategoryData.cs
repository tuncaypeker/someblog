namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class BotRemoteCategoryData : EntityBaseData<Model.BotRemoteCategory>
    {
        public BotRemoteCategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
