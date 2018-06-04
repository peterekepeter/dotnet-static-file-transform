using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;
using System;
using System.Text.RegularExpressions;
using StaticFileTransform.Implementation;

namespace StaticFileTransform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Func<String, Boolean> matcher,
            Func<String, String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddSingleton<ITransformationPriority>(services => new TextTransform(matcher, transform, priority));

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Func<String, Boolean> matcher,
            Func<String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddStaticFileTransform(matcher, (filename, content) => transform(content), priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Regex regexp,
            Func<String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddStaticFileTransform(regexp, (filename, content) => transform(content), priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Regex regexp,
            Func<String, String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddStaticFileTransform(regexp.IsMatch, transform, priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            String globPattern,
            Func<String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddStaticFileTransform(GlobPatternToFunction(globPattern), transform, priority);


        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            String globPattern,
            Func<String, String, String> transform,
            Double priority = TransformationPriority.Stitcher)
            => collection.AddStaticFileTransform(GlobPatternToFunction(globPattern), transform, priority);

        public static Func<string, bool> GlobPatternToFunction(string pattern)
        {
            if (pattern.Contains("*"))
            {
                pattern = pattern
                    .Replace(".", "\\.")
                    .Replace("?", ".?")
                    .Replace("*", ".*?")
                    .Replace("(", "\\(")
                    .Replace(")", "\\)")
                    .Replace("[", "\\[")
                    .Replace("]", "\\]")
                    .Replace("{", "\\{")
                    .Replace("}", "\\}");
                var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
                return filename => regex.IsMatch(filename);
            }
            else
            {
                return filename => filename.EndsWith(filename);
            }
        }

    }
}
