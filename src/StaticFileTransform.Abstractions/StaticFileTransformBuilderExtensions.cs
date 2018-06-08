using System;
using System.Text.RegularExpressions;
using Builder = StaticFileTransform.Abstractions.StaticFileTransformBuilder;

namespace StaticFileTransform.Abstractions
{
    public static class StaticFileTransformBuilderExtensions
    {
        #region CompleteTransformations

        public static Builder UseStaticContent(this Builder builder, String pattern, String content) => builder
            .IfMatches(pattern)
            .Use(content)
            .WithPreProcessPriority();

        public static Builder UseFallbackToFile(this Builder builder, String pattern, String fallbackFile) => builder
            .IfMatches(pattern)
            .Use((filename, provider) => provider.GetContent(filename))
            .WithRouterPriority();

        public static Builder UseFallbackPattern(this Builder builder, String matchPattern, String fallbackPattern)
        {
            var regex = Helpers.PatternToRegex(matchPattern);
            var stitch = Helpers.PatternToStitch(fallbackPattern);
            return builder
                .Use((filename, provider) => provider.GetContent(filename) ?? provider.GetContent(regex.Replace(filename, stitch)))
                .IfMatches(regex)
                .WithRouterPriority();
        }
        
        #endregion

        #region Transformations

        public static Builder Use(this Builder builder, Func<String, String> transformation)
            => builder.Use((filename, provider) => transformation(provider.GetContent(filename)));

        public static Builder Use(this Builder builder, Func<String> content)
            => builder.Use((filename, provider) => content());

        public static Builder Use(this Builder builder, String content)
            => builder.Use((filename, provider) => content);

        #endregion

        #region Matchers

        public static Builder IfMatches(this Builder builder, Regex regex)
            => builder.IfMatches(regex.IsMatch);

        public static Builder IfMatches(this Builder builder, string pattern)
            => builder.IfMatches(Helpers.PatternToFunction(pattern));

        public static Builder IfFilenameEndsWith(this Builder builder, string filter)
            => builder.IfMatches(input => input.EndsWith(filter));
        
        public static Builder IfMatchesExactly(this Builder builder, string filename)
            => builder.IfMatches(input => input == filename);

        #endregion
        
        #region Priorities

        public static Builder WithCompilerPriority(this Builder builder) 
            => builder.WithPriority(TransformationPriority.Compiler);

        public static Builder WithMinifierPriority(this Builder builder)
            => builder.WithPriority(TransformationPriority.Minifier);

        public static Builder WithRouterPriority(this Builder builder)
            => builder.WithPriority(TransformationPriority.Router);

        public static Builder WithStitcherPriority(this Builder builder)
            => builder.WithPriority(TransformationPriority.Stitcher);

        public static Builder WithPreProcessPriority(this Builder builder)
            => builder.WithPriority(TransformationPriority.PreProcess);

        public static Builder PriorityModifier(this Builder builder, int modify)
            => builder.WithPriority(builder.Priority + modify);

        #endregion
    }
}
