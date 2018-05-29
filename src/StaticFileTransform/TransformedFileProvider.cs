using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using StaticFileTransform.Abstractions;
using StaticFileTransform.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StaticFileTransform
{
    public class TransformedFileProvider : IFileProvider
    {
        // From Constructor

        private IFileProvider _baseProvider;
        private List<ITextFileTransform> _transformations;

        // Internal structure

        private ConcurrentDictionary<String, TransformedFileInfo> _memoized = new ConcurrentDictionary<string, TransformedFileInfo>();

        public TransformedFileProvider(IFileProvider fileProvider, IEnumerable<ITextFileTransform> transformations)
        {
            this._baseProvider = fileProvider;
            this._transformations = transformations.ToList();
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _baseProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var item = _memoized.GetOrAdd(subpath, _ => new TransformedFileInfo());

            // get real file info
            var file = _baseProvider.GetFileInfo(subpath);

            // transformation not possible in this cases
            if (file.Exists == false || file.IsDirectory)
            {
                // just forward the file
                return file;
            }

            // early exit
            if (item.FileInfo != null && InfoMatch(item.OriginalFileInfo, file))
            {
                return item.FileInfo;
            }

            // we got some work to do
            item.WorkerSemaphore.Wait();
            try
            {
                // check if some other thread did it for us!
                if (item.FileInfo != null && InfoMatch(item.OriginalFileInfo, file))
                {
                    return item.FileInfo;
                }


                // build transformations
                if (item.Transformations == null)
                {
                    item.Transformations = _transformations.MatchingTransformations(subpath);
                }

                // transformation not possible in this cases
                if (item.Transformations.Count == 0)
                {
                    // just forward the file
                    return item.FileInfo = item.OriginalFileInfo = file;
                }

                Stream stream = null;
                StreamReader reader = null;
                String content = null;
                try
                {
                    stream = file.CreateReadStream();
                    reader = new StreamReader(stream);
                    content = reader.ReadToEnd();
                }
                finally
                {
                    stream?.Dispose();
                    reader?.Dispose();
                }

                // transform and store
                content = item.Transformations.ApplyAll(subpath, content);
                item.FileInfo = new FileWithMemoizedTransform(file, content);
                item.OriginalFileInfo = file;

                // all done!
                return item.FileInfo;
            }
            finally
            {
                item.WorkerSemaphore.Release();
            }
        }

        private bool InfoMatch(IFileInfo originalFileInfo, IFileInfo file)
            => originalFileInfo.Exists == file.Exists
            && originalFileInfo.IsDirectory == file.IsDirectory
            && originalFileInfo.LastModified == file.LastModified
            && originalFileInfo.Length == file.Length
            ;

        public IChangeToken Watch(string filter)
        {
            var change = _baseProvider.Watch(filter);
            return change;
        }
    }
}
