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

        /// <summary>
        /// Transformations are sorted by priority.
        /// </summary>
        public Double Priority { get; set; } = StaticFileTransform.DefaultPriority.Minifier;

        /// <summary>
        /// Determines which files to transform. By default all files that end with ".js"
        /// </summary>
        public Func<String, Boolean> FileMatcher { get; set; } = null;
    }
}