namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class BlogFormFillValueData : EntityBaseData<Model.BlogFormFillValue>
    {
        public BlogFormFillValueData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
          : base(logger, serviceScopeFactory) { }
    }
}
