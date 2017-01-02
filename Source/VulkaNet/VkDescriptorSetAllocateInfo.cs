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
    public unsafe class VkDescriptorSetAllocateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IVkDescriptorPool DescriptorPool { get; set; }
        public IReadOnlyList<IVkDescriptorSetLayout> SetLayouts { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDescriptorPool.HandleType descriptorPool;
            public int descriptorSetCount;
            public VkDescriptorSetLayout.HandleType* pSetLayouts;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDescriptorSetAllocateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkDescriptorSetAllocateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.SetLayouts.SizeOfMarshalDirect();
        }

        public static VkDescriptorSetAllocateInfo.Raw MarshalDirect(this VkDescriptorSetAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pSetLayouts = s.SetLayouts.MarshalDirect(ref unmanaged);

            VkDescriptorSetAllocateInfo.Raw result;
            result.sType = VkStructureType.DescriptorSetAllocateInfo;
            result.pNext = pNext;
            result.descriptorPool = s.DescriptorPool?.Handle ?? VkDescriptorPool.HandleType.Null;
            result.descriptorSetCount = s.SetLayouts?.Count ?? 0;
            result.pSetLayouts = pSetLayouts;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDescriptorSetAllocateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDescriptorSetAllocateInfo.Raw.SizeInBytes;

        public static VkDescriptorSetAllocateInfo.Raw* MarshalIndirect(this VkDescriptorSetAllocateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDescriptorSetAllocateInfo.Raw*)0;
            var result = (VkDescriptorSetAllocateInfo.Raw*)unmanaged;
            unmanaged += VkDescriptorSetAllocateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDescriptorSetAllocateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDescriptorSetAllocateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDescriptorSetAllocateInfo.Raw* MarshalDirect(this IReadOnlyList<VkDescriptorSetAllocateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorSetAllocateInfo.Raw*)0;
            var result = (VkDescriptorSetAllocateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDescriptorSetAllocateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
