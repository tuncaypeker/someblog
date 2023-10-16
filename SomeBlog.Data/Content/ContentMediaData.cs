namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Infrastructure.Interfaces;
    using System.Collections.Generic;

    public class ContentMediaData : EntityBaseData<Model.ContentMedia>
    {
        public ContentMediaData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory) { }

        public List<Model.ContentMedia> GetByContentId(int id)
        {
            return GetBy(x => x.ContentId == id);
        }
    }
}
