using System;

namespace StaticFileTransform.Jsx
{
    /// <summary>
    /// Allows Parsing JSX code.
    /// </summary>
    public interface IJsxParser
    {
        /// <summary>
        /// Parses input JSX code and returns plain old javascript code.
        /// </summary>
        /// <param name="inputJsx">Required JSX input, see https://facebook.github.io/jsx/ for quick reference.</param>
        /// <param name="options">Optional parser options, can be null.</param>
        /// <returns></returns>
        string ParseAndCompileToJs(string inputJsx, JsxParserOptions options = null);
    }
}
