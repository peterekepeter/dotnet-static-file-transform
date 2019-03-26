namespace StaticFileTransform.Jsx
{
    /// <summary>
    /// Contains options for IJsxParser
    /// </summary>
    public class JsxParserOptions
    {
        /// <summary>
        /// The name of the rendering function for javascript to use to create elements from JSX syntax.
        /// </summary>
        public string JsCreateElementMethodName { get; set; } = "React.createElement";
    }
}