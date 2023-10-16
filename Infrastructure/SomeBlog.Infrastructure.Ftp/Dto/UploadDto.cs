namespace SomeBlog.Infrastructure.Ftp.Dto
{
    public class UploadDto
    {
        public string ImagePath { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileWidth { get; set; }
        public int FileHeight { get; set; }
        public int FileQuality { get; set; }
        public string FileResize { get; set; }
    }
}
