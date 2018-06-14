using System;
using NUglify;
using NUglify.JavaScript;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// Javascript minification.
    /// </summary>
    public class NUglifyJs
    {
        private CodeSettings _codeSettings;

        public NUglifyJs(NUglifyJsOptions options)
        {
            if (options == null) options = new NUglifyJsOptions();
            _codeSettings = options.CodeSettings;
        }

        /// <summary>
        /// Will minify given input, throws ArgumentException if Uglify reports any error.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Apply(string filename, string input) 
        {
            var result = Uglify.Js(input, filename, _codeSettings);
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
