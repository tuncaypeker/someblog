namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TagData : EntityBaseData<Model.Tag>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TagData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public List<Model.Tag> GetByBlog(int blogId, int page = 1, int rowCount = 20)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                return _context.Set<Model.Tag>()
                    .Where(x => x.BlogId == blogId)
                    .Skip((page - 1) * rowCount).Take(rowCount).ToList();
            }
        }

        public List<Model.Tag> GetByIds(int blogId, IEnumerable<int> ids, int count)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                return _context.Set<Model.Tag>()
                .Where(x => x.BlogId == blogId && ids.Contains(x.Id))
                .Take(count).ToList();
            }
        }

        public Model.Tag GetBySlug(string slug)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                return GetBy(x => x.Slug == slug.ToLowerInvariant())
                .FirstOrDefault();
            }
        }
    }
}
