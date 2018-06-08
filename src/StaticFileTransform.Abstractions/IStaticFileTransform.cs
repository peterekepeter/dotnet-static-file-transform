using System;

namespace StaticFileTransform.Abstractions
{
    public interface IStaticFileTransform
    {
        /// <summary>
        /// Applies the tranformation to the file.
        /// Basic implementation should just return input.
        /// </summary>
        /// <param name="filename">Path to file, might be needed for extra decisions during transform.</param>
        /// <param name="provider">Load the content of a file.</param>
        /// <returns>Based on input a new string should be returned.</returns>
        String Apply(String filename, IContentProvider provider);

        /// <summary>
        /// Determine if this transformation should be applied to the file.
        /// </summary>
        /// <param name="filename">Full path to file.</param>
        /// <returns>True if transformation should be applied, false otherwise.</returns>
        bool Matches(String filename);

        /// <summary>
        /// If multiple transformations match a file, this will determine the order of transformations.
        /// For goot defaults check TransformPriority class which contains constants.
        /// </summary>
        /// <returns>Number which defines the priority of this transformation.</returns>
        int Priority { get; }
    }
}