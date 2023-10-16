using System;

namespace SomeBlog.Model
{
    public class SearchHistory : Core.ModelBase
    {
        public string Query { get; set; }
        public int BlogId { get; set; }
        public DateTime CreateDate { get; set; }
        public int ResultCount { get; set; }
    }
}
