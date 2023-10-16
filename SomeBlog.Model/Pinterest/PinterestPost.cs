using System;

namespace SomeBlog.Model
{
    public class PinterestPost : Core.ModelBase
    {
        public PinterestPost()
        {
            Width = 0;
            Height = 0;
        }

        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool HasPosted { get; set; }
        public string Caption { get; set; }
        public string BoardId { get; set; }
        public string LocalImagePath { get; set; }
        public string RemoteImagePath { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string PinId { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
