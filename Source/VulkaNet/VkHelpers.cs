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

using System;
using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public static class VkHelpers
    {
        public static TDelegate GetDelegate<TDelegate>(IVkInstance instance, string name)
        {
            var funPtr = VkGlobal.GetInstanceProcAddr(instance, name);
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(funPtr);
        }

        public static unsafe string ToString(byte* cstr) => Marshal.PtrToStringAnsi((IntPtr)cstr);

        public static unsafe void RunWithUnamangedData(int size, Action<IntPtr> func)
        {
            if (size <= 256)
            {
                VkBlob256 blob;
                func((IntPtr)(&blob));
            }
            else
            {
                var data = new byte[size];
                fixed (byte* pData = data)
                    func((IntPtr)pData);
            }
        }

        public static unsafe TResult RunWithUnamangedData<TResult>(int size, Func<IntPtr, TResult> func)
        {
            if (size <= 256)
            {
                VkBlob256 blob;
                return func((IntPtr)(&blob));
            }
            else
            {
                var data = new byte[size];
                fixed (byte* pData = data)
                    return func((IntPtr)pData);
            }
        }
    }
}