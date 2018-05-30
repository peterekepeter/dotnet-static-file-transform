using System;
using NUglify;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// Css minification.
    /// </summary>
    public class NUglifyCss: ITextFileTransform
    {
        private readonly NUglifyCssOptions _options;
        private readonly Func<String, Boolean> _matcher;

        public NUglifyCss(NUglifyCssOptions options)
        {
            if (options == null) options = new NUglifyCssOptions();
            _options = options;
            if (_options.FileMatcher != null) _matcher = _options.FileMatcher;
            else _matcher = filename => filename.EndsWith(".css");
        }

        /// <summary>
        /// Will minify given input, throws ArgumentException if Uglify reports any error.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Apply(string filename, string input) 
        {
            var result = Uglify.Css(input, filename, _options.CssSettings, _options.CodeSettings);
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
