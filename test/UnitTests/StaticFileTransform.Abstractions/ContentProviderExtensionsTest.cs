using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticFileTransform.Abstractions;
using UnitTests.StaticFileTransform.Abstractions.Mock;

namespace UnitTests.StaticFileTransform.Abstractions
{
    [TestClass]
    public class ContentProviderExtensionsTest
    {
        public ContentProviderExtensionsTest()
        {
            Provider = new MockContentProvider(new Dictionary<string, string>{ {"robots.txt", "Disallow"} });
        }

        public MockContentProvider Provider { get; set; }
        
        [TestMethod]
        public void FileExistsContentProvider()
            => Provider.FileExists("robots.txt").Should().BeTrue();

        [TestMethod]
        public void FileDoesNotExist()
            => Provider.FileExists("index.html").Should().BeFalse();

        [TestMethod]
        [DataRow("robots.txt")]
        [DataRow("index.html")]
        public void IfFileExistsContentIsReturnedOtherwiseNull(String filename)
        {
            if (Provider.FileExists(filename))
            {
                Provider.GetContent(filename).Should().NotBeNull();
            }
            else
            {
                Provider.GetContent(filename).Should().BeNull();
            }
        }

    }
}
