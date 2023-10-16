namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentStartTemplateCategoryData : EntityBaseData<Model.ContentStartTemplateCategory>
    {
        public ContentStartTemplateCategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }
    }
}
