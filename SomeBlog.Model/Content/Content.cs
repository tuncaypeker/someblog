namespace SomeBlog.Model
{
    using SomeBlog.Model.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Content : Core.ModelBase, IValidatableObject
    {
        public Content()
        {
            HasFeatured = false;
            CreateDate = DateTime.Now;
            PublishDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            IsBotContent = false;
            HasUpdatedAfterBotInsert = false;
            Hit = 0;
            IsIndexed = false;
            AccessibilityScore = 0;
            BestPracticesScore = 0;
            LastAuditDate = new DateTime(1970, 1, 1);
            PerformanceScore = 0;
            SeoScore = 0;
            LastGoogleBotDate = new DateTime(1970, 1, 1);
            IsIndexedCheckDate = new DateTime(1970, 1, 1);
            CreatedById = -1;
            UpdatedById = -1;
            Tags = "";
            IsDelete = false;
            SourcePath = "";
            FocusKeyword = "";
        }

        public int BlogId { get; set; }
        public string Excerpt { get; set; }
        public int Hit { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public string Slug { get; set; }
        public string Slug2 { get; set; }
        public string FocusKeyword { get; set; }
        public bool HasFeatured { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PublishDate { get; set; }
        public int? FeaturedMediaId { get; set; }
        public virtual Media FeaturedMedia { get; set; }
        public bool IsBotContent { get; set; }

        public decimal AccessibilityScore { get; set; }
        public decimal BestPracticesScore { get; set; }
        public decimal PerformanceScore { get; set; }
        public decimal SeoScore { get; set; }
        public DateTime? LastAuditDate { get; set; }
        /// <summary>
        /// Bu içerik yazılırken hangi kaynaktan faydalanıldı
        /// </summary>
        public string SourcePath { get; set; }
        /// <summary>
        /// Bot icerigi ekledikten sonra manuel kontrol gerekiyor, bu yapildi mi
        /// </summary>
        public bool HasUpdatedAfterBotInsert { get; set; }
        public virtual List<ContentCategory> ContentCategories { get; set; }
        public virtual List<ContentTag> ContentTags { get; set; }
        public DateTime LastGoogleBotDate { get; set; }
        public int CreatedById { get; set; }
        public int UpdatedById { get; set; }
        public bool IsIndexed { get; set; }
        public DateTime IsIndexedCheckDate { get; set; }

        /// <summary>
        /// FormAdd = 1,
        /// BotAdd = 2,
        /// PoolContentInject = 3,
        /// PoolContentImport = 4,
        /// PAAContentGenerate = 5
        /// </summary>
        public ContentAddType AddType { get; set; }
        public int LastMonthHit { get; set; }
        public int Last3MonthsHit { get; set; }
        public int Last6MonthsHit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PageTitle))
                yield return new ValidationResult("PageTitle Gerekli.", new[] { nameof(PageTitle) });
            else if (PageTitle.Length > 500)
                yield return new ValidationResult("PageTitle 500 Karakterden Uzun Olamaz.", new[] { nameof(PageTitle) });
        }

        public static Content FromPoolContent(PoolContent poolContent)
        {
            return new Content()
            {
                BlogId = -1,
                ContentCategories = new List<ContentCategory>(),
                ContentTags = new List<ContentTag>(),
                CreateDate = poolContent.CreateDate,
                Excerpt = poolContent.Excerpt,
                FocusKeyword = "",
                HasFeatured = false,
                Hit = 0,
                IsActive = true,
                IsDelete = false,
                MetaDescription = poolContent.Excerpt,
                MetaTitle = poolContent.Title,
                PageDescription = poolContent.Excerpt,
                PageTitle = poolContent.Title,
                PublishDate = poolContent.Date,
                Slug = poolContent.Slug,
                Tags = "",
                Text = poolContent.Content,
                UpdateDate = DateTime.Now
            };
        }
    }
}
