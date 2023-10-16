using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Ahrefs.Dto
{
    public class GetOrganicKeywordsResponseDto
    {
        public int CurrentPage { get; set; }
        public int RowCount { get; set; }
        public List<GetOrganicKeywordRowDto> Rows { get; set; }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }

    public class GetOrganicKeywordRowDto
    {
        public string Language { get; set; }
        public int BestPosition { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Url { get; set; }
        public int Cpc { get; set; }
        public string BestPositionKind { get; set; }
        public int Traffic { get; set; }
        public int Difficulty { get; set; }
        public string Keyword { get; set; }
        public int Volume { get; set; }
    }
}
