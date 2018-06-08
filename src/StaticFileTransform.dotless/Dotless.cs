using System;
using System.Collections.Generic;
using dotless.Core;
using dotless.Core.configuration;
using dotless.Core.Plugins;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    public class Dotless
    {
        private readonly DotlessConfiguration _config;

        public Dotless(DotlessOptions options = null)
        {
            if (options == null)
            {
                options = new DotlessOptions(); // defaults
            }
            _config = DotlessConfiguration.GetDefault();
            _config.KeepFirstSpecialComment = options.KeepFirstSpecialComment;
            _config.RootPath = options.RootPath;
            if (!String.IsNullOrWhiteSpace(_config.RootPath) && !_config.RootPath.EndsWith("/"))
            {
                _config.RootPath += "/";
            }
            _config.InlineCssFiles = options.InlineCssFiles;
            _config.ImportAllFilesAsLess = options.ImportAllFilesAsLess;
            _config.MinifyOutput = options.MinifyOutput;
            _config.Debug = options.Debug;
            _config.Optimization = options.Optimization;
            _config.StrictMath = options.StrictMath;
        }

        public string Apply(string filename, IContentProvider provider)
        {
            var input = provider.GetContent(filename);
            var config = new DotlessConfiguration(_config);
            config.
            Less.Parse(input, _config);
        };

    }
}
