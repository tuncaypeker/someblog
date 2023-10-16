﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class KeywordVariationsResult
    {
        public int id { get; set; }
        public string phrase { get; set; }
        public string database { get; set; }
        public int volume { get; set; }
        public double cpc { get; set; }
        public double competition_level { get; set; }
        public int? difficulty { get; set; }
        public object rds_median { get; set; }
        public int? results { get; set; }
        public List<int> serp_features { get; set; }
        public List<int> trend { get; set; }
        public string trend_base_date { get; set; }
        public string snapshot_date { get; set; }
        public int update_status { get; set; }
        public int status_updated_at { get; set; }
        public string serp_id { get; set; }
        public int serp_updated_at { get; set; }
    }

    public class KeywordVariationsDto
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }

        [JsonProperty("Result")]
        public List<KeywordVariationsResult> result { get; set; }
    }
}
