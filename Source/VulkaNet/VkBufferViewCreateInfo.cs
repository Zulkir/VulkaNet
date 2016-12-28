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
    public interface IVkBufferViewCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkBufferViewCreateFlags Flags { get; }
        IVkBuffer Buffer { get; }
        VkFormat Format { get; }
        ulong Offset { get; }
        ulong Range { get; }
    }

    public unsafe class VkBufferViewCreateInfo : IVkBufferViewCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkBufferViewCreateFlags Flags { get; set; }
        public IVkBuffer Buffer { get; set; }
        public VkFormat Format { get; set; }
        public ulong Offset { get; set; }
        public ulong Range { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkBufferViewCreateFlags flags;
            public VkBuffer.HandleType buffer;
            public VkFormat format;
            public ulong offset;
            public ulong range;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkBufferViewCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkBufferViewCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkBufferViewCreateInfo.Raw MarshalDirect(this IVkBufferViewCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkBufferViewCreateInfo.Raw result;
            result.sType = VkStructureType.BufferViewCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.buffer = s.Buffer?.Handle ?? VkBuffer.HandleType.Null;
            result.format = s.Format;
            result.offset = s.Offset;
            result.range = s.Range;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkBufferViewCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkBufferViewCreateInfo.Raw.SizeInBytes;

        public static VkBufferViewCreateInfo.Raw* MarshalIndirect(this IVkBufferViewCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkBufferViewCreateInfo.Raw*)0;
            var result = (VkBufferViewCreateInfo.Raw*)unmanaged;
            unmanaged += VkBufferViewCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkBufferViewCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkBufferViewCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkBufferViewCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkBufferViewCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferViewCreateInfo.Raw*)0;
            var result = (VkBufferViewCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkBufferViewCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkBufferViewCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkBufferViewCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkBufferViewCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkBufferViewCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferViewCreateInfo.Raw**)0;
            var result = (VkBufferViewCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkBufferViewCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
