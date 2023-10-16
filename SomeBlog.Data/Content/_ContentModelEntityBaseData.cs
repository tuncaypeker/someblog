namespace SomeBlog.Data.Content
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Model.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ContentModelEntityBaseData : EntityBaseData<Model.Content>, IContentData 
    {
        protected DbContext _context;
        SomeBlog.Infrastructure.Interfaces.ILogger<object> logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContentModelEntityBaseData(SomeBlog.Infrastructure.Interfaces.ILogger<object> logger
            , IServiceScopeFactory serviceScopeFactory)
            :base(logger, serviceScopeFactory)
        {
            this.logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public List<Model.Content> GetByBlog(int blogId)
        {
            return GetBy(x => x.BlogId == blogId);
        }

        public List<Model.Content> GetByBlogWithIncludes(int blogId, int count)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;

                var _context = scope.ServiceProvider.GetService<ContentDataContext>();

                return _context.Set<Model.Content>()
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Include(x => x.ContentTags)
                    .Where(x => x.BlogId == blogId && x.IsActive && !x.IsDelete && x.PublishDate <= dtNow)
                    .OrderByDescending(x => x.Hit)
                    .Take(count)
                    .ToList();
            }
        }

        public List<Model.Content> GetPopularContentsOfBlog(int blogId, int count = 10)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .OrderByDescending("Hit")
                    .Include(x => x.FeaturedMedia)
                    .Where(x => x.BlogId == blogId && x.PublishDate <= dtNow && !x.IsDelete && x.IsActive)
                    .Include(x => x.ContentCategories)
                    .Take(count).ToList();
            }
        }

        public List<Model.Content> GetNewContentsOfBlog(int blogId, int count = 30)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .OrderByDescending("PublishDate")
                    .Include(x => x.FeaturedMedia)
                    .Where(x => x.BlogId == blogId && x.PublishDate <= dtNow && !x.IsDelete && x.IsActive && !x.HasFeatured)
                    .Include(x => x.ContentCategories)
                    .Take(count).ToList();
            }
        }

        public List<Model.Content> GetFeatureContentsOfBlog(int blogId, int count = 10)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                try
                {
                    return _context.Set<Model.Content>()
                   .OrderByDescending("PublishDate")
                   .Include(x => x.FeaturedMedia)
                   .Include(x => x.ContentCategories)
                   .Where(x => x.BlogId == blogId && x.PublishDate <= dtNow && !x.IsDelete && x.HasFeatured && x.IsActive )
                   .Take(count).ToList();
                }
                catch (Exception exc)
                {
                    return null;
                }
            }

        }

        public List<Model.Content> SearchContentsByPages(int blogId, int count, string query, int skip = 0)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .OrderByDescending("PublishDate")
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Where(x => x.BlogId == blogId && x.PublishDate <= dtNow && !x.IsDelete && x.IsActive && (x.PageTitle.Contains(query) || x.Text.Contains(query)))
                    .Skip(skip).Take(count).ToList();
            }
        }

        public List<Model.Content> GetByIds(int blogId, List<int> ids, int count)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .OrderByDescending("PublishDate")
                    .Include(x => x.FeaturedMedia)
                    //.Include(x => x.ContentCategories)
                    .Where(x => x.BlogId == blogId && x.IsActive && ids.Contains(x.Id) && !x.IsDelete && x.PublishDate <= dtNow)
                    .Take(count).ToList();
            }
        }

        public List<Model.Dto.ContentForListSimpleDto> GetContentSimpleByIds(int blogId, List<int> ids, int count)
        {
            if (ids.Count == 0)
                return new List<ContentForListSimpleDto>();

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .Where(x => x.BlogId == blogId && x.IsActive && ids.Contains(x.Id) && !x.IsDelete && x.PublishDate <= dtNow)
                    .OrderByDescending("PublishDate")
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Select(p => new Model.Dto.ContentForListSimpleDto()
                    {
                        FeaturedMediaUrl = p.FeaturedMediaId > 0 ? p.FeaturedMedia.MediaUrl : "",
                        FeaturedMediaAlt = p.FeaturedMediaId > 0 ? p.FeaturedMedia.Alt : "",
                        FeaturedMediaTitle = p.FeaturedMediaId > 0 ? p.FeaturedMedia.Title : "",
                        FeaturedMediaHasAvif = p.FeaturedMediaId > 0 ? p.FeaturedMedia.HasAvif : false,
                        FeaturedMediaHasWebp = p.FeaturedMediaId > 0 ? p.FeaturedMedia.HasWebp : false,
                        FeaturedMediaHeight = p.FeaturedMediaId > 0 ? p.FeaturedMedia.Height : 0,
                        FeaturedMediaWidth = p.FeaturedMediaId > 0 ? p.FeaturedMedia.Width : 0,
                        PublishDate = p.PublishDate,
                        Slug = p.Slug,
                        PageTitle = p.MetaTitle,
                        PageDescription = p.PageDescription,
                        FirstCategoryId = p.ContentCategories.Count > 0 ? p.ContentCategories[0].CategoryId : -1,
                        CreateDate = p.CreateDate
                    })
                    .Take(count).ToList();
            }
        }

        public Model.Content GetBySlug(string slug)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;

                var _context = scope.ServiceProvider.GetService<ContentDataContext>();

                return _context.Set<Model.Content>()
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Include(x => x.ContentTags)
                    .Where(x => x.Slug == slug.ToLowerInvariant() && x.IsActive && !x.IsDelete && x.PublishDate <= dtNow)
                    .FirstOrDefault();
            }
        }

        public Model.Content GetBySlug(string slug, int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;

                var _context = scope.ServiceProvider.GetService<ContentDataContext>();

                return _context.Set<Model.Content>()
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Include(x => x.ContentTags)
                    .Where(x => x.Slug == slug.ToLowerInvariant() && x.BlogId == blogId && x.IsActive && !x.IsDelete && x.PublishDate <= dtNow)
                    .FirstOrDefault();
            }
        }

        public Model.Content GetPreviewBySlug(string slug, int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<ContentDataContext>();

                return _context.Set<Model.Content>()
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .Include(x => x.ContentTags)
                    .Where(x => x.Slug == slug.ToLowerInvariant() && x.BlogId == blogId)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Son 2 aydan itibaren, önümüzdeki 3 ay için, icerik listesini getir takvimde gostercem
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public List<CalendarContentDto> GetForCalendar(int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtStart = DateTime.Now.AddDays(-60);
                var dtEnd = DateTime.Now.AddDays(180);

                var _context = scope.ServiceProvider.GetService<ContentDataContext>();

                return _context.Set<Model.Content>()
                    .Where(x=>x.PublishDate >= dtStart && x.PublishDate <= dtEnd && x.BlogId == blogId)
                    .Select(p => new CalendarContentDto() { 
                        start = p.PublishDate.ToString("yyyy-MM-dd"),
                        title = $"[{p.PublishDate.ToString("HH:mm")}] {p.PageTitle}",
                        slug = p.Slug,
                        id = p.Id,
                        isActive = p.IsActive
                    }).ToList();
            }
        }

        public List<ContentSitemapSimpleDto> GetContentsForSiteMapGeneration(int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                var query = _context.Set<Model.Content>()
                               .Where(p => p.BlogId == blogId && p.IsActive && !p.IsDelete && p.PublishDate <= dtNow)
                               .OrderByDescending("PublishDate")
                               .Select(p => new ContentSitemapSimpleDto()
                               {
                                   Slug = p.Slug,
                                   UpdateDate = p.UpdateDate,
                                   PublishDate = p.PublishDate,
                                   CreateDate = p.CreateDate
                               }).AsQueryable();

                return query.ToList();
            }
        }

        public List<ContentJustIdSlugDto> GetBlogContentsAllIdSlug(int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                var query = _context.Set<Model.Content>()
                               .Where(p => p.BlogId == blogId && p.IsActive && !p.IsDelete && p.PublishDate <= dtNow)
                               .Select(p => new ContentJustIdSlugDto()
                               {
                                   Slug = p.Slug,
                                   Id = p.Id
                               }).AsQueryable();

                return query.ToList();
            }
        }

        public List<ContentJustIdSlugAuditScoresDto> FilterLightHouseAudits(int maxScore = 90, int blogId = -1, int page = 1, int rowCount = 20)
        {
            var maxScoreToCompare = (decimal)((decimal)maxScore / 100);

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                var query = _context.Set<Model.Content>()
                               .Where(p => p.IsActive && !p.IsDelete && p.PublishDate <= dtNow && (p.SeoScore < maxScoreToCompare || p.AccessibilityScore < maxScoreToCompare || p.BestPracticesScore < maxScoreToCompare || p.PerformanceScore < maxScoreToCompare))
                               .AsQueryable();

                if (blogId > 0)
                    query = query.Where(x => x.BlogId == blogId).AsQueryable();

                return query.Select(p => new ContentJustIdSlugAuditScoresDto()
                {
                    Slug = p.Slug,
                    Id = p.Id,
                    AccessibilityScore = p.AccessibilityScore,
                    BestPracticesScore = p.BestPracticesScore,
                    BlogId = p.BlogId,
                    PerformanceScore = p.PerformanceScore,
                    SeoScore = p.SeoScore
                })
                               .Skip((page - 1) * rowCount)
                               .Take(rowCount)
                               .ToList();
            }
        }

        /// <summary>
        /// ContentCategories de Include edilir
        /// Stack Content'ler de gelir, dikkatli olmakta fayda var
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public List<Model.Content> GetContentsForFeedRss(int blogId)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                var query = _context.Set<Model.Content>()
                               .Where(p => p.BlogId == blogId && p.IsActive && !p.IsDelete && p.PublishDate <= dtNow)
                               .Include(x=>x.FeaturedMedia)
                               .Include(x=>x.ContentCategories)
                               .OrderByDescending("UpdateDate")
                               .Take(10);

                return query.ToList();
            }
        }

        /// <summary>
        /// ContentCategories de Include edilir
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="ids"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Model.Content> GetByIdsForFeedRss(int blogId, List<int> ids, int count)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                var dtNow = DateTime.Now;
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                return _context.Set<Model.Content>()
                    .Include(x => x.FeaturedMedia)
                    .Include(x => x.ContentCategories)
                    .OrderByDescending("PublishDate")
                    .Where(x => x.BlogId == blogId && x.IsActive && ids.Contains(x.Id) && !x.IsDelete && x.PublishDate <= dtNow)
                    .Take(count).ToList();
            }
        }

        public int GetIdBySearch(string query)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<ContentDataContext>();
                var iquery = _context.Set<Model.Content>()
                           .Where(p => p.PageTitle.Contains(query) || p.Slug.Contains(query))
                           .Select(p => p.Id)
                           .ToList();

                if (iquery == null || iquery.Count == 0)
                    return -1;

                return iquery.FirstOrDefault();
            }
        }
    }
}
