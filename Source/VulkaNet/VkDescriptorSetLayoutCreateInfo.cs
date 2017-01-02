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
    public unsafe class VkDescriptorSetLayoutCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkDescriptorSetLayoutCreateFlags Flags { get; set; }
        public IReadOnlyList<VkDescriptorSetLayoutBinding> Bindings { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDescriptorSetLayoutCreateFlags flags;
            public int bindingCount;
            public VkDescriptorSetLayoutBinding.Raw* pBindings;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDescriptorSetLayoutCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkDescriptorSetLayoutCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Bindings.SizeOfMarshalDirect();
        }

        public static VkDescriptorSetLayoutCreateInfo.Raw MarshalDirect(this VkDescriptorSetLayoutCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pBindings = s.Bindings.MarshalDirect(ref unmanaged);

            VkDescriptorSetLayoutCreateInfo.Raw result;
            result.sType = VkStructureType.DescriptorSetLayoutCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.bindingCount = s.Bindings?.Count ?? 0;
            result.pBindings = pBindings;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDescriptorSetLayoutCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDescriptorSetLayoutCreateInfo.Raw.SizeInBytes;

        public static VkDescriptorSetLayoutCreateInfo.Raw* MarshalIndirect(this VkDescriptorSetLayoutCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDescriptorSetLayoutCreateInfo.Raw*)0;
            var result = (VkDescriptorSetLayoutCreateInfo.Raw*)unmanaged;
            unmanaged += VkDescriptorSetLayoutCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDescriptorSetLayoutCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDescriptorSetLayoutCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDescriptorSetLayoutCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkDescriptorSetLayoutCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorSetLayoutCreateInfo.Raw*)0;
            var result = (VkDescriptorSetLayoutCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDescriptorSetLayoutCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
