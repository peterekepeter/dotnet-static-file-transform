using System;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.Implementation
{
    /// <summary>
    /// Text transformation which can be implemented with lambda functions.
    /// </summary>
    public class TextTransform : ITransformationPriority
    {
        private readonly Func<String, String> _transform;
        private readonly Func<String, Boolean> _matcher;

        public TextTransform(Func<String, Boolean> matcher, Func<String, String> transform, Double priority = 10.0)
        {
            _matcher = matcher;
            _transform = transform;
            Priority = priority;
        }

        public double Priority { get; }

        public string Apply(string content, string filename, IContentProvider provider) => _transform(content);

        public bool Matches(string filename) => _matcher(filename);
    }
}
