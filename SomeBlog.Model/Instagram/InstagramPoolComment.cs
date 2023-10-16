using System;

namespace SomeBlog.Model
{
    public class InstagramPoolComment : Core.ModelBase
    {
        public int PoolPostId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
    }
}
