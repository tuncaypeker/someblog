using System.Collections.Generic;

namespace SomeBlog.CssOptimizer
{
    public class CSSStyleRule : CSSRule
    {
        public HashSet<string> ClassNames { get; set; }
        public HashSet<string> TagNames { get; set; }
        public HashSet<string> IdNames { get; set; }

        public CSSStyleRule(string content)
            : base(content)
        {
            ClassNames = new HashSet<string>();
            TagNames = new HashSet<string>();
            IdNames = new HashSet<string>();
        }
    }
}
