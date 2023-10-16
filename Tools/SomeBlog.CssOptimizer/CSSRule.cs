namespace SomeBlog.CssOptimizer
{
    public abstract class CSSRule
    {
        public string Content { get; }
        public string Selector { get; set; }

        public CSSRule(string content)
        {
            this.Content = content;
        }
    }
}
