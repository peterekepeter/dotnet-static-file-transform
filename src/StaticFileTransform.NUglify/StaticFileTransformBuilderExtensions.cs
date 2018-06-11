using StaticFileTransform.Abstractions;

namespace StaticFileTransform.NUglify
{
    public static class StaticFileTransformBuilderExtensions
    {
        public static StaticFileTransformBuilder NUglifyCss(this StaticFileTransformBuilder builder, NUglifyCssOptions options = null)
        {
            var nuglify = new NUglifyCss(options);
            return builder
                .Use((filename, provider) =>
                {
                    var content = provider.GetContent(filename);
                    return content == null ? null : nuglify.Apply(filename, content);
                })
                .IfFilenameEndsWith(".css")
                .WithMinifierPriority();
        }
        public static StaticFileTransformBuilder NUglifyHtml(this StaticFileTransformBuilder builder, NUglifyHtmlOptions options = null)
        {
            var nuglify = new NUglifyHtml(options);
            return builder
                .Use((filename, provider) =>
                {
                    var content = provider.GetContent(filename);
                    return content == null ? null : nuglify.Apply(filename, content);
                })
                .IfMatches("html?")
                .WithMinifierPriority();
        }
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


    }
}
