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
    public interface IVkPipelineVertexInputStateCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineVertexInputStateCreateFlags Flags { get; }
        IReadOnlyList<VkVertexInputBindingDescription> VertexBindingDescriptions { get; }
        IReadOnlyList<VkVertexInputAttributeDescription> VertexAttributeDescriptions { get; }
    }

    public unsafe class VkPipelineVertexInputStateCreateInfo : IVkPipelineVertexInputStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineVertexInputStateCreateFlags Flags { get; set; }
        public IReadOnlyList<VkVertexInputBindingDescription> VertexBindingDescriptions { get; set; }
        public IReadOnlyList<VkVertexInputAttributeDescription> VertexAttributeDescriptions { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineVertexInputStateCreateFlags flags;
            public int vertexBindingDescriptionCount;
            public VkVertexInputBindingDescription* pVertexBindingDescriptions;
            public int vertexAttributeDescriptionCount;
            public VkVertexInputAttributeDescription* pVertexAttributeDescriptions;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineVertexInputStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkPipelineVertexInputStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.VertexBindingDescriptions.SizeOfMarshalDirect() +
                s.VertexAttributeDescriptions.SizeOfMarshalDirect();
        }

        public static VkPipelineVertexInputStateCreateInfo.Raw MarshalDirect(this IVkPipelineVertexInputStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pVertexBindingDescriptions = s.VertexBindingDescriptions.MarshalDirect(ref unmanaged);
            var pVertexAttributeDescriptions = s.VertexAttributeDescriptions.MarshalDirect(ref unmanaged);

            VkPipelineVertexInputStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineVertexInputStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.vertexBindingDescriptionCount = s.VertexBindingDescriptions?.Count ?? 0;
            result.pVertexBindingDescriptions = pVertexBindingDescriptions;
            result.vertexAttributeDescriptionCount = s.VertexAttributeDescriptions?.Count ?? 0;
            result.pVertexAttributeDescriptions = pVertexAttributeDescriptions;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkPipelineVertexInputStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineVertexInputStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineVertexInputStateCreateInfo.Raw* MarshalIndirect(this IVkPipelineVertexInputStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineVertexInputStateCreateInfo.Raw*)0;
            var result = (VkPipelineVertexInputStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineVertexInputStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkPipelineVertexInputStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineVertexInputStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineVertexInputStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkPipelineVertexInputStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineVertexInputStateCreateInfo.Raw*)0;
            var result = (VkPipelineVertexInputStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineVertexInputStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkPipelineVertexInputStateCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkPipelineVertexInputStateCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkPipelineVertexInputStateCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkPipelineVertexInputStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineVertexInputStateCreateInfo.Raw**)0;
            var result = (VkPipelineVertexInputStateCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkPipelineVertexInputStateCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
