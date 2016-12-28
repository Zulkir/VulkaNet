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
    public interface IVkPipelineLayoutCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineLayoutCreateFlags Flags { get; }
        IReadOnlyList<IVkDescriptorSetLayout> SetLayouts { get; }
        IReadOnlyList<VkPushConstantRange> PushConstantRanges { get; }
    }

    public unsafe class VkPipelineLayoutCreateInfo : IVkPipelineLayoutCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineLayoutCreateFlags Flags { get; set; }
        public IReadOnlyList<IVkDescriptorSetLayout> SetLayouts { get; set; }
        public IReadOnlyList<VkPushConstantRange> PushConstantRanges { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineLayoutCreateFlags flags;
            public int setLayoutCount;
            public VkDescriptorSetLayout.HandleType* pSetLayouts;
            public int pushConstantRangeCount;
            public VkPushConstantRange* pPushConstantRanges;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineLayoutCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkPipelineLayoutCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.SetLayouts.SizeOfMarshalDirect() +
                s.PushConstantRanges.SizeOfMarshalDirect();
        }

        public static VkPipelineLayoutCreateInfo.Raw MarshalDirect(this IVkPipelineLayoutCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pSetLayouts = s.SetLayouts.MarshalDirect(ref unmanaged);
            var pPushConstantRanges = s.PushConstantRanges.MarshalDirect(ref unmanaged);

            VkPipelineLayoutCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineLayoutCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.setLayoutCount = s.SetLayouts?.Count ?? 0;
            result.pSetLayouts = pSetLayouts;
            result.pushConstantRangeCount = s.PushConstantRanges?.Count ?? 0;
            result.pPushConstantRanges = pPushConstantRanges;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkPipelineLayoutCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineLayoutCreateInfo.Raw.SizeInBytes;

        public static VkPipelineLayoutCreateInfo.Raw* MarshalIndirect(this IVkPipelineLayoutCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineLayoutCreateInfo.Raw*)0;
            var result = (VkPipelineLayoutCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineLayoutCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkPipelineLayoutCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineLayoutCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineLayoutCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkPipelineLayoutCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineLayoutCreateInfo.Raw*)0;
            var result = (VkPipelineLayoutCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineLayoutCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkPipelineLayoutCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkPipelineLayoutCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkPipelineLayoutCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkPipelineLayoutCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineLayoutCreateInfo.Raw**)0;
            var result = (VkPipelineLayoutCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkPipelineLayoutCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
