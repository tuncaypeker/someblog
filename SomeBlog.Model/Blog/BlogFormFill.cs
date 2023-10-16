namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogFormFill : Core.ModelBase
    {
        public int BlogFormId { get; set; }

        public string IpAddress { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
