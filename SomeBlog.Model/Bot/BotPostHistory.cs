using System;

namespace SomeBlog.Model
{
    /// <summary>
    /// hangi siteye hangi siteden hangi post ne zmn eklenmis
    /// </summary>
    public class BotPostHistory : Core.ModelBase
    {
        public int BotId { get; set; }
        public int BlogId { get; set; }
        
        /// <summary>
        /// Post Slug
        /// </summary>
        public string Slug { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
