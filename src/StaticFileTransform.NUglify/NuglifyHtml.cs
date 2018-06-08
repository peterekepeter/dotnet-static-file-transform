using System;
using NUglify;
using NUglify.Html;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// HTML minification.
    /// </summary>
    public class NUglifyHtml
    {
        private readonly NUglifyHtmlOptions _options;

        public NUglifyHtml(NUglifyHtmlOptions options)
        {
            if (options == null) options = new NUglifyHtmlOptions();
            _options = options;
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
        
    }
}
