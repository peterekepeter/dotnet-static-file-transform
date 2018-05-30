using StaticFileTransform.Abstractions;
using System;
using System.Collections.Generic;

namespace StaticFileTransform.Internal
{
    public static class Extensions
    {
        public static List<ITextFileTransform> MatchingTransformations(this IEnumerable<ITextFileTransform> transforms, string subpath)
        {
            var list = new List<ITextFileTransform>();
            foreach (var transform in transforms)
            {
                if (transform.Matches(subpath))
                {
                    list.Add(transform);
                }
                list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            }
            return list;
        }

        public static String ApplyAll(this IEnumerable<ITextFileTransform> transforms, string subpath, string content)
        {
            var list = new List<ITextFileTransform>();
            foreach (var transform in transforms)
            {
                content = transform.Apply(subpath, content);
            }
            return content;
        }
    }
}
