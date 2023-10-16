using System;

namespace SomeBlog.Model.Core
{
    public class IgnoredAttribute : System.Attribute
    {
        public string SomeProperty { get; set; }
    }
}
