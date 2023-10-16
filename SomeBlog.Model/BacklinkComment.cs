using System;

namespace SomeBlog.Model
{
    public class BacklinkComment : Core.ModelBase
    {
        public string SitePath { get; set; }
        public string PostPath { get; set; }
        public int PostId { get; set; }

        public string Comment { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }

        public DateTime Created { get; set; }
    }
}
