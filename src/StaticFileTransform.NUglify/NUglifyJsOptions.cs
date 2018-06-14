using System;
using NUglify.JavaScript;

namespace StaticFileTransform.NUglify
{
    /// <summary>
    /// Options avaiable for uglify.
    /// </summary>
    public class NUglifyJsOptions
    {
        /// <summary>
        /// Code settings passed on to NUglify
        /// </summary>
        public CodeSettings CodeSettings { get; set; } = null;
    }
}