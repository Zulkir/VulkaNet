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
    public unsafe struct VkSparseMemoryBind
    {
        public ulong ResourceOffset { get; set; }
        public ulong Size { get; set; }
        public IVkDeviceMemory Memory { get; set; }
        public ulong MemoryOffset { get; set; }
        public VkSparseMemoryBindFlags Flags { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public ulong resourceOffset;
            public ulong size;
            public VkDeviceMemory.HandleType memory;
            public ulong memoryOffset;
            public VkSparseMemoryBindFlags flags;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSparseMemoryBindExtensions
    {
        public static int SizeOfMarshalDirect(this VkSparseMemoryBind s)
        {
            return 0;
        }

        public static VkSparseMemoryBind.Raw MarshalDirect(this VkSparseMemoryBind s, ref byte* unmanaged)
        {

            VkSparseMemoryBind.Raw result;
            result.resourceOffset = s.ResourceOffset;
            result.size = s.Size;
            result.memory = s.Memory?.Handle ?? VkDeviceMemory.HandleType.Null;
            result.memoryOffset = s.MemoryOffset;
            result.flags = s.Flags;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSparseMemoryBind s) =>
            s.SizeOfMarshalDirect() + VkSparseMemoryBind.Raw.SizeInBytes;

        public static VkSparseMemoryBind.Raw* MarshalIndirect(this VkSparseMemoryBind s, ref byte* unmanaged)
        {
            var result = (VkSparseMemoryBind.Raw*)unmanaged;
            unmanaged += VkSparseMemoryBind.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSparseMemoryBind> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSparseMemoryBind.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSparseMemoryBind.Raw* MarshalDirect(this IReadOnlyList<VkSparseMemoryBind> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseMemoryBind.Raw*)0;
            var result = (VkSparseMemoryBind.Raw*)unmanaged;
            unmanaged += sizeof(VkSparseMemoryBind.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
