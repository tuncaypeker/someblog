namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogMail : Core.ModelBase
    {
        public int BlogId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pop3Address { get; set; }
        public int Pop3Port { get; set; }
    }
}
