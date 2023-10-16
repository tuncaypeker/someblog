using System;

namespace SomeBlog.Model
{
    public class BlogUptime : Core.ModelBase
    {
        public int BlogId { get; set; }

        /// <summary>
        /// Ftp or Http
        /// </summary>
        public string Protocol { get; set; }
        public DateTime StartTime { get; set; }
        public int Seconds { get; set; }

        /// <summary>
        /// Up or Down
        /// </summary>
        public string Status { get; set; }

        public bool HasNotified { get; set; }
        public DateTime LastNotified { get; set; }
    }
}
