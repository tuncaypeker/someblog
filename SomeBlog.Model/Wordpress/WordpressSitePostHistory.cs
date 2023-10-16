using System;

namespace SomeBlog.Model
{
    public class WordpressSitePostHistory : Core.ModelBase
    {
        public int WordpressSiteId { get; set; }

        public int RemoteId { get; set; }
        public int LocalId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string RemoteSitePath { get; set; }
        public string RemotePostUrl { get; set; }
        public string LocalPostUrl { get; set; }
    }
}
