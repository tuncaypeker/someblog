namespace SomeBlog.Infrastructure.Ftp.Dto
{
    public class FtpCredentialsDto
    {
        public FtpCredentialsDto(string address, int port, string username, string password)
        {
            Address = address;
            Port = port;
            Username = username;
            Password = password;
        }

        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
