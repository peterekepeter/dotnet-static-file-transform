using System;
using NUglify.Html;

namespace StaticFileTransform.NUglify
{
    public class NUglifyHtmlOptions
    {
        public double Priority { get; set; } = StaticFileTransform.TransformationPriority.Minifier;
        public HtmlSettings HtmlSettings { get; set; } = null;
        public Func<string, bool> FileMatcher { get; set; } = null;
    }
}