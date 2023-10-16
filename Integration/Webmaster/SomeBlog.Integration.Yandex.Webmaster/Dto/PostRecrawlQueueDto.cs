using Newtonsoft.Json;

namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class PostRecrawlQueueDto
    {
        public string error_code { get; set; }

        [JsonProperty("task_id")]
        public string TaskId { get; set; }

        [JsonProperty("quota_remainder")]
        public int QuotaReminder { get; set; }
    }
}
