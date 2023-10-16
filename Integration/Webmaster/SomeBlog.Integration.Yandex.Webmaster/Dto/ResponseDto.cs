namespace SomeBlog.Integration.Yandex.Webmaster.Dto
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        public bool IsSucceded { get; set; }
        public string Message { get; set; }
    }
}
