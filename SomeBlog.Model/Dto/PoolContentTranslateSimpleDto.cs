namespace SomeBlog.Model.Dto
{
    public class PoolContentTranslateSimpleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int PoolLanguageId { get; set; }
        public string BlogName { get; set; }
        public int BlogId { get; set; }
    }
}
