using System;
using System.Collections.Generic;
using System.Text;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    public static class StaticFileTransformBuilderExtensions
    {
        public static StaticFileTransformBuilder UseDotless(this StaticFileTransformBuilder builder, DotlessOptions options)
        {
            var dotless = new Dotless(options);
            return builder;
        }
    }
}
