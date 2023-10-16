namespace SomeBlog.Data
{
    using Microsoft.EntityFrameworkCore;
    using SomeBlog.Data.Infrastructure;

    public class PoolDataContext : DbContext
    {
        public PoolDataContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySQL(connectionString).Options)
        {
        }

        public DbSet<Model.PoolBlog> PoolBlogs { get; set; }
        public DbSet<Model.PoolCategory> PoolCategories { get; set; }
        public DbSet<Model.PoolContent> PoolContents { get; set; }
        public DbSet<Model.PoolContentCategory> PoolContentCategories { get; set; }
        public DbSet<Model.PoolContentTranslate> PoolContentTranslates { get; set; }
        public DbSet<Model.PoolContentTag> PoolContentTags { get; set; }
        public DbSet<Model.PoolMedia> PoolMedias { get; set; }
        public DbSet<Model.PoolTag> PoolTags { get; set; }
        public DbSet<Model.PoolContentUsage> PoolContentUsages { get; set; }
        public DbSet<Model.PoolSubject> PoolSubjects { get; set; }
        public DbSet<Model.PoolBlogSubject> PoolBlogSubjects { get; set; }
        public DbSet<Model.PoolBlogImportRequest> PoolBlogImportRequests { get; set; }
        public DbSet<Model.PoolBlogImportRequestLog> PoolBlogImportRequestLogs { get; set; }

        public DbSet<Model.PoolBlogImportRequestRemoveRule> PoolBlogImportRequestRemoveRules { get; set; }
        public DbSet<Model.PoolBlogImportRequestReplaceRule> PoolBlogImportRequestReplaceRules { get; set; }

        public DbSet<Model.PoolLanguage> PoolBlogLanguages { get; set; }
        public DbSet<Model.PoolContentComment> PoolContentComments { get; set; }
        public DbSet<Model.PoolBlogUpdateLog> PoolBlogUpdateLogs { get; set; }
        public DbSet<Model.PoolBlogUpdateLogItem> PoolBlogUpdateLogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model.PoolBlog>(entity => entity.ToTable("pool_blogs"));
            builder.Entity<Model.PoolCategory>(entity => entity.ToTable("pool_categories"));
            builder.Entity<Model.PoolContent>(entity => entity.ToTable("pool_contents"));
            builder.Entity<Model.PoolContentCategory>(entity => entity.ToTable("pool_content_categories"));
            builder.Entity<Model.PoolContentTag>(entity => entity.ToTable("pool_content_tags"));
            builder.Entity<Model.PoolMedia>(entity => entity.ToTable("pool_medias"));
            builder.Entity<Model.PoolTag>(entity => entity.ToTable("pool_tags"));
            builder.Entity<Model.PoolContentUsage>(entity => entity.ToTable("pool_content_usages"));
            builder.Entity<Model.PoolContentTranslate>(entity => entity.ToTable("pool_content_translates"));
            builder.Entity<Model.PoolSubject>(entity => entity.ToTable("pool_subjects"));
            builder.Entity<Model.PoolBlogSubject>(entity => entity.ToTable("pool_blog_subjects"));
            builder.Entity<Model.PoolBlogImportRequest>(entity => entity.ToTable("pool_blog_import_requests"));
            builder.Entity<Model.PoolLanguage>(entity => entity.ToTable("pool_languages"));
            builder.Entity<Model.PoolBlogImportRequestLog>(entity => entity.ToTable("pool_blog_import_request_logs"));
            builder.Entity<Model.PoolBlogImportRequestRemoveRule>(entity => entity.ToTable("pool_blog_import_request_remove_rules"));
            builder.Entity<Model.PoolBlogImportRequestReplaceRule>(entity => entity.ToTable("pool_blog_import_request_replace_rules"));
            builder.Entity<Model.PoolBlogUpdateLog>(entity => entity.ToTable("pool_blog_update_logs"));
            builder.Entity<Model.PoolBlogUpdateLogItem>(entity => entity.ToTable("pool_blog_update_log_items"));
            builder.Entity<Model.PoolContentComment>(entity => entity.ToTable("pool_content_comments"));

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