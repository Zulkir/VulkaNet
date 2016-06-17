using System.Linq;

namespace VulkaNet
{
    public static class StringExtensions
    {
        public static byte[] ToAnsiArray(this string str) => str.Select(x => (byte)x).ToArray();

        public static unsafe void CopyAsAnsiTo(this string str, byte* dest)
        {
            CopyAsAnsiTo(str, ref dest);
        }

        public static unsafe void CopyAsAnsiTo(this string str, ref byte* dest)
        {
            foreach (var ch in str)
            {
                *dest = (byte)ch;
                dest += 1;
            }
            *dest = 0;
            dest += 1;
        }

        public static int SafeMarshalSize(this string str)
        {
            if (str == null)
                return 0;
            return str.Length + 1;
        }

        public static unsafe byte* SafeMarshalTo(this string str, ref byte* dst)
        {
            if (str == null)
                return (byte*)0;
            var result = dst;
            foreach (var ch in str)
            {
                *dst = (byte)ch;
                dst++;
            }
            *dst = 0;
            dst++;
            return result;
        }
    }
}