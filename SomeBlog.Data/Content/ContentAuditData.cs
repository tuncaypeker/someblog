namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentAuditData : EntityBaseData<Model.ContentAudit>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContentAuditData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) 
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }
    }
}
