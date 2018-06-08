using System;

namespace StaticFileTransform.dotless
{
    public class DotlessOptions
    {
        /// <summary>
        /// Keep first comment begining /**
        /// </summary>
        public bool KeepFirstSpecialComment { get; set; } = false;

        /// <summary>
        /// Disable using parameters
        /// </summary>
        public bool DisableParameters { get; set; } = false;

        /// <summary>
        /// You need to set this to support imports.
        /// This corresponds to 'rootpath' option of lessc. (default "")
        /// </summary>
        public string RootPath { get; set; } = "";

        /// <summary>
        /// Inlines css files into the less output (default false)
        /// </summary>
        public bool InlineCssFiles { get; set; } = false;

        /// <summary>
        /// Import all files (even if ending in .css) as less files (default false)
        /// </summary>
        public bool ImportAllFilesAsLess { get; set; } = false;

        /// <summary>
        /// Whether to minify the ouput (default false)
        /// </summary>
        public bool MinifyOutput { get; set; } = true;

        /// <summary>
        /// Prints helpful comments in the output while debugging (default false)
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        ///  Optimisation int
        ///  0 - do not chunk up the input
        ///  > 0 - chunk up output
        ///  
        ///  Recommended value - 1 (by default its 1)
        /// </summary>
        public int Optimization { get; set; } = 1;

        /// <summary>
        /// Whether to only evaluate mathematical expressions when they are wrapped in an extra set of parentheses. (default false)
        /// </summary>
        public bool StrictMath { get; set; } = false;

    }
}