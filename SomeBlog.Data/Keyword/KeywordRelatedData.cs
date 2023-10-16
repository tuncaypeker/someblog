namespace SomeBlog.Data.Keyword
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class KeywordRelatedData : EntityBaseData<Model.KeywordRelated>
    {
        public KeywordRelatedData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
