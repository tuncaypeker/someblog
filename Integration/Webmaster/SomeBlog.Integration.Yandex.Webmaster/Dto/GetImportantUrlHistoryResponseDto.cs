using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class ImportantUrlHistoryIndexingStatus
    {
        public string status { get; set; }
        public int http_code { get; set; }
        public DateTime access_date { get; set; }
    }

    public class ImportantUrlHistorySearchStatus
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime last_access { get; set; }
        public object excluded_url_status { get; set; }
        public object bad_http_status { get; set; }
        public bool searchable { get; set; }
        public object target_url { get; set; }
    }

    public class History
    {
        public string url { get; set; }
        public DateTime update_date { get; set; }
        public List<object> change_indicators { get; set; }
        public ImportantUrlHistoryIndexingStatus indexing_status { get; set; }
        public ImportantUrlHistorySearchStatus search_status { get; set; }
    }

    public class GetImportantUrlHistoryResponseDto
    {
        public List<History> history { get; set; }
    }
}
