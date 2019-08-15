using System;
using System.Collections.Generic;

namespace Commons.Misc
{
    public static class Secrets
    {
        public static string Get(string secretName)
        {
            return Environment.GetEnvironmentVariable(secretName) 
                   ?? throw new KeyNotFoundException($"Could not find environment variable '{secretName}'");
        }
    }
}
