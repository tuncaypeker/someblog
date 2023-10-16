namespace SomeBlog.Model
{
    /// <summary>
    /// Her User her blogu duzenleyemez, bu model ile yetki verilir
    /// </summary>
    public class UserBlog : Core.ModelBase
    {
        public int UserId { get; set; }
        public int BlogId { get; set; }
    }
}
