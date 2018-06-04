using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;
using StaticFileTransform.Implementation;

namespace StaticFileTransform
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseStaticFilesWithTransformations(this IApplicationBuilder app)
        {
            return UseStaticFilesWithTransformations(app, null);
        }

        public static IApplicationBuilder UseStaticFilesWithTransformations(this IApplicationBuilder app, StaticFileOptions options)
        {
            // default options
            if (options == null) options = new StaticFileOptions();

            // use provided file provider if any
            var baseFileProvider = options.FileProvider;

            // default base file provider
            if (baseFileProvider == null) baseFileProvider = app.ApplicationServices.GetService<IHostingEnvironment>().WebRootFileProvider;

            // build new transformation filter
            options.FileProvider = new TransformedFileProvider(baseFileProvider, app.ApplicationServices.GetService<IEnumerable<ITransformationPriority>>());

            // add static files module with the added plugin
            app.UseStaticFiles(options);
            return app;
        }
    }

}
