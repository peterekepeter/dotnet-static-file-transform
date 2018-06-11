using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticFileTransform.Abstractions
{
    /// <summary>
    /// Allows transforming filenames based on patterms.
    /// </summary>
    public class FilenameTransform
    {
        private readonly Regex regex;
        private readonly string stitch;

        /// <summary>
        /// Create a transform based on match and replace pattern, for example:
        /// new FilenameTransform("*.html", "*.htm"); replaces html to htm
        /// </summary>
        /// <param name="matchPattern">Filename match pattern, for example *.css</param>
        /// <param name="replacePattern">Filename replace pattern for example *.less</param>
        public FilenameTransform(String matchPattern, String replacePattern)
        {
            regex = Helpers.PatternToRegex(matchPattern);
            stitch = Helpers.PatternToStitch(replacePattern);
        }

        /// <summary>
        /// Applies the transformation.
        /// </summary>
        public String TrasformFilename(String filename) => regex.Replace(filename, stitch);

        /// <summary>
        /// Check if pattern matches the filename.
        /// </summary>
        public bool Matches(string filename) => regex.IsMatch(filename);
    }
}
