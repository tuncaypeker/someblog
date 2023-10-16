namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogForm : Core.ModelBase
    {
        public int BlogId { get; set; }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreatedById { get; set; }

        [NotMapped]
        public int FillCount { get; set; }
    }
}
