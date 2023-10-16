using SomeBlog.Data.Infrastructure;
using SomeBlog.Model;
using SomeBlog.Model.Dto;
using System.Collections.Generic;

namespace SomeBlog.Data.Content
{
    public interface IContentData : IData<Model.Content>
    {
        List<ContentJustIdSlugAuditScoresDto> FilterLightHouseAudits(int maxScore = 90, int blogId = -1, int page = 1, int rowCount = 20);
        List<ContentJustIdSlugDto> GetBlogContentsAllIdSlug(int blogId);
        List<Model.Content> GetByBlog(int blogId);
        List<Model.Content> GetByBlogWithIncludes(int blogId, int count);
        List<Model.Content> GetByIds(int blogId, List<int> ids, int count);
        List<CalendarContentDto> GetForCalendar(int blogId);
        Model.Content GetBySlug(string slug);
        Model.Content GetBySlug(string slug, int blogId);
        Model.Content GetPreviewBySlug(string slug, int blogId);
        List<ContentSitemapSimpleDto> GetContentsForSiteMapGeneration(int blogId);
        List<ContentForListSimpleDto> GetContentSimpleByIds(int blogId, List<int> ids, int count);
        List<Model.Content> GetFeatureContentsOfBlog(int blogId, int count = 5);
        int GetIdBySearch(string query);
        List<Model.Content> GetNewContentsOfBlog(int blogId, int count = 30);
        List<Model.Content> GetPopularContentsOfBlog(int blogId, int count = 10);
        List<Model.Content> SearchContentsByPages(int blogId, int count, string query, int skip = 0);
    }
}