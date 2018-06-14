using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using StaticFileTransform.Abstractions;
using StaticFileTransform.Internal;

namespace StaticFileTransform.Implementation
{
    public class TransformedFileProvider : IFileProvider
    {
        // From Constructor

        private readonly IFileProvider _baseProvider;
        private readonly List<IStaticFileTransform> _transformations;
        private Int32 _buildCounter = 1;

        // Internal structure

        private readonly ConcurrentDictionary<String, TransformedFileInfo> _memoized = new ConcurrentDictionary<string, TransformedFileInfo>();
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);

        public TransformedFileProvider(IFileProvider fileProvider, IEnumerable<IStaticFileTransform> transformations)
        {
            this._baseProvider = fileProvider;
            this._transformations = transformations.ToList();
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _baseProvider.GetDirectoryContents(subpath);
        }

        private TransformedFileInfo GetTransformedFileInfo(String path, Int32 buildId = 0)
        {
            if (!_memoized.TryGetValue(path, out var info))
            {
                _sync.Wait();
                try
                {
                    info = new TransformedFileInfo()
                    {
                        Path = path
                    };
                    if (!_memoized.TryAdd(path, info))
                    {
                        throw new InvalidOperationException("Conflict!");
                    }
                }
                finally
                {
                    _sync.Release();
                }
            }
            if (CheckIfNeedToBuild(info))
            {
                // allocate build id to current run
                if (buildId == 0)
                {
                    buildId = Interlocked.Increment(ref _buildCounter);
                }
                // check build id of target, it should not equal current build
                if (info.BuildId == buildId)
                {
                    throw new InvalidOperationException($"Detected circular dependency {path}");
                }
                BuildContent(info, buildId);
            }
            return info;
        }

        private void BuildContent(TransformedFileInfo info, Int32 buildId)
        {
            info.WorkerSemaphore.Wait();
            try
            {
                info.BuildId = buildId;
                List<IStaticFileTransform> transforms;

                // gather transformations and order them
                if (info.MatchingTransforms == null)
                {
                    transforms = _transformations.MatchingTransformations(info.Path);
                    info.MatchingTransforms = transforms;
                }
                else
                {
                    transforms = info.MatchingTransforms;
                }

                // check if we have any transforms, if we dont we early exit
                if (transforms.Count == 0)
                {
                    if (info.Original.Exists == false || info.Original.IsDirectory)
                    {
                        info.Content = null;
                        info.Transformed = info.Original;
                    }
                    else
                    {
                        info.Content = ReadOriginalFileContent(info.Original);
                        info.Transformed = info.Original;
                    }
                    return; // early exit
                }

                // init or clear dependency list
                if (info.Depdendencies == null)
                {
                    info.Depdendencies = new List<TransformedFileInfo>();
                }
                else
                {
                    info.Depdendencies.Clear();
                }
                
                // apply the transformations
                var enumerator = transforms.GetEnumerator();
                info.Content = RecursiveApplyTransform(enumerator, info);
                info.Transformed = new FileWithMemoizedTransform(info.Original, info.Content);
                enumerator.Dispose();
            }
            finally
            {
                info.WorkerSemaphore.Release();
            }
        }

        private String RecursiveApplyTransform(List<IStaticFileTransform>.Enumerator enumerator, TransformedFileInfo info)
        {
            if (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var contentProvider = new ContentProvider(requested => {
                    if (requested == info.Path)
                    {
                        return RecursiveApplyTransform(enumerator, info);
                    }
                    else
                    {
                        var dependency = GetTransformedFileInfo(requested, info.BuildId);
                        info.Depdendencies.Add(dependency);
                        return dependency.Content;
                    }
                });
                return current?.Apply(info.Path, contentProvider);
            } 
            else
            {
                return ReadOriginalFileContent(info.Original);
            }
        }

        private bool CheckIfNeedToBuild(TransformedFileInfo info)
        {
            // update file infos and recursively check for signature changed
            info.Original = _baseProvider.GetFileInfo(info.Path);
            if (info.Transformed != null && info.Original.LastModified == info.Transformed.LastModified)
            {
                if (info.Depdendencies != null)
                {
                    foreach (var dependency in info.Depdendencies)
                    {
                        if (CheckIfNeedToBuild(dependency))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return GetTransformedFileInfo(subpath).Transformed;
        }

        private static string ReadOriginalFileContent(IFileInfo file)
        {
            if (!file.Exists || file.IsDirectory)
            {
                return null;
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
            return content;
        }

        public IChangeToken Watch(string filter)
        {
            var change = _baseProvider.Watch(filter);
            return change;
        }
    }
}
