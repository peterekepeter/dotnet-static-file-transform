using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;
using System;
using System.Text.RegularExpressions;
using StaticFileTransform.Implementation;

namespace StaticFileTransform
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStaticFileTransform(this IServiceCollection collection, 
            Action<StaticFileTransformBuilder> configure)
        {
            collection.AddSingleton<IStaticFileTransform>(services =>
            {
                var builder = new StaticFileTransformBuilder();
                configure(builder);
                return builder.Build();
            });
            return collection;
        }

        public static IServiceCollection AddStaticFileTransform(this IServiceCollection collection, Func<IServiceProvider, IStaticFileTransform> factory) 
            => collection.AddSingleton<IStaticFileTransform>(factory);
    }
}
