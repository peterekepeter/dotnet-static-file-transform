using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticFileTransform.Abstractions;

namespace UnitTests.StaticFileTransform.Abstractions
{
    [TestClass]
    public class HelpersTest
    {
        [TestMethod]
        [DataRow("build/bundle*", "build/bundle-21291.js")]
        [DataRow("*.js", "test.js")]
        [DataRow("*.css", "test.css")]
        [DataRow("*.css", "/index/test.css")]
        [DataRow("index.html", "index.html")]
        [DataRow("index.html", "feature/index.html")]
        [DataRow("feature/index.html", "application/feature/index.html")]
        [DataRow("feature\\index.html", "application\\feature\\index.html")]
        public void PatternShouldMatch(string pattern, string filename) 
            => Helpers.PatternToFunction(pattern)(filename).Should().BeTrue();
        
        [TestMethod]
        [DataRow("*.ts", "test.ts.js")]
        [DataRow(".ts", "test.ts.js")]
        [DataRow("feature/test.ts", "feature/test.ts.js")]
        public void PatternShouldNotMatch(string pattern, string filename)
            => Helpers.PatternToFunction(pattern)(filename).Should().BeFalse();

        [TestMethod]
        [DataRow("build/*/bundle.*.js", "publish/*/*.js", "build/x64/bundle.app.js", "publish/x64/app.js")]
        [DataRow("index.ts", "index.js", "index.ts", "index.js")]
        [DataRow("*.ts", "*.js", "index.ts", "index.js")]
        public void PatternBasedRemap(string patternMatch, string patternReplace, string input, string result) 
            => Helpers.PatternToRegex(patternMatch)
            .Replace(input, Helpers.PatternToStitch(patternReplace))
            .Should().Be(result);
    }
}
