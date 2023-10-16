using System.Collections.Generic;

namespace SomeBlog.CssOptimizer
{
    /// <summary>
    /// https://github.com/thoqbk/css-optimizer
    /// </summary>
    public class CSSMediaRule : NonCSSStyleRule
    {
        private readonly List<CSSRule> children = new List<CSSRule>();

        public CSSMediaRule(string content)
            : base(content)
        {
        }

        public List<CSSRule> getChildren()
        {
            return children;
        }
    }
}
