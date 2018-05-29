using Microsoft.Extensions.FileProviders;
using StaticFileTransform.Abstractions;
using System.Collections.Generic;
using System.Threading;

namespace StaticFileTransform.Internal
{
    internal class TransformedFileInfo
    {
        public IFileInfo FileInfo;
        public IFileInfo OriginalFileInfo;
        public List<ITextFileTransform> Transformations;
        public SemaphoreSlim WorkerSemaphore = new SemaphoreSlim(1, 1);
    }
}
