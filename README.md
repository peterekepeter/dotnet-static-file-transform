

# Static File Transform

Built for dotnet core, and it allows you to transformation to statically 
served files. This is done by hooking into the StaticFileMiddleware, so we're
using standard components all the way. There are even neat helper functions for
usage and registration.


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
		// just use a lambda function
        services.AddStaticFileTransform("*.html", content => {
            return content.Replace("<body>", "<body><h1>Transformed</h1>");
        });

        // create your own transformations
        services.AddSingleton<ITextFileTransform, MyCustomTransform>();
    }

You also need to use a "middleware", but there is really no new middleware, 
just the standard static files with custom options. (keep this in mind if you
need customization)

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

		// use transformations that were added in ConfigureServices
        app.UseStaticFilesWithTransformations();

        app.Run(async (context) =>
        {
            await context.Response.WriteAsync("Hello World!");
        });
    }

That's all! The library will apply your transformations, it will filter out
for which file which transformation is required to save performance.

The following things should be noted though:

 - Transformation result is cached, so you can do expensive computation.
 - If original file is modified, cache is invalidated, so you can live edit.


## Implement Your Own Transformations

You just need to implement the ITextFileTransform interface. If you intend on
sharing your transformation with others, I recommend making the priority
initialize from constructor so that developers can change the priority of 
your transformation.

You only need to reference StaticFileTransform.Abstractions to use the
interfaces. This is recommended. See the ITextFileTransform for more details.

	public class MyCustomTransform : ITextFileTransform
    {
        public String Apply(String filename, String input)
        {
            return $"<!-- Copyright SomeCompany {DateTime.Now} -->\n" + input;
        }

        public bool Matches(string filename)
        {
            return filename.EndsWith(".html");
        }

        public double Priority => 10.0;
    }


## Future

Integrate some actually useful transformations.
