namespace SomeBlog.Model
{
    using SomeBlog.Model.Core;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogFormItem : Core.ModelBase
    {
        public int BlogFormId { get; set; }

        public string Label { get; set; }
        public string Placeholder { get; set; }
        public int Type { get; set; }
        public bool HasRequired { get; set; }

        //Sıralama işlemi yapar en küçük olan üstte çıkar
        public int Order { get; set; }
    }
}
