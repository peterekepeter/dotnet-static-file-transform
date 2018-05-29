using StaticFileTransform.Abstractions;
using System;

namespace StaticFileTransform
{
    /// <summary>
    /// Text transformation which can be implemented with lambda functions.
    /// </summary>
    public class CustomTextTransform : ITextFileTransform
    {
        private Func<String, String, String> _transform;
        private Func<String, Boolean> _matcher;
        private readonly double _priority;

        public CustomTextTransform(Func<String, Boolean> matcher, Func<String, String, String> transform, Double priority = 10.0)
        {
            _transform = transform;
            _matcher = matcher;
            _priority = priority;
        }

        public double Priority => _priority;

        public string Apply(string filename, string input) => _transform(filename, input);

        public bool Matches(string filename) => _matcher(filename);
    }
}
