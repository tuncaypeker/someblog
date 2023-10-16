namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class PinterestPostData : EntityBaseData<Model.PinterestPost>
    {
        public PinterestPostData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory): base(logger, serviceScopeFactory) { }
    }
}
