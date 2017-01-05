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
    public unsafe class VkMirSurfaceCreateInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public VkMirSurfaceCreateFlagsKHR Flags { get; set; }
        public IntPtr Connection { get; set; }
        public IntPtr MirSurface { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkMirSurfaceCreateFlagsKHR flags;
            public IntPtr connection;
            public IntPtr mirSurface;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkMirSurfaceCreateInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkMirSurfaceCreateInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkMirSurfaceCreateInfoKHR.Raw MarshalDirect(this VkMirSurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkMirSurfaceCreateInfoKHR.Raw result;
            result.sType = VkStructureType.MirSurfaceCreateInfoKHR;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.connection = s.Connection;
            result.mirSurface = s.MirSurface;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkMirSurfaceCreateInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkMirSurfaceCreateInfoKHR.Raw.SizeInBytes;

        public static VkMirSurfaceCreateInfoKHR.Raw* MarshalIndirect(this VkMirSurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkMirSurfaceCreateInfoKHR.Raw*)0;
            var result = (VkMirSurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += VkMirSurfaceCreateInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkMirSurfaceCreateInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkMirSurfaceCreateInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkMirSurfaceCreateInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkMirSurfaceCreateInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMirSurfaceCreateInfoKHR.Raw*)0;
            var result = (VkMirSurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkMirSurfaceCreateInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
