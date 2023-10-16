using System;

namespace SomeBlog.Model
{
    public class Comment : Core.ModelBase
    {
        public Comment()
        {
            Published = new DateTime(1970, 1, 1);
        }

        public int BlogId { get; set; }
        public int ContentId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsApproved { get; set; }
        public DateTime Created { get; set; }
        public DateTime Published { get; set; }
        public string VideoId { get; set; }
        public string VideoCommentId { get; set; }
    }
}
