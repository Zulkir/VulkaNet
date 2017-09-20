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

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe struct VkSparseImageMemoryBind
    {
        public VkImageSubresource Subresource { get; set; }
        public VkOffset3D Offset { get; set; }
        public VkExtent3D Extent { get; set; }
        public IVkDeviceMemory Memory { get; set; }
        public ulong MemoryOffset { get; set; }
        public VkSparseMemoryBindFlags Flags { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkImageSubresource subresource;
            public VkOffset3D offset;
            public VkExtent3D extent;
            public VkDeviceMemory.HandleType memory;
            public ulong memoryOffset;
            public VkSparseMemoryBindFlags flags;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSparseImageMemoryBindExtensions
    {
        public static int SizeOfMarshalDirect(this VkSparseImageMemoryBind s)
        {
            return 0;
        }

        public static VkSparseImageMemoryBind.Raw MarshalDirect(this VkSparseImageMemoryBind s, ref byte* unmanaged)
        {

            VkSparseImageMemoryBind.Raw result;
            result.subresource = s.Subresource;
            result.offset = s.Offset;
            result.extent = s.Extent;
            result.memory = s.Memory?.Handle ?? VkDeviceMemory.HandleType.Null;
            result.memoryOffset = s.MemoryOffset;
            result.flags = s.Flags;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSparseImageMemoryBind s) =>
            s.SizeOfMarshalDirect() + VkSparseImageMemoryBind.Raw.SizeInBytes;

        public static int SizeOfMarshalIndirect(this VkSparseImageMemoryBind? s) =>
            s?.SizeOfMarshalIndirect() ?? 0;

        public static VkSparseImageMemoryBind.Raw* MarshalIndirect(this VkSparseImageMemoryBind s, ref byte* unmanaged)
        {
            var result = (VkSparseImageMemoryBind.Raw*)unmanaged;
            unmanaged += VkSparseImageMemoryBind.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static VkSparseImageMemoryBind.Raw* MarshalIndirect(this VkSparseImageMemoryBind? s, ref byte* unmanaged) =>
            s.HasValue ? s.Value.MarshalIndirect(ref unmanaged) : null;

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSparseImageMemoryBind> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSparseImageMemoryBind.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSparseImageMemoryBind.Raw* MarshalDirect(this IReadOnlyList<VkSparseImageMemoryBind> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseImageMemoryBind.Raw*)0;
            var result = (VkSparseImageMemoryBind.Raw*)unmanaged;
            unmanaged += sizeof(VkSparseImageMemoryBind.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
