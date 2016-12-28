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
    public interface IVkMemoryAllocateInfo
    {
        IVkStructWrapper Next { get; }
        ulong AllocationSize { get; }
        int MemoryTypeIndex { get; }
    }

    public unsafe class VkMemoryAllocateInfo : IVkMemoryAllocateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public ulong AllocationSize { get; set; }
        public int MemoryTypeIndex { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public ulong allocationSize;
            public int memoryTypeIndex;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkMemoryAllocateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkMemoryAllocateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkMemoryAllocateInfo.Raw MarshalDirect(this IVkMemoryAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkMemoryAllocateInfo.Raw result;
            result.sType = VkStructureType.MemoryAllocateInfo;
            result.pNext = pNext;
            result.allocationSize = s.AllocationSize;
            result.memoryTypeIndex = s.MemoryTypeIndex;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkMemoryAllocateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkMemoryAllocateInfo.Raw.SizeInBytes;

        public static VkMemoryAllocateInfo.Raw* MarshalIndirect(this IVkMemoryAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkMemoryAllocateInfo.Raw*)0;
            var result = (VkMemoryAllocateInfo.Raw*)unmanaged;
            unmanaged += VkMemoryAllocateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkMemoryAllocateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkMemoryAllocateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkMemoryAllocateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkMemoryAllocateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMemoryAllocateInfo.Raw*)0;
            var result = (VkMemoryAllocateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkMemoryAllocateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkMemoryAllocateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkMemoryAllocateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkMemoryAllocateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkMemoryAllocateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMemoryAllocateInfo.Raw**)0;
            var result = (VkMemoryAllocateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkMemoryAllocateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
