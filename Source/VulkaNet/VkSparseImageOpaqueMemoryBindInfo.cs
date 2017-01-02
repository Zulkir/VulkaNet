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
    public unsafe struct VkSparseImageOpaqueMemoryBindInfo
    {
        public IVkImage Image { get; set; }
        public IReadOnlyList<VkSparseMemoryBind> Binds { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkImage.HandleType image;
            public int bindCount;
            public VkSparseMemoryBind.Raw* pBinds;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSparseImageOpaqueMemoryBindInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkSparseImageOpaqueMemoryBindInfo s)
        {
            return
                s.Binds.SizeOfMarshalDirect();
        }

        public static VkSparseImageOpaqueMemoryBindInfo.Raw MarshalDirect(this VkSparseImageOpaqueMemoryBindInfo s, ref byte* unmanaged)
        {
            var pBinds = s.Binds.MarshalDirect(ref unmanaged);

            VkSparseImageOpaqueMemoryBindInfo.Raw result;
            result.image = s.Image?.Handle ?? VkImage.HandleType.Null;
            result.bindCount = s.Binds?.Count ?? 0;
            result.pBinds = pBinds;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSparseImageOpaqueMemoryBindInfo s) =>
            s.SizeOfMarshalDirect() + VkSparseImageOpaqueMemoryBindInfo.Raw.SizeInBytes;

        public static VkSparseImageOpaqueMemoryBindInfo.Raw* MarshalIndirect(this VkSparseImageOpaqueMemoryBindInfo s, ref byte* unmanaged)
        {
            var result = (VkSparseImageOpaqueMemoryBindInfo.Raw*)unmanaged;
            unmanaged += VkSparseImageOpaqueMemoryBindInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSparseImageOpaqueMemoryBindInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSparseImageOpaqueMemoryBindInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSparseImageOpaqueMemoryBindInfo.Raw* MarshalDirect(this IReadOnlyList<VkSparseImageOpaqueMemoryBindInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseImageOpaqueMemoryBindInfo.Raw*)0;
            var result = (VkSparseImageOpaqueMemoryBindInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSparseImageOpaqueMemoryBindInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
