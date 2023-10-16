using System;

namespace SomeBlog.Model
{
    public class KeywordSerp : Core.ModelBase
    {
        public int KeywordId { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int PageAuthority { get; set; }
        public int DomainAuthority { get; set; }
        public int LinkingDomainsToPage { get; set; }
        public int LinkingDomainsToDomain { get; set; }
        public DateTime CreateDate { get; set; }

        //SomeBlog.Model.Enums.SeoTools
        public int Source { get; set; }
    }
}
