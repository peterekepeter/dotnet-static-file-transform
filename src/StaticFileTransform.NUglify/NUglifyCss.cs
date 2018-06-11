using System;
using NUglify;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// Css minification.
    /// </summary>
    public class NUglifyCss
    {
        private readonly NUglifyCssOptions _options;

        public NUglifyCss(NUglifyCssOptions options)
        {
            if (options == null) options = new NUglifyCssOptions();
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
            var result = Uglify.Css(input, filename, _options.CssSettings, _options.CodeSettings);
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
