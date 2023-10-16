using System;

namespace SomeBlog.Integration.Google.SearchConsole.Dto
{
    public class SitemapsListDto
    {
        public DateTime LastDownloaded { get; set; }
        public DateTime LastSubmitted { get; set; }
        public string Path { get; set; }
        public long? Warnings { get; set; }

        public bool IsPending { get; set; }
        public bool IsSitemapsIndex { get; set; }
    }
}
