namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class Response<T>
    {
        public Response()
        {

        }

        public Response(bool isSucceed, string message)
        {
            this.IsSucceed = isSucceed;
            this.Message = message;
        }

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
