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
    public interface IVkMappedMemoryRange
    {
        IVkStructWrapper Next { get; }
        IVkDeviceMemory Memory { get; }
        ulong Offset { get; }
        ulong Size { get; }
    }

    public unsafe class VkMappedMemoryRange : IVkMappedMemoryRange
    {
        public IVkStructWrapper Next { get; set; }
        public IVkDeviceMemory Memory { get; set; }
        public ulong Offset { get; set; }
        public ulong Size { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDeviceMemory.HandleType memory;
            public ulong offset;
            public ulong size;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkMappedMemoryRangeExtensions
    {
        public static int SizeOfMarshalDirect(this IVkMappedMemoryRange s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkMappedMemoryRange.Raw MarshalDirect(this IVkMappedMemoryRange s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkMappedMemoryRange.Raw result;
            result.sType = VkStructureType.MappedMemoryRange;
            result.pNext = pNext;
            result.memory = s.Memory?.Handle ?? VkDeviceMemory.HandleType.Null;
            result.offset = s.Offset;
            result.size = s.Size;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkMappedMemoryRange s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkMappedMemoryRange.Raw.SizeInBytes;

        public static VkMappedMemoryRange.Raw* MarshalIndirect(this IVkMappedMemoryRange s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkMappedMemoryRange.Raw*)0;
            var result = (VkMappedMemoryRange.Raw*)unmanaged;
            unmanaged += VkMappedMemoryRange.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkMappedMemoryRange> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkMappedMemoryRange.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkMappedMemoryRange.Raw* MarshalDirect(this IReadOnlyList<IVkMappedMemoryRange> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMappedMemoryRange.Raw*)0;
            var result = (VkMappedMemoryRange.Raw*)unmanaged;
            unmanaged += sizeof(VkMappedMemoryRange.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkMappedMemoryRange> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkMappedMemoryRange.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkMappedMemoryRange.Raw** MarshalIndirect(this IReadOnlyList<IVkMappedMemoryRange> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkMappedMemoryRange.Raw**)0;
            var result = (VkMappedMemoryRange.Raw**)unmanaged;
            unmanaged += sizeof(VkMappedMemoryRange.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
