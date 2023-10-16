using System;

namespace SomeBlog.Model
{
    public class FeedlyTopic : Core.ModelBase
    {
        public string Name { get; set; }

        /// <summary>
        /// Latin dilleri icin genelde Name ile ayni olurken, diger dillerde farklilasiyor, api ikisine de ayni cevabi veriyor
        /// </summary>
        public string TopicId { get; set; }
        public int ParentId { get; set; }
        public DateTime Updated { get; set; }
    }
}
