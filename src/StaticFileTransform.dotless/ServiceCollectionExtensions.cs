using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDotless(this IServiceCollection collection, DotlessOptions options = null) 
            => collection.AddSingleton<ITransformationPriority>(services => new Dotless(options));

        public static IServiceCollection AddDotless(this IServiceCollection collection, Func<IServiceProvider, DotlessOptions> optionsBuilder)
            => collection.AddSingleton<ITransformationPriority>(services => new Dotless(optionsBuilder(services)));
    }
}
