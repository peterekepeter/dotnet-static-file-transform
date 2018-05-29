using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Text;

namespace StaticFileTransform.Internal
{
    internal class FileWithMemoizedTransform : IFileInfo
    {
        public FileWithMemoizedTransform(IFileInfo original, String content)
        {
            Exists = original.Exists;
            PhysicalPath = original.PhysicalPath;
            Name = original.Name;
            LastModified = original.LastModified;
            IsDirectory = original.IsDirectory;
            _data = Encoding.UTF8.GetBytes(content);
            Length = _data.Length;
        }

        public bool Exists { get; }

        public long Length { get; }

        public string PhysicalPath { get; }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory { get; }

        private byte[] _data;

        public Stream CreateReadStream()
        {
            return new MemoryStream(_data, false);
        }
    }
}
