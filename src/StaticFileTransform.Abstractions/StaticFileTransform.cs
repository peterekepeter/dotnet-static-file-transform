using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticFileTransform.Abstractions
{
    public class StaticFileTransform : IStaticFileTransform
    {
        private readonly Func<String, IContentProvider, String> transform;
        private readonly Func<String, Boolean> matcher;
        private readonly int priority;

        public StaticFileTransform(Func<string, bool> matcher, Func<string, IContentProvider, string> transform, int priority)
        {
            this.matcher = matcher;
            this.transform = transform;
            this.priority = priority;
        }

        public string Apply(string filename, IContentProvider provider) => transform(filename, provider);

        public bool Matches(string filename) => matcher(filename);

        public int Priority => priority;

    }
}
