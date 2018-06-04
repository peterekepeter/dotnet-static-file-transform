using StaticFileTransform.Abstractions;
using System;
using System.Collections.Generic;

namespace StaticFileTransform.Internal
{
    public static class Extensions
    {
        public static List<ITransformationPriority> MatchingTransformations(this IEnumerable<ITransformationPriority> transforms, string subpath)
        {
            var list = new List<ITransformationPriority>();
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

        public static String ApplyAll(this IEnumerable<ITransformationPriority> transforms, string subpath, string content)
        {
            var list = new List<ITransformationPriority>();
            foreach (var transform in transforms)
            {
                content = transform.Apply(subpath, content);
            }
            return content;
        }
    }
}
