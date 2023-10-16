namespace SomeBlog.Model
{
    public class RegistrarAccount : Core.ModelBase
    {
        public int RegistrarId { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
