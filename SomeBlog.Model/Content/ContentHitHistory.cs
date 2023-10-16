namespace SomeBlog.Model
{
    using System;

    public class ContentHitHistory : Core.ModelBase
    {
        public int BlogId { get; set; }
        public int ContentId { get; set; }
        public int Hit { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
