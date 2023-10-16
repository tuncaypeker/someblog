using System;

namespace SomeBlog.Model
{
    public class InstagramPoolPostImage : Core.ModelBase
    {
        public int PoolPostId { get; set; }
        public string LocalImagePath { get; set; }
        public string RemoteImagePath { get; set; }
    }
}
