using System;
using NUglify.Css;
using NUglify.JavaScript;

namespace StaticFileTransform.NUglify
{
    public class NUglifyCssOptions
    {
        public double Priority { get; set; } = StaticFileTransform.DefaultPriority.Minifier;
        public CssSettings CssSettings { get; set; } = null;
        public CodeSettings CodeSettings { get; set; } = null;
        public Func<string, bool> FileMatcher { get; set; }
    }
}