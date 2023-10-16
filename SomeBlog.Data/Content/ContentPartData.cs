namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentPartData : EntityBaseData<Model.ContentPart>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContentPartData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) 
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }
    }
}
