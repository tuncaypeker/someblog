using System;

namespace SomeBlog.Model
{
    public class InstagramPoolAccount : Core.ModelBase
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Pk { get; set; }
        public string ProfilePicture { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }
        public string About { get; set; }
        public int Followers { get; set; }
        public int Followings { get; set; }
        public int Medias { get; set; }
        public string LangCode { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ImportDate { get; set; }
        public bool HasFirstImportDone { get; set; }
        public bool ShouldInsertNewContents { get; set; }
        public int FirstImportMaxPostCount { get; set; }
    }
}
