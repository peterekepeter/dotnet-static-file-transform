using System;
using System.Collections.Generic;
using dotless.Core;
using dotless.Core.configuration;
using dotless.Core.Plugins;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    public class Dotless : ITextFileTransform
    {
        private readonly DotlessConfiguration _config;
        private readonly double _priority;
        private readonly Func<String, Boolean> _matcher;

        public Dotless(DotlessOptions options = null)
        {
            if (options == null)
            {
                options = new DotlessOptions(); // defaults
            }
            _config = DotlessConfiguration.GetDefault();
            _config.KeepFirstSpecialComment = options.KeepFirstSpecialComment;
            _config.RootPath = options.RootPath;
            _config.InlineCssFiles = options.InlineCssFiles;
            _config.ImportAllFilesAsLess = options.ImportAllFilesAsLess;
            _config.MinifyOutput = options.MinifyOutput;
            _config.Debug = options.Debug;
            _config.Optimization = options.Optimization;
            _config.StrictMath = options.StrictMath;
            _priority = options.Priority;
            if (options.FileMatcher != null) _matcher = options.FileMatcher;
            else _matcher = filename => filename.EndsWith(".less");
        }

        public string Apply(string filename, string input) => Less.Parse(input, _config);

        public bool Matches(string filename) => _matcher(filename);

        public double Priority => _priority;
    }
}
