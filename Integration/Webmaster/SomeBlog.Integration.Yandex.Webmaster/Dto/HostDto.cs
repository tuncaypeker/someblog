namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class HostDto
    {
        public string host_id { get; set; }
        public string ascii_host_url { get; set; }
        public string unicode_host_url { get; set; }
        public bool verified { get; set; }
        public object main_mirror { get; set; }
        public string host_data_status { get; set; }
        public string host_display_name { get; set; }
    }
}
