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
    public unsafe class VkPipelineRasterizationStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineRasterizationStateCreateFlags Flags { get; set; }
        public bool DepthClampEnable { get; set; }
        public bool RasterizerDiscardEnable { get; set; }
        public VkPolygonMode PolygonMode { get; set; }
        public VkCullMode CullMode { get; set; }
        public VkFrontFace FrontFace { get; set; }
        public bool DepthBiasEnable { get; set; }
        public float DepthBiasConstantFactor { get; set; }
        public float DepthBiasClamp { get; set; }
        public float DepthBiasSlopeFactor { get; set; }
        public float LineWidth { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineRasterizationStateCreateFlags flags;
            public VkBool32 depthClampEnable;
            public VkBool32 rasterizerDiscardEnable;
            public VkPolygonMode polygonMode;
            public VkCullMode cullMode;
            public VkFrontFace frontFace;
            public VkBool32 depthBiasEnable;
            public float depthBiasConstantFactor;
            public float depthBiasClamp;
            public float depthBiasSlopeFactor;
            public float lineWidth;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineRasterizationStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineRasterizationStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkPipelineRasterizationStateCreateInfo.Raw MarshalDirect(this VkPipelineRasterizationStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkPipelineRasterizationStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineRasterizationStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.depthClampEnable = new VkBool32(s.DepthClampEnable);
            result.rasterizerDiscardEnable = new VkBool32(s.RasterizerDiscardEnable);
            result.polygonMode = s.PolygonMode;
            result.cullMode = s.CullMode;
            result.frontFace = s.FrontFace;
            result.depthBiasEnable = new VkBool32(s.DepthBiasEnable);
            result.depthBiasConstantFactor = s.DepthBiasConstantFactor;
            result.depthBiasClamp = s.DepthBiasClamp;
            result.depthBiasSlopeFactor = s.DepthBiasSlopeFactor;
            result.lineWidth = s.LineWidth;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineRasterizationStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineRasterizationStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineRasterizationStateCreateInfo.Raw* MarshalIndirect(this VkPipelineRasterizationStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineRasterizationStateCreateInfo.Raw*)0;
            var result = (VkPipelineRasterizationStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineRasterizationStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineRasterizationStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineRasterizationStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineRasterizationStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineRasterizationStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineRasterizationStateCreateInfo.Raw*)0;
            var result = (VkPipelineRasterizationStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineRasterizationStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
