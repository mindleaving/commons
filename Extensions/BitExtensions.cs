using System;
using System.Linq;

namespace Commons.Extensions
{
    public static class BitExtensions
    {
        public static int InvertEndian(this int x)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(x).Reverse().ToArray(), 0);
        }
    }
}
