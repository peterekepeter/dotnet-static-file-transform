using System;
using System.Collections.Generic;
using StaticFileTransform.Abstractions;
using StaticFileTransform.Implementation;

namespace StaticFileTransform.Internal
{
    internal class ContentProvider : IContentProvider
    {
        private readonly Func<string, string> _loader;
        private readonly Dictionary<string, string> _loaded;

        public ContentProvider(Func<string, string> loader)
        {
            _loader = loader;
            _loaded = new Dictionary<string, string>();
        }

        public string GetContent(string filename)
        {
            if (_loaded.TryGetValue(filename, out var content)) return content;
            content = _loader(filename);
            _loaded.Add(filename, content);
            return content;
        }
    }
}
