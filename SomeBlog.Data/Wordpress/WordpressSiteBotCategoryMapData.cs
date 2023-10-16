namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class WordpressSiteBotCategoryMapData : EntityBaseData<Model.WordpressSiteBotCategoryMap>
    {
        public WordpressSiteBotCategoryMapData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
