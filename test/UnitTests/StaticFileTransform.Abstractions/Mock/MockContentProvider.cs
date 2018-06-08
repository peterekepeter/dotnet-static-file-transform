using System;
using System.Collections.Generic;
using System.Text;
using StaticFileTransform.Abstractions;

namespace UnitTests.StaticFileTransform.Abstractions.Mock
{
    public class MockContentProvider : IContentProvider
    {
        public Dictionary<String, String> Content;

        public MockContentProvider() => Content = new Dictionary<string, string>();

        public MockContentProvider(Dictionary<string, string> store) => Content = store;

        public string GetContent(string filename) => Content.GetValueOrDefault(filename);
    }
}
