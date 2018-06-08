using System;
using NUglify.Css;
using NUglify.JavaScript;

namespace StaticFileTransform.NUglify
{
    public class NUglifyCssOptions
    {
        public CssSettings CssSettings { get; set; } = null;
        public CodeSettings CodeSettings { get; set; } = null;
    }
}