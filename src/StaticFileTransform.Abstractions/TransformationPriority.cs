
// ReSharper disable once CheckNamespace
namespace StaticFileTransform
{
    public static class TransformationPriority
    {
        /// <summary>
        /// Any other type of post processing transformation. This step would happen last.
        /// </summary>
        public const int PostProcess = 0;

        /// <summary>
        /// Transformations that minify content.
        /// </summary>
        public const int Minifier = 20;

        /// <summary>
        /// Compiles content from source to destination file.
        /// </summary>
        public const int Compiler = 40;

        /// <summary>
        /// Transformation that takes the content of multiple files to produce one file.
        /// </summary>
        public const int Stitcher = 60;

        /// <summary>
        /// Transformation that remaps content from one file to another.
        /// </summary>
        public const int Router = 80;

        /// <summary>
        /// This transformation should by before other trnasformation.
        /// </summary>
        public const int PreProcess = 100;
    }
}