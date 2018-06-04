using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNUglifyCss(
            this IServiceCollection collection, 
            NUglifyCssOptions options = null) 
            => collection
            .AddSingleton<ITransformationPriority>(services => new NUglifyCss(options))
            ;
        
        public static IServiceCollection AddNUglifyHtml(
            this IServiceCollection collection,
            NUglifyHtmlOptions options = null)
            => collection
            .AddSingleton<ITransformationPriority>(services => new NUglifyHtml(options))
            ;

        public static IServiceCollection AddNUglifyJs(
            this IServiceCollection collection,
            NUglifyJsOptions options = null)
            => collection
            .AddSingleton<ITransformationPriority>(services => new NUglifyJs(options))
            ;

        public static IServiceCollection AddNUglifyAll(
            this IServiceCollection collection,
            NUglifyCssOptions css = null,
            NUglifyHtmlOptions html = null,
            NUglifyJsOptions js = null)
            => collection
            .AddNUglifyCss(css)
            .AddNUglifyHtml(html)
            .AddNUglifyJs(js)
            ;
    }
}
