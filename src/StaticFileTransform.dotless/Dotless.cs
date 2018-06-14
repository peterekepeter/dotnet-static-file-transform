using System;
using System.Collections.Generic;
using dotless.Core;
using dotless.Core.configuration;
using dotless.Core.Importers;
using dotless.Core.Parser;
using dotless.Core.Plugins;
using dotless.Core.Stylizers;
using Microsoft.Extensions.DependencyInjection;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    public class Dotless
    {
        private readonly DotlessConfiguration _config;
        private readonly FilenameTransform _cssToLess;
        private PlainStylizer _stylizer;

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
            _cssToLess = new FilenameTransform(options.CssMatchPattern, options.LessSourceFilePattern);
            _stylizer = new PlainStylizer();
        }
        
        public string Apply(string filename, IContentProvider provider)
        {
            // transformation is only applied of requested css file is not found
            // and in the folder there is a less file with the same name
            var cssContent = provider.GetContent(filename);
            if (cssContent != null) return cssContent; // css content is available
            if (!_cssToLess.Matches(filename)) return null; // no less file
            var lessFilename = _cssToLess.TrasformFilename(filename);
            var lessContent = provider.GetContent(lessFilename);
            if (lessContent == null) return null;
            var fileReader = new FileReaderAdapter(provider);
            var importer = new Importer(fileReader, 
                _config.DisableUrlRewriting, _config.RootPath, _config.InlineCssFiles, _config.ImportAllFilesAsLess);
            var parser = new Parser(_config, _stylizer, importer);
            fileReader.CurrentLocationGetter = () => parser.FileName;
            var logger = new LoggerAdapter();
            var engine = new LessEngine(parser, logger, _config);
            var css = engine.TransformToCss(lessContent, filename);
            if (logger.ErrorCount > 0)
                throw new InvalidOperationException(
                    $"Found {logger.ErrorCount} errors while compiling Less:\n{logger.CompilationLog}");
            return css;
        }

        public bool Matches(string filename)
        {
            return _cssToLess.Matches(filename);
        }

    }
}
