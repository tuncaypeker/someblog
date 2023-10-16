using System;

namespace SomeBlog.Model
{
    public class CustomBotPostHistory : Core.ModelBase
    {
       public int CustomBotId { get; set; }
        public int BlogId { get; set; }
        
        /// <summary>
        /// Post Slug
        /// </summary>
        public string Slug { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
