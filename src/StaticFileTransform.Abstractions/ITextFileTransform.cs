using System;

namespace StaticFileTransform.Abstractions
{
    public interface ITextFileTransform
    {
        /// <summary>
        /// Applies the tranformation to the file.
        /// Basic implementation should just return input.
        /// </summary>
        /// <param name="filename">Full path to file, might be needed for extra decisions during transform.</param>
        /// <param name="input">The contents of the file to tranform.</param>
        /// <returns>Based on input a new string should be returned.</returns>
        String Apply(String filename, String input);

        /// <summary>
        /// Determine if this transformation should be applied to the file.
        /// </summary>
        /// <param name="filename">Full path to file.</param>
        /// <returns>True if transformation should be applied, false otherwise.</returns>
        bool Matches(String filename);

        /// <summary>
        /// If multiple transformations match a file, this will determine the order of transformations.
        /// A good default value should be 10.0, set it a bit higher for higher priority, lower for lower.
        /// The value is float so that in between priority values are possible.
        /// </summary>
        /// <returns>Number which defines the priority of this transformation.</returns>
        Double Priority { get; }
    }
}
