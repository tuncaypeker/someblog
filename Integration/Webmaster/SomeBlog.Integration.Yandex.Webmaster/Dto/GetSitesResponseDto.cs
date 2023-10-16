using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class GetSitesResponseDto
    {
        [JsonProperty("hosts")]
        public List<HostDto> Hosts { get; set; }
    }
}
