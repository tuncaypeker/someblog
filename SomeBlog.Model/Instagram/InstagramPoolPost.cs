using System;

namespace SomeBlog.Model
{
    public class InstagramPoolPost : Core.ModelBase
    {
        public int PoolAccountId { get; set; }
        public string MediaId { get; set; }
        public string Code { get; set; }
        public bool IsVideo { get; set; }
        public bool IsMultiple { get; set; }
        public string Caption { get; set; }
        public string Location { get; set; }
        public string LocalImagePath { get; set; }
        public string LocalVideoPath { get; set; }
        public string RemoteImagePath { get; set; }
        public string RemoteVideoPath { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string LikeCount { get; set; }
        public string CommentCount { get; set; }
    }
}
