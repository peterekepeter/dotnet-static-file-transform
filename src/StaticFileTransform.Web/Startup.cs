using DotnetStaticFileTransformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;
using StaticFileTransform.dotless;
using StaticFileTransform.NUglify;

namespace StaticFileTransform.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // register text file transforamtions
            services.AddSingleton<ITransformationPriority, MyCustomTransform>();
            
            services.AddStaticFileTransform("*.html", content => content.Replace("<body>", "<body><h1>Transformed</h1>"));
            services.AddNUglifyAll();

            services.AddDotless(provider => new DotlessOptions
            {
                RootPath = provider.GetService<IHostingEnvironment>().WebRootPath
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFilesWithTransformations(new StaticFileOptions
            {
                ServeUnknownFileTypes = true // to serve less files
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
