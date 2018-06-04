using StaticFileTransform.Abstractions;
using System;

namespace StaticFileTransform
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

        public string Apply(string content, string filename, IContentProvider provider)
        {
            provider.GetTransformedFileContent();
        };

        public bool Matches(string filename) => _matcher(filename);
    }
}
