using System;
using StaticFileTransform.Abstractions;

namespace DotnetStaticFileTransformation
{
    public class MyCustomTransform : ITextFileTransform
    {
        public String Apply(String filename, String input)
        {
            return $"<!-- Copyright SomeCompany {DateTime.Now} -->\n" + input;
        }

        public bool Matches(string filename)
        {
            return filename.EndsWith(".html");
        }

        public double Priority => 10.0;
    }
}
