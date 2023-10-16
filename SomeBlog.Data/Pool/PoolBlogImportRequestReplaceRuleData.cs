namespace SomeBlog.Data.Pool
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class PoolBlogImportRequestReplaceRuleData : EntityBaseData<Model.PoolBlogImportRequestReplaceRule>
    {
        public PoolBlogImportRequestReplaceRuleData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
