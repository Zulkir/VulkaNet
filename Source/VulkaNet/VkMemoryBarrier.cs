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
    public unsafe class VkMemoryBarrier
    {
        public IVkStructWrapper Next { get; set; }
        public VkAccessFlags SrcAccessMask { get; set; }
        public VkAccessFlags DstAccessMask { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkAccessFlags srcAccessMask;
            public VkAccessFlags dstAccessMask;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkMemoryBarrierExtensions
    {
        public static int SizeOfMarshalDirect(this VkMemoryBarrier s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkMemoryBarrier.Raw MarshalDirect(this VkMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkMemoryBarrier.Raw result;
            result.sType = VkStructureType.MemoryBarrier;
            result.pNext = pNext;
            result.srcAccessMask = s.SrcAccessMask;
            result.dstAccessMask = s.DstAccessMask;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkMemoryBarrier s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkMemoryBarrier.Raw.SizeInBytes;

        public static VkMemoryBarrier.Raw* MarshalIndirect(this VkMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkMemoryBarrier.Raw*)0;
            var result = (VkMemoryBarrier.Raw*)unmanaged;
            unmanaged += VkMemoryBarrier.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkMemoryBarrier> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkMemoryBarrier.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkMemoryBarrier.Raw* MarshalDirect(this IReadOnlyList<VkMemoryBarrier> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMemoryBarrier.Raw*)0;
            var result = (VkMemoryBarrier.Raw*)unmanaged;
            unmanaged += sizeof(VkMemoryBarrier.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
