#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

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

        public static int SizeOfMarshalIndirect(this string str)
        {
            if (str == null)
                return 0;
            return str.Length + 1;
        }

        public static unsafe byte* MarshalIndirect(this string str, ref byte* dst)
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