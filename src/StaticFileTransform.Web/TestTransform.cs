using System;
using StaticFileTransform.Abstractions;

namespace StaticFileTransform.Web
{
    public class MyCustomTransform : IStaticFileTransform
    {
        public String Apply(String filename, IContentProvider provider)
        {
            return $"<!-- Copyright SomeCompany {DateTime.Now} -->\n" + provider.GetContent(filename);
        }

        public bool Matches(string filename)
        {
            return filename.EndsWith(".html");
        }

        public int Priority => 50;
    }
}
