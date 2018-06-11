using System;
using System.Collections.Generic;
using System.Text;

namespace StaticFileTransform.Abstractions
{
    public static class ContentProviderExtensions
    {
        /// <summary>
        /// Check if a file originally existed.
        /// </summary>
        public static bool FileExists(this IContentProvider provider, string filename)
            => provider.GetContent(filename) != null;
    }
}
