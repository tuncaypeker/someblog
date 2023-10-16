using Newtonsoft.Json;

namespace SomeBlog.Integration.MozPro.Dto
{
    public class SerpResultDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("page_authority")]
        public int PageAuthority { get; set; }

        [JsonProperty("domain_authority")]
        public int DomainAuthority { get; set; }

        [JsonProperty("linking_domains_to_page")]
        public int LinkingDomainsToPage { get; set; }

        [JsonProperty("linking_domains_to_domain")]
        public int LinkingDomainsToDomain { get; set; }
    }
}
