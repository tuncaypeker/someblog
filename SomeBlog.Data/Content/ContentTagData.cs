namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentTagData : EntityBaseData<Model.ContentTag>
    {
        public ContentTagData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
