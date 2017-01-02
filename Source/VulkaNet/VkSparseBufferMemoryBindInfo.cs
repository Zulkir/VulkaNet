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
    public unsafe struct VkSparseBufferMemoryBindInfo
    {
        public IVkBuffer Buffer { get; set; }
        public IReadOnlyList<VkSparseMemoryBind> Binds { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkBuffer.HandleType buffer;
            public int bindCount;
            public VkSparseMemoryBind.Raw* pBinds;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSparseBufferMemoryBindInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkSparseBufferMemoryBindInfo s)
        {
            return
                s.Binds.SizeOfMarshalDirect();
        }

        public static VkSparseBufferMemoryBindInfo.Raw MarshalDirect(this VkSparseBufferMemoryBindInfo s, ref byte* unmanaged)
        {
            var pBinds = s.Binds.MarshalDirect(ref unmanaged);

            VkSparseBufferMemoryBindInfo.Raw result;
            result.buffer = s.Buffer?.Handle ?? VkBuffer.HandleType.Null;
            result.bindCount = s.Binds?.Count ?? 0;
            result.pBinds = pBinds;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSparseBufferMemoryBindInfo s) =>
            s.SizeOfMarshalDirect() + VkSparseBufferMemoryBindInfo.Raw.SizeInBytes;

        public static VkSparseBufferMemoryBindInfo.Raw* MarshalIndirect(this VkSparseBufferMemoryBindInfo s, ref byte* unmanaged)
        {
            var result = (VkSparseBufferMemoryBindInfo.Raw*)unmanaged;
            unmanaged += VkSparseBufferMemoryBindInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSparseBufferMemoryBindInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSparseBufferMemoryBindInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSparseBufferMemoryBindInfo.Raw* MarshalDirect(this IReadOnlyList<VkSparseBufferMemoryBindInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseBufferMemoryBindInfo.Raw*)0;
            var result = (VkSparseBufferMemoryBindInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSparseBufferMemoryBindInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
