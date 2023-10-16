namespace SomeBlog.Data.Content
{
    using Microsoft.Extensions.DependencyInjection;
    using SomeBlog.Data.Infrastructure.Entities;
    using SomeBlog.Data.Keyword;
    using SomeBlog.Infrastructure.Interfaces;

    public class ContentData : ContentModelEntityBaseData
    {
        BlogKeywordData blogKeywordData;
        CommentData commentData;
        ContentAuditData contentAuditData;
        ContentCategoryData contentCategoryData;
        ContentHitHistoryData contentHitHistoryData;
        ContentKeywordTailData contentKeywordTailData;
        ContentLighthouseAuditData contentLighthouseAuditData;
        ContentMediaData contentMediaData;
        ContentMetaData contentMetaData;
        ContentTagData contentTagData;
        ContentPartData contentPartData;
        ContentUpdateHistoryData contentUpdateHistoryData;
        KeywordTempMovementData keywordTempMovementData;
        SearchEngineBotLogData searchEngineBotLogData;
        SearchEngineSubmitLogData searchEngineSubmitLogData;

        /// <summary>
        /// Silme islemi yoksa diger binding'lere gerek yok
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public ContentData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        { 
            
        }

        public ContentData(ILogger<object> logger, IServiceScopeFactory serviceScopeFactory, BlogKeywordData blogKeywordData,
            CommentData commentData, ContentAuditData contentAuditData, ContentCategoryData contentCategoryData, ContentHitHistoryData contentHitHistoryData,
            ContentKeywordTailData contentKeywordTailData, ContentLighthouseAuditData contentLighthouseAuditData,
            ContentMediaData contentMediaData, ContentMetaData contentMetaData, ContentTagData contentTagData, ContentUpdateHistoryData contentUpdateHistoryData,
            KeywordTempMovementData keywordTempMovementData, SearchEngineBotLogData searchEngineBotLogData, SearchEngineSubmitLogData searchEngineSubmitLogData, ContentPartData contentPartData)
            : base(logger, serviceScopeFactory)
        {
            this.blogKeywordData = blogKeywordData;
            this.commentData = commentData;
            this.contentAuditData = contentAuditData;
            this.contentCategoryData = contentCategoryData;
            this.contentHitHistoryData = contentHitHistoryData;
            this.contentKeywordTailData = contentKeywordTailData;
            this.contentLighthouseAuditData = contentLighthouseAuditData;
            this.contentMediaData = contentMediaData;
            this.contentMetaData = contentMetaData;
            this.contentTagData = contentTagData;
            this.contentUpdateHistoryData = contentUpdateHistoryData;
            this.keywordTempMovementData = keywordTempMovementData;
            this.searchEngineBotLogData = searchEngineBotLogData;
            this.searchEngineSubmitLogData = searchEngineSubmitLogData;
            this.contentPartData = contentPartData;
        }

        public DataResult DeleteWithAllRelationsByBlog(int blogId)
        {
            var contents = GetBy(x => x.BlogId == blogId);
            foreach (var content in contents)
                DeleteWithAllRelations(content.Id);

            return new DataResult(true, "");
        }

        public DataResult DeleteWithAllRelations(int id)
        {
            var dataResultBlogKeyword = blogKeywordData.DeleteBulk(x => x.ContentId == id);
            var dataResultComment = commentData.DeleteBulk(x => x.ContentId == id);
            var dataResultAudit = contentAuditData.DeleteBulk(x => x.ContentId == id);
            var dataResultCategory = contentCategoryData.DeleteBulk(x => x.ContentId == id);
            var dataResultHitHistory = contentHitHistoryData.DeleteBulk(x => x.ContentId == id);
            var dataResultTail = contentKeywordTailData.DeleteBulk(x => x.ContentId == id);
            var dataResultLighthouseAudit = contentLighthouseAuditData.DeleteBulk(x => x.ContentId == id);
            var dataMedia = contentMediaData.DeleteBulk(x => x.ContentId == id);
            var dataMeta = contentMetaData.DeleteBulk(x => x.ContentId == id);
            var dataTag = contentTagData.DeleteBulk(x => x.ContentId == id);
            var dataUpdateHistory = contentUpdateHistoryData.DeleteBulk(x => x.ContentId == id);
            var datakeywordTempMovement = keywordTempMovementData.DeleteBulk(x => x.ContentId == id);
            var dataSearchEngineBotLog = searchEngineBotLogData.DeleteBulk(x => x.ContentId == id);
            var dataSearchEngineSubmitLog = searchEngineSubmitLogData.DeleteBulk(x => x.ContentId == id);
            var dataResultPart = contentPartData.DeleteBulk(x => x.ContentId == id);

            return DeleteByKey(id);
        }
    }
}
