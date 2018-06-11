using System;
using Microsoft.Extensions.FileProviders;
using StaticFileTransform.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StaticFileTransform.Internal
{
    internal class TransformedFileInfo
    {
        public String Path;
        public String Content;
        public IFileInfo Transformed;
        public IFileInfo Original;
        public List<IStaticFileTransform> MatchingTransforms;
        public List<TransformedFileInfo> Depdendencies;
        public SemaphoreSlim WorkerSemaphore = new SemaphoreSlim(1, 1);
        public int BuildId = 0;

        public override String ToString() => $"{Path}{(Original.IsDirectory?" directory":"")}{(Original.Exists ? " exists" : "")}{(Depdendencies.Any() ? $" {Depdendencies.Count} dependencies" : " ")}";
    }
}
