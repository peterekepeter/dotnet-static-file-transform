using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using AssetTransform;

namespace DotnetStaticFileTransformation
{

   public class TestTransform : ITextFileTransform
    {
        public String Apply(String filename, String input)
        {
            new PhysicalFileProvider
            return $"<!-- Copyright SomeCompany {DateTime.Now} -->\n" + input;
        }

        public bool Matches(string filename)
        {
            return filename.EndsWith(".html");
        }

        public double Priority => 10.0;
    }


    


}
