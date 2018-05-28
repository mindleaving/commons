using System;
using System.IO;
using System.Reflection;

namespace Commons.IO
{
    public static class WorkingDirectory
    {
        public static string GetPath()
        {
            var uri = GetUri();
            var path = Uri.UnescapeDataString(uri.AbsolutePath);
            return Path.GetDirectoryName(path);
        }

        public static Uri GetUri()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            return new UriBuilder(codeBase).Uri;
        }
    }
}
