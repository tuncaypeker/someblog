using System;

namespace SomeBlog.Model.Dto
{
    public class CalendarContentDto
    {
        public string title { get; set; }
        public string start { get; set; }
        public string slug { get; set; }

        /// <summary>
        /// view tarafinda dolacak
        /// </summary>
        public string url { get; set; }
        public string color { get; set; }

        public int id { get; set; }
        public bool isActive { get; set; }
    }
}
