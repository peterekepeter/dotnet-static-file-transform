using System;
using System.Collections.Generic;
using System.Text;

namespace StaticFileTransform.Abstractions
{
    public interface IContentProvider
    {
        /// <summary>
        /// Get content of a file, with all transformations added.
        /// Should be called on a different file to avoid recursion.
        /// </summary>
        string GetContent(string filename);
    }
}
