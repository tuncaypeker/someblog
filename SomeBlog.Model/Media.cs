using System;

namespace SomeBlog.Model
{
    public class Media : Core.ModelBase
    {
        public Media()
        {
            CreateDate = DateTime.Now;
            Alt = "";
            Title = "";
        }

        public int BlogId { get; set; }

        /// <summary>
        /// Image = 1 
        /// Video = 2
        /// </summary>
        public int Type { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }

        public string MediaUrl { get; set; }
        public string ImageSlug { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedById { get; set; }
        public int UpdatedById { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool HasWebp { get; set; }
        public bool HasAvif { get; set; }
    }
}
