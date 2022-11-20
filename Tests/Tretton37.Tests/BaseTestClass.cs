using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tretton37.Tests
{
    public static class BaseTestClass
    {
        private static string BaseFolder = @"TestFiles";
        
        public static string GetHTMLFile
        {
            get
            {
                return ReadFile("HTMLPage1.html");
            }
        }
        private static string ReadFile(string filename)
        {
            var fullPath = Path.Combine(BaseFolder, filename);
            return File.ReadAllText(fullPath);
        }
    }
}
