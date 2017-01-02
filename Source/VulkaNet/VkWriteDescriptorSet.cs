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
    public unsafe class VkWriteDescriptorSet
    {
        public IVkStructWrapper Next { get; set; }
        public IVkDescriptorSet DstSet { get; set; }
        public int DstBinding { get; set; }
        public int DstArrayElement { get; set; }
        public int DescriptorCount { get; set; }
        public VkDescriptorType DescriptorType { get; set; }
        public IReadOnlyList<VkDescriptorImageInfo> ImageInfo { get; set; }
        public IReadOnlyList<VkDescriptorBufferInfo> BufferInfo { get; set; }
        public IReadOnlyList<IVkBufferView> TexelBufferView { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDescriptorSet.HandleType dstSet;
            public int dstBinding;
            public int dstArrayElement;
            public int descriptorCount;
            public VkDescriptorType descriptorType;
            public VkDescriptorImageInfo.Raw* pImageInfo;
            public VkDescriptorBufferInfo.Raw* pBufferInfo;
            public VkBufferView.HandleType* pTexelBufferView;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkWriteDescriptorSetExtensions
    {
        public static int SizeOfMarshalDirect(this VkWriteDescriptorSet s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.ImageInfo.SizeOfMarshalDirect() +
                s.BufferInfo.SizeOfMarshalDirect() +
                s.TexelBufferView.SizeOfMarshalDirect();
        }

        public static VkWriteDescriptorSet.Raw MarshalDirect(this VkWriteDescriptorSet s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pImageInfo = s.ImageInfo.MarshalDirect(ref unmanaged);
            var pBufferInfo = s.BufferInfo.MarshalDirect(ref unmanaged);
            var pTexelBufferView = s.TexelBufferView.MarshalDirect(ref unmanaged);

            VkWriteDescriptorSet.Raw result;
            result.sType = VkStructureType.WriteDescriptorSet;
            result.pNext = pNext;
            result.dstSet = s.DstSet?.Handle ?? VkDescriptorSet.HandleType.Null;
            result.dstBinding = s.DstBinding;
            result.dstArrayElement = s.DstArrayElement;
            result.descriptorCount = s.DescriptorCount;
            result.descriptorType = s.DescriptorType;
            result.pImageInfo = pImageInfo;
            result.pBufferInfo = pBufferInfo;
            result.pTexelBufferView = pTexelBufferView;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkWriteDescriptorSet s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkWriteDescriptorSet.Raw.SizeInBytes;

        public static VkWriteDescriptorSet.Raw* MarshalIndirect(this VkWriteDescriptorSet s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkWriteDescriptorSet.Raw*)0;
            var result = (VkWriteDescriptorSet.Raw*)unmanaged;
            unmanaged += VkWriteDescriptorSet.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkWriteDescriptorSet> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkWriteDescriptorSet.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkWriteDescriptorSet.Raw* MarshalDirect(this IReadOnlyList<VkWriteDescriptorSet> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkWriteDescriptorSet.Raw*)0;
            var result = (VkWriteDescriptorSet.Raw*)unmanaged;
            unmanaged += sizeof(VkWriteDescriptorSet.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkWriteDescriptorSet> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkWriteDescriptorSet.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkWriteDescriptorSet.Raw** MarshalIndirect(this IReadOnlyList<VkWriteDescriptorSet> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkWriteDescriptorSet.Raw**)0;
            var result = (VkWriteDescriptorSet.Raw**)unmanaged;
            unmanaged += sizeof(VkWriteDescriptorSet.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
