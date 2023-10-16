using System;

namespace SomeBlog.Model
{
    public class InstagramPost : Core.ModelBase
    {
        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool HasPosted { get; set; }
        public string Caption { get; set; }
        public string MediaId { get; set; }
        public string Code { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string LocalImagePath { get; set; }
        public string LocalVideoPath { get; set; }
        public string RemoteImagePath { get; set; }
    }
}
