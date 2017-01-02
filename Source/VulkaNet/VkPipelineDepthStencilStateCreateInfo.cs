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
    public unsafe class VkPipelineDepthStencilStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineDepthStencilStateCreateFlags Flags { get; set; }
        public bool DepthTestEnable { get; set; }
        public bool DepthWriteEnable { get; set; }
        public VkCompareOp DepthCompareOp { get; set; }
        public bool DepthBoundsTestEnable { get; set; }
        public bool StencilTestEnable { get; set; }
        public VkStencilOpState Front { get; set; }
        public VkStencilOpState Back { get; set; }
        public float MinDepthBounds { get; set; }
        public float MaxDepthBounds { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineDepthStencilStateCreateFlags flags;
            public VkBool32 depthTestEnable;
            public VkBool32 depthWriteEnable;
            public VkCompareOp depthCompareOp;
            public VkBool32 depthBoundsTestEnable;
            public VkBool32 stencilTestEnable;
            public VkStencilOpState front;
            public VkStencilOpState back;
            public float minDepthBounds;
            public float maxDepthBounds;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineDepthStencilStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineDepthStencilStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkPipelineDepthStencilStateCreateInfo.Raw MarshalDirect(this VkPipelineDepthStencilStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkPipelineDepthStencilStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineDepthStencilStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.depthTestEnable = new VkBool32(s.DepthTestEnable);
            result.depthWriteEnable = new VkBool32(s.DepthWriteEnable);
            result.depthCompareOp = s.DepthCompareOp;
            result.depthBoundsTestEnable = new VkBool32(s.DepthBoundsTestEnable);
            result.stencilTestEnable = new VkBool32(s.StencilTestEnable);
            result.front = s.Front;
            result.back = s.Back;
            result.minDepthBounds = s.MinDepthBounds;
            result.maxDepthBounds = s.MaxDepthBounds;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineDepthStencilStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineDepthStencilStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineDepthStencilStateCreateInfo.Raw* MarshalIndirect(this VkPipelineDepthStencilStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineDepthStencilStateCreateInfo.Raw*)0;
            var result = (VkPipelineDepthStencilStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineDepthStencilStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineDepthStencilStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineDepthStencilStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineDepthStencilStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineDepthStencilStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineDepthStencilStateCreateInfo.Raw*)0;
            var result = (VkPipelineDepthStencilStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineDepthStencilStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
