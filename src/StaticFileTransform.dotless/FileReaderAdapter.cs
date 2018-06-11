using System;
using System.Collections.Generic;
using System.Text;
using dotless.Core.Input;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.dotless
{
    internal class FileReaderAdapter : IFileReader
    {
        private readonly IContentProvider _provider;

        public FileReaderAdapter(IContentProvider provider)
        {
            _provider = provider;
        }

        public byte[] GetBinaryFileContents(string fileName)
        {
            throw new NotImplementedException("GetBinaryFileContents not supported");
        }

        public string GetFileContents(string fileName)
        {
            return _provider.GetContent(fileName);
        }

        public bool DoesFileExist(string fileName)
        {
            return _provider.FileExists(fileName);
        }

        public bool UseCacheDependencies => true;
    }
}
