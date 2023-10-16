using System;

namespace SomeBlog.Model
{
    /// <summary>
    /// Publish Date geldiginde ilgili content'in sonuna eklenir
    /// Bunu dashboard'da bulunan bir tane job yapar
    /// </summary>
    public class ContentPart : Core.ModelBase
    {
        public int ContentId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishDate { get; set; }

        public bool HasPublished { get; set; }
        public string Content { get; set; }
    }
}
