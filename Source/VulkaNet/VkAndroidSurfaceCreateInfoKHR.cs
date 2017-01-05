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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe class VkAndroidSurfaceCreateInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public VkAndroidSurfaceCreateFlagsKHR Flags { get; set; }
        public IntPtr Window { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkAndroidSurfaceCreateFlagsKHR flags;
            public IntPtr window;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkAndroidSurfaceCreateInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkAndroidSurfaceCreateInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkAndroidSurfaceCreateInfoKHR.Raw MarshalDirect(this VkAndroidSurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkAndroidSurfaceCreateInfoKHR.Raw result;
            result.sType = VkStructureType.AndroidSurfaceCreateInfoKHR;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.window = s.Window;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkAndroidSurfaceCreateInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkAndroidSurfaceCreateInfoKHR.Raw.SizeInBytes;

        public static VkAndroidSurfaceCreateInfoKHR.Raw* MarshalIndirect(this VkAndroidSurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkAndroidSurfaceCreateInfoKHR.Raw*)0;
            var result = (VkAndroidSurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += VkAndroidSurfaceCreateInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkAndroidSurfaceCreateInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkAndroidSurfaceCreateInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkAndroidSurfaceCreateInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkAndroidSurfaceCreateInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkAndroidSurfaceCreateInfoKHR.Raw*)0;
            var result = (VkAndroidSurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkAndroidSurfaceCreateInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
