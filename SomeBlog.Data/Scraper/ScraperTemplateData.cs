namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;

    public class ScraperTemplateData : EntityBaseData<Model.ScraperTemplate>
    {
        public ScraperTemplateData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory)
        {
        }
    }
}
