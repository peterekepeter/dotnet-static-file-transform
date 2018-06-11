using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using dotless.Core.Input;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    internal class FileReaderAdapter : IFileReader
    {
        private readonly IContentProvider _provider;

        public FileReaderAdapter( IContentProvider provider)
        {
            _provider = provider;
        }

        public byte[] GetBinaryFileContents(string fileName)
        {
            throw new NotImplementedException("GetBinaryFileContents not supported");
        }

        internal String ResolvePath(string request)
        {
            // absolute path
            if (request.StartsWith("/")) return request; 
            // relative path
            var location = CurrentLocationGetter() ?? "";
            var lastIndex = location.LastIndexOf("/", StringComparison.Ordinal);
            location = lastIndex > 0 ? location.Substring(0, lastIndex) : "";
            var repeat = true;
            while (repeat)
            {
                repeat = false;
                if (request.StartsWith("../"))
                {
                    request = request.Substring(3);
                    lastIndex = location.LastIndexOf("/", StringComparison.Ordinal);
                    location = lastIndex > 0 ? location.Substring(0, lastIndex) : "";
                    repeat = true;
                }
                if (request.StartsWith("./"))
                {
                    request = request.Substring(2);
                    repeat = true;
                }
            }
            return $"{location}/{request}";
        }

        public string GetFileContents(string fileName) => _provider.GetContent(ResolvePath(fileName));

        public bool DoesFileExist(string fileName) => _provider.FileExists(ResolvePath(fileName));

        public bool UseCacheDependencies => true;

        public Func<String> CurrentLocationGetter { get; set; }
    }
}
