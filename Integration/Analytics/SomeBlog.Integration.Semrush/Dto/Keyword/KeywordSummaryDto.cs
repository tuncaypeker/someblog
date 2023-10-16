using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class KeywordSummaryDto
    {
        public List<KeywordVolumeDto> KeywordVolumesByCountry { get; set; }
        public KeywordVariationsDto KeywordVariations { get; set; }
        public KeywordSerpDto KeywordSerps { get; set; }
        public KeywordRelatedDto KeywordRelateds { get; set; }
    }
}
