using StaticFileTransform.Abstractions;
using System;
using System.Collections.Generic;

namespace StaticFileTransform.Internal
{
    public static class Extensions
    {
        public static List<IStaticFileTransform> MatchingTransformations(this IEnumerable<IStaticFileTransform> transforms, string subpath)
        {
            var list = new List<IStaticFileTransform>();
            foreach (var transform in transforms)
            {
                if (transform.Matches(subpath))
                {
                    list.Add(transform);
                }
                list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }
            return list;
        }
        
    }
}
