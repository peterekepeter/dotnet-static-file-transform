

# Static File Transform

Built for dotnet core, and it allows you to transformation to statically 
served files. This is done by hooking into the StaticFileMiddleware, so we're
using standard components all the way. There are even neat helper functions for
usage and registration.

## Avaliable integrations

 - dotless
 - NUglify

## How to use?

You first need to install the base nuget package called StaticFileTransform.
This contains everything you need to transform but it does not contain any 
transformations. You'll need to add those manually. It will still work 
fine if no transformations are defined.

Then you need to register you transformations. This is done by registering
implementations for ITextFileTransform. All of these will be applied. A handy
alternative is to just use the AddStaticFileTransform method which uses lambda
functions.

	public void ConfigureServices(IServiceCollection services)
	{
	    // register text file transforamtions
	    services.AddSingleton<IStaticFileTransform, MyCustomTransform>();
	
	    services.AddStaticFileTransform(builder => builder
	        .Use(content => content.Replace("<body>", "<body><h1>Transformed</h1>"))
	        .IfMatches("*.html").WithStitcherPriority());
		
	
	    services.AddNUglifyAll();
	
	    services.AddDotless(provider => new DotlessOptions
	    {
	        RootPath = provider.GetService<IHostingEnvironment>().WebRootPath
	    });
	}

You also need to use a "middleware", but there is really no new middleware, 
just the standard static files with custom options. (keep this in mind if you
need customization)

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
	    if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
	
	    // this line is required to enable static file transformation support
	    app.UseStaticFilesWithTransformations();
	
	    app.Run(async (context) => await context.Response.WriteAsync("Hello World!"));
	}

That's all! The library will apply your transformations, it will filter out
for which file which transformation is required to save performance.

The following things should be noted though:

 - Transformation result is cached, so you can do expensive computation.
 - If original file is modified, cache is invalidated, so you can live edit.


## Implement Your Own Transformations

One way is to implement the IStaticFileTransform interface. But that can be
long and it's hard to configure. 

	public class MyCustomTransform : IStaticFileTransform
	{
	    public String Apply(String filename, IContentProvider provider)
	    {
	        return $"<!-- Copyright SomeCompany {DateTime.Now} -->\n" + provider.GetContent(filename);
	    }
	
	    public bool Matches(string filename)
	    {
	        return filename.EndsWith(".html");
	    }
	
	    public int Priority => 50;
	}

The better way is to rely on StaticFileTransformBuilder which will 
help you with common functionality such as filename matching based on patterns.

    public static StaticFileTransformBuilder NUglifyJs(this StaticFileTransformBuilder builder, NUglifyJsOptions options = null)
    {
        var nuglify = new NUglifyJs(options);
        return builder
            .Use((filename, provider) =>
            {
                var content = provider.GetContent(filename);
                return content == null ? null : nuglify.Apply(filename, content);
            })
            .IfMatches("js")
            .WithMinifierPriority();
    }

And use dependency injection, you need to inject the transformations as
an implementation of IStaticFileTransform interface. Then the middleware
will be able to make use of the transformations.

        public static IServiceCollection AddNUglifyJs(
            this IServiceCollection collection,
            NUglifyJsOptions options = null)
            => collection.AddSingleton<IStaticFileTransform>(services
                => new StaticFileTransformBuilder().NUglifyJs(options).Build());


You only need to reference StaticFileTransform.Abstractions to use the
interfaces. This is the recommended way of adding a new transformation.