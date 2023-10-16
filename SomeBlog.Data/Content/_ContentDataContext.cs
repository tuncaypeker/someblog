namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using SomeBlog.Data.Infrastructure;

    public class ContentDataContext : DbContext
    {
        public ContentDataContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySQL(connectionString).Options)
        {
        }

        public DbSet<Model.ContentAudit> ContentAudits { get; set; }
        public DbSet<Model.ContentCategory> ContentCategories { get; set; }
        public DbSet<Model.Content> Contents { get; set; }

        public DbSet<Model.ContentKeywordTail> ContentKeywordTails { get; set; }
        public DbSet<Model.ContentUpdateHistory> ContentUpdateHistories { get; set; }
        
        public DbSet<Model.ContentMeta> ContentMetas { get; set; }
        public DbSet<Model.ContentMedia> ContentMedias { get; set; }
        public DbSet<Model.ContentTag> ContentTags { get; set; }
        public DbSet<Model.ContentStartTemplate> ContentStartTemplates { get; set; }
        public DbSet<Model.ContentStartTemplateCategory> ContentStartTemplateCategories { get; set; }
        
        public DbSet<Model.ContentLighthouseAudit> ContentLighthouseAudits { get; set; }
        public DbSet<Model.ContentHitHistory> ContentHistoryLogs { get; set; }
        public DbSet<Model.ContentPart> ContentParts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model.ContentStartTemplate>(entity => entity.ToTable("content_start_templates"));
            builder.Entity<Model.ContentStartTemplateCategory>(entity => entity.ToTable("content_start_template_categories"));
            builder.Entity<Model.ContentKeywordTail>(entity => entity.ToTable("content_keyword_tails"));
            builder.Entity<Model.Content>(entity => entity.ToTable("contents"));
            builder.Entity<Model.ContentPart>(entity => entity.ToTable("content_parts"));
            builder.Entity<Model.ContentUpdateHistory>(entity => entity.ToTable("content_update_histories"));
            builder.Entity<Model.ContentMedia>(entity => entity.ToTable("content_medias"));
            builder.Entity<Model.ContentLighthouseAudit>(entity => entity.ToTable("content_lighthouse_audits"));
            builder.Entity<Model.ContentAudit>(entity => entity.ToTable("content_audits"));
            builder.Entity<Model.ContentCategory>(entity => entity.ToTable("content_categories"));
            builder.Entity<Model.ContentMeta>(entity => entity.ToTable("content_metas"));
            builder.Entity<Model.ContentTag>(entity => entity.ToTable("content_tags"));
            builder.Entity<Model.ContentHitHistory>(entity => entity.ToTable("content_hit_histories"));

            //related from other schema
            builder.Entity<Model.Media>(entity => entity.ToTable("medias","db_someblog"));
            builder.Entity<Model.Category>(entity => entity.ToTable("categories","db_someblog"));
            builder.Entity<Model.Blog>(entity => entity.ToTable("blogs","db_someblog"));
            builder.Entity<Model.Meta>(entity => entity.ToTable("metas","db_someblog"));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }
    }
}