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
    public unsafe class VkSparseImageMemoryBindInfo
    {
        public IVkImage Image { get; set; }
        public IReadOnlyList<VkSparseImageMemoryBind> Binds { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkImage.HandleType image;
            public int bindCount;
            public VkSparseImageMemoryBind.Raw* pBinds;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSparseImageMemoryBindInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkSparseImageMemoryBindInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Binds.SizeOfMarshalDirect();
        }

        public static VkSparseImageMemoryBindInfo.Raw MarshalDirect(this VkSparseImageMemoryBindInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pBinds = s.Binds.MarshalDirect(ref unmanaged);

            VkSparseImageMemoryBindInfo.Raw result;
            result.image = s.Image?.Handle ?? VkImage.HandleType.Null;
            result.bindCount = s.Binds?.Count ?? 0;
            result.pBinds = pBinds;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSparseImageMemoryBindInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSparseImageMemoryBindInfo.Raw.SizeInBytes;

        public static VkSparseImageMemoryBindInfo.Raw* MarshalIndirect(this VkSparseImageMemoryBindInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSparseImageMemoryBindInfo.Raw*)0;
            var result = (VkSparseImageMemoryBindInfo.Raw*)unmanaged;
            unmanaged += VkSparseImageMemoryBindInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSparseImageMemoryBindInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSparseImageMemoryBindInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSparseImageMemoryBindInfo.Raw* MarshalDirect(this IReadOnlyList<VkSparseImageMemoryBindInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseImageMemoryBindInfo.Raw*)0;
            var result = (VkSparseImageMemoryBindInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSparseImageMemoryBindInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkSparseImageMemoryBindInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkSparseImageMemoryBindInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkSparseImageMemoryBindInfo.Raw** MarshalIndirect(this IReadOnlyList<VkSparseImageMemoryBindInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSparseImageMemoryBindInfo.Raw**)0;
            var result = (VkSparseImageMemoryBindInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkSparseImageMemoryBindInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
