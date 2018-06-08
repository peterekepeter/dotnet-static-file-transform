using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticFileTransform.Abstractions
{
    public class StaticFileTransformBuilder
    {
        private Func<String, IContentProvider, String> transform = null;
        private Func<String, Boolean> matcher = null;
        private Nullable<int> priority = null;

        public StaticFileTransformBuilder Use(Func<String, IContentProvider, String> transformation)
        {
            transform = transformation;
            return this;
        }

        public StaticFileTransformBuilder WithPriority(int prio)
        {
            this.priority = prio;
            return this;
        }

        public StaticFileTransformBuilder IfMatches(Func<String, Boolean> rule)
        {
            matcher = rule;
            return this;
        }

        public StaticFileTransform Build()
        {
            if (transform == null) throw new InvalidOperationException("Transformation must be defined when attempting to build an IStaticFileTransform.");
            if (matcher == null) throw new InvalidOperationException("File matcher must be defined when attempting to build an IStaticFileTransform.");
            if (!priority.HasValue) throw new InvalidOperationException("Priority must be defined when attempting to build an IStaticFileTransform.");
            return new StaticFileTransform(matcher, transform, priority.Value);
        }
        
        public int Priority => priority ?? 0;

        public static implicit operator StaticFileTransform(StaticFileTransformBuilder builder) => builder.Build();
    }
}
