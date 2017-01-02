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
    public unsafe class VkCopyDescriptorSet
    {
        public IVkStructWrapper Next { get; set; }
        public IVkDescriptorSet SrcSet { get; set; }
        public int SrcBinding { get; set; }
        public int SrcArrayElement { get; set; }
        public IVkDescriptorSet DstSet { get; set; }
        public int DstBinding { get; set; }
        public int DstArrayElement { get; set; }
        public int DescriptorCount { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDescriptorSet.HandleType srcSet;
            public int srcBinding;
            public int srcArrayElement;
            public VkDescriptorSet.HandleType dstSet;
            public int dstBinding;
            public int dstArrayElement;
            public int descriptorCount;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkCopyDescriptorSetExtensions
    {
        public static int SizeOfMarshalDirect(this VkCopyDescriptorSet s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkCopyDescriptorSet.Raw MarshalDirect(this VkCopyDescriptorSet s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkCopyDescriptorSet.Raw result;
            result.sType = VkStructureType.CopyDescriptorSet;
            result.pNext = pNext;
            result.srcSet = s.SrcSet?.Handle ?? VkDescriptorSet.HandleType.Null;
            result.srcBinding = s.SrcBinding;
            result.srcArrayElement = s.SrcArrayElement;
            result.dstSet = s.DstSet?.Handle ?? VkDescriptorSet.HandleType.Null;
            result.dstBinding = s.DstBinding;
            result.dstArrayElement = s.DstArrayElement;
            result.descriptorCount = s.DescriptorCount;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkCopyDescriptorSet s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkCopyDescriptorSet.Raw.SizeInBytes;

        public static VkCopyDescriptorSet.Raw* MarshalIndirect(this VkCopyDescriptorSet s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkCopyDescriptorSet.Raw*)0;
            var result = (VkCopyDescriptorSet.Raw*)unmanaged;
            unmanaged += VkCopyDescriptorSet.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkCopyDescriptorSet> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkCopyDescriptorSet.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkCopyDescriptorSet.Raw* MarshalDirect(this IReadOnlyList<VkCopyDescriptorSet> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCopyDescriptorSet.Raw*)0;
            var result = (VkCopyDescriptorSet.Raw*)unmanaged;
            unmanaged += sizeof(VkCopyDescriptorSet.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
