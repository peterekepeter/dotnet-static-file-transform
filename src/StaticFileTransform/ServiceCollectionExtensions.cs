﻿using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;
using System;
using System.Text.RegularExpressions;

namespace StaticFileTransform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Func<String, Boolean> matcher,
            Func<String, String, String> transform,
            Double priority = 10.0)
            => collection.AddSingleton<ITextFileTransform>(services => new CustomTextTransform(matcher, transform, priority));

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Func<String, Boolean> matcher,
            Func<String, String> transform,
            Double priority = 10.0)
            => collection.AddStaticFileTransform(matcher, (filename, content) => transform(content), priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Regex regexp,
            Func<String, String> transform,
            Double priority = 10.0)
            => collection.AddStaticFileTransform(regexp, (filename, content) => transform(content), priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            Regex regexp,
            Func<String, String, String> transform,
            Double priority = 10.0)
            => collection.AddStaticFileTransform(filename => regexp.IsMatch(filename), transform, priority);

        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            String pattern,
            Func<String, String> transform,
            Double priority = 10.0)
            => collection.AddStaticFileTransform(PatternToFunction(pattern), transform, priority);


        public static IServiceCollection AddStaticFileTransform(
            this IServiceCollection collection,
            String pattern,
            Func<String, String, String> transform,
            Double priority = 10.0)
            => collection.AddStaticFileTransform(PatternToFunction(pattern), transform, priority);

        private static Func<string, bool> PatternToFunction(string pattern)
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