namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogFormFillValue : Core.ModelBase
    {
        public int BlogFormId { get; set; }
        public int BlogFormFillId { get; set; }
        public int BlogFormItemId { get; set; }
        public string Value { get; set; }
    }
}
