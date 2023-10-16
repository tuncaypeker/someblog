namespace SomeBlog.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoryData : EntityBaseData<Model.Category>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        Data.Content.ContentCategoryData contentCategoryData;
        BotCategoryMapData botCategoryMapData;

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public CategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public CategoryData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, Data.Content.ContentCategoryData contentCategoryData, BotCategoryMapData botCategoryMapData)
            : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
            this.contentCategoryData = contentCategoryData;
            this.botCategoryMapData = botCategoryMapData;
        }

        public List<Model.Category> GetByBlog(int blogId)
        {
            return GetBy(x => x.BlogId == blogId);
        }

        public Model.Category GetBySlug(string slug)
        {
            return GetBy(x => x.Slug == slug.ToLowerInvariant())
                .FirstOrDefault();
        }

        public List<Model.Category> GetByIds(List<int> ids)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                return _context.Set<Model.Category>()
                .OrderByDescending("Id")
                .Where(x => ids.Contains(x.Id))
                .ToList();
            }
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var categories = GetBy(x => x.BlogId == blogId);
            foreach (var category in categories)
                DeleteWithAllRelations(category.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultBrowser = contentCategoryData.DeleteBulk(x => x.CategoryId == id);
            var dataResultBotCategoryMap = botCategoryMapData.DeleteBulk(x => x.LocalCategoryId == id);

            return DeleteByKey(id);
        }
    }
}
