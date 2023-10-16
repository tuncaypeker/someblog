using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.MozPro.Dto
{
    public class BacklinkAnalysisDto
    {
        [JsonProperty("idina_response")]
        public BacklinkAnalysisResponseDto Response { get; set; }
    }

    public class BacklinkAnalysisResponseDto
    {
        [JsonProperty("next_token")]
        public string NextToken { get; set; }

        [JsonProperty("links")]
        public List<BacklinkAnalysisLinkPairDto> Links { get; set; }
    }

    public class BacklinkAnalysisLinkPairDto
    {
        [JsonProperty("via_redirect")]
        public bool ViaRedirect { get; set; }

        [JsonProperty("date_last_seen")]
        public string DateLastSeen { get; set; }

        [JsonProperty("date_first_seen")]
        public string DateFirstSeen { get; set; }

        [JsonProperty("date_disappeared")]
        public string DateDisappeared { get; set; }

        [JsonProperty("anchor_text")]
        public string AnchorText { get; set; }

        [JsonProperty("target")]
        public BacklinkAnalysisLinkDto Target { get; set; }

        [JsonProperty("source")]
        public BacklinkAnalysisLinkDto Source { get; set; }
    }

    public class BacklinkAnalysisLinkDto
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("last_crawled")]
        public string LastCrawled { get; set; }

        [JsonProperty("spam_score")]
        public double SpamScore { get; set; }

        [JsonProperty("page_authority")]
        public int PageAuthority { get; set; }

        [JsonProperty("domain_authority")]
        public int DomainAuthority { get; set; }
    }
}
