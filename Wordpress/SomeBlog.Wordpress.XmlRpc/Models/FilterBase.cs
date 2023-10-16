namespace SomeBlog.Wordpress.XmlRpc.Models
{
    public abstract class FilterBase
    {
        public int Number { get; set; }
        protected FilterBase()
        {
            Number = int.MaxValue;
        }
    }
}
