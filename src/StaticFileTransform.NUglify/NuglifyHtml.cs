using System;
using NUglify;
using NUglify.Html;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// HTML minification.
    /// </summary>
    public class NUglifyHtml: ITextFileTransform
    {
        private readonly NUglifyHtmlOptions _options;
        private readonly Func<String, Boolean> _matcher;

        public NUglifyHtml(NUglifyHtmlOptions options)
        {
            if (options == null) options = new NUglifyHtmlOptions();
            _options = options;
            if (_options.FileMatcher != null) _matcher = _options.FileMatcher;
            else _matcher = filename => filename.EndsWith(".html") || filename.EndsWith(".htm");
        }

        /// <summary>
        /// Will minify given input, throws ArgumentException if Uglify reports any error.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Apply(string filename, string input) 
        {
            var result = Uglify.Html(input, _options.HtmlSettings, filename);
            if (result.HasErrors)
            {
                var exception = new ArgumentException($"UglifyJavaScript failed {filename}");
                exception.Data.Add("Result", result);
                throw exception;
            }
            return result.Code;
        }

        public bool Matches(string filename) => _matcher(filename);

        public double Priority => _options.Priority;
    }
}
