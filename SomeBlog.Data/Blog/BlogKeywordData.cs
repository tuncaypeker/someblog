namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using SomeBlog.Data.Infrastructure;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BlogKeywordData : EntityBaseData<Model.BlogKeyword>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        BlogKeywordTrackData blogKeywordTrackData;

        /// <summary>
        /// Silme islemi yapmayacaksan diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public BlogKeywordData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {

        }

        public BlogKeywordData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, IOptions<DatabaseSettings> dbOptions, BlogKeywordTrackData blogKeywordTrackData) : base(logger, serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
            this.blogKeywordTrackData = blogKeywordTrackData;
        }

        public List<Model.BlogKeyword> GetBlogKeywordsWithContent(int blogId, int pageNumber, int pageCount, string orderBy)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                try
                {
                    return _context.Set<Model.BlogKeyword>()
                        .Where(x => x.BlogId == blogId)
                        .Include(x => x.Keyword)
                        //.Include(x => x.Content)
                        .OrderByDescending(orderBy).Skip((pageNumber - 1) * pageCount).Take(pageCount).ToList();
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
        }

        public List<Model.BlogKeyword> GetNewBlogKeywords(int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                int searchConsoleValue = (int)Model.Enums.SeoTools.Google_Search_Console;
                var date2DaysAgo = DateTime.Now.AddDays(-2);

                var dataContext = scope.ServiceProvider.GetService<DataContext>();
                var query = (from bg in dataContext.BlogKeywords
                             join k in dataContext.Keywords on new { keyword = bg.KeywordId } equals new { keyword = k.Id }
                             where k.CreateDate > date2DaysAgo && bg.BlogId == blogId && k.Source == searchConsoleValue
                             orderby k.CreateDate descending
                             select bg)
                            .Include(x => x.Keyword);

                return query.Take(50).ToList();
            }
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var blogKeywords = GetBy(x => x.BlogId == blogId);
            foreach (var blogKeyword in blogKeywords)
                DeleteWithAllRelations(blogKeyword.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultTrack = blogKeywordTrackData.DeleteBulk(x => x.BlogKeywordId == id);

            return DeleteByKey(id);
        }
    }
}
