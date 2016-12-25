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
    public interface IVkGraphicsPipelineCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineCreateFlags Flags { get; }
        IReadOnlyList<IVkPipelineShaderStageCreateInfo> Stages { get; }
        IVkPipelineVertexInputStateCreateInfo VertexInputState { get; }
        IVkPipelineInputAssemblyStateCreateInfo InputAssemblyState { get; }
        IVkPipelineTessellationStateCreateInfo TessellationState { get; }
        IVkPipelineViewportStateCreateInfo ViewportState { get; }
        IVkPipelineRasterizationStateCreateInfo RasterizationState { get; }
        IVkPipelineMultisampleStateCreateInfo MultisampleState { get; }
        IVkPipelineDepthStencilStateCreateInfo DepthStencilState { get; }
        IVkPipelineColorBlendStateCreateInfo ColorBlendState { get; }
        IVkPipelineDynamicStateCreateInfo DynamicState { get; }
        IVkPipelineLayout Layout { get; }
        IVkRenderPass RenderPass { get; }
        int Subpass { get; }
        IVkPipeline BasePipelineHandle { get; }
        int BasePipelineIndex { get; }
    }

    public unsafe class VkGraphicsPipelineCreateInfo : IVkGraphicsPipelineCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineCreateFlags Flags { get; set; }
        public IReadOnlyList<IVkPipelineShaderStageCreateInfo> Stages { get; set; }
        public IVkPipelineVertexInputStateCreateInfo VertexInputState { get; set; }
        public IVkPipelineInputAssemblyStateCreateInfo InputAssemblyState { get; set; }
        public IVkPipelineTessellationStateCreateInfo TessellationState { get; set; }
        public IVkPipelineViewportStateCreateInfo ViewportState { get; set; }
        public IVkPipelineRasterizationStateCreateInfo RasterizationState { get; set; }
        public IVkPipelineMultisampleStateCreateInfo MultisampleState { get; set; }
        public IVkPipelineDepthStencilStateCreateInfo DepthStencilState { get; set; }
        public IVkPipelineColorBlendStateCreateInfo ColorBlendState { get; set; }
        public IVkPipelineDynamicStateCreateInfo DynamicState { get; set; }
        public IVkPipelineLayout Layout { get; set; }
        public IVkRenderPass RenderPass { get; set; }
        public int Subpass { get; set; }
        public IVkPipeline BasePipelineHandle { get; set; }
        public int BasePipelineIndex { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineCreateFlags flags;
            public int stageCount;
            public VkPipelineShaderStageCreateInfo.Raw* pStages;
            public VkPipelineVertexInputStateCreateInfo.Raw* pVertexInputState;
            public VkPipelineInputAssemblyStateCreateInfo.Raw* pInputAssemblyState;
            public VkPipelineTessellationStateCreateInfo.Raw* pTessellationState;
            public VkPipelineViewportStateCreateInfo.Raw* pViewportState;
            public VkPipelineRasterizationStateCreateInfo.Raw* pRasterizationState;
            public VkPipelineMultisampleStateCreateInfo.Raw* pMultisampleState;
            public VkPipelineDepthStencilStateCreateInfo.Raw* pDepthStencilState;
            public VkPipelineColorBlendStateCreateInfo.Raw* pColorBlendState;
            public VkPipelineDynamicStateCreateInfo.Raw* pDynamicState;
            public VkPipelineLayout.HandleType layout;
            public VkRenderPass.HandleType renderPass;
            public int subpass;
            public VkPipeline.HandleType basePipelineHandle;
            public int basePipelineIndex;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkGraphicsPipelineCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkGraphicsPipelineCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Stages.SizeOfMarshalDirect() +
                s.VertexInputState.SizeOfMarshalIndirect() +
                s.InputAssemblyState.SizeOfMarshalIndirect() +
                s.TessellationState.SizeOfMarshalIndirect() +
                s.ViewportState.SizeOfMarshalIndirect() +
                s.RasterizationState.SizeOfMarshalIndirect() +
                s.MultisampleState.SizeOfMarshalIndirect() +
                s.DepthStencilState.SizeOfMarshalIndirect() +
                s.ColorBlendState.SizeOfMarshalIndirect() +
                s.DynamicState.SizeOfMarshalIndirect();
        }

        public static VkGraphicsPipelineCreateInfo.Raw MarshalDirect(this IVkGraphicsPipelineCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pStages = s.Stages.MarshalDirect(ref unmanaged);
            var pVertexInputState = s.VertexInputState.MarshalIndirect(ref unmanaged);
            var pInputAssemblyState = s.InputAssemblyState.MarshalIndirect(ref unmanaged);
            var pTessellationState = s.TessellationState.MarshalIndirect(ref unmanaged);
            var pViewportState = s.ViewportState.MarshalIndirect(ref unmanaged);
            var pRasterizationState = s.RasterizationState.MarshalIndirect(ref unmanaged);
            var pMultisampleState = s.MultisampleState.MarshalIndirect(ref unmanaged);
            var pDepthStencilState = s.DepthStencilState.MarshalIndirect(ref unmanaged);
            var pColorBlendState = s.ColorBlendState.MarshalIndirect(ref unmanaged);
            var pDynamicState = s.DynamicState.MarshalIndirect(ref unmanaged);

            VkGraphicsPipelineCreateInfo.Raw result;
            result.sType = VkStructureType.GraphicsPipelineCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.stageCount = s.Stages?.Count ?? 0;
            result.pStages = pStages;
            result.pVertexInputState = pVertexInputState;
            result.pInputAssemblyState = pInputAssemblyState;
            result.pTessellationState = pTessellationState;
            result.pViewportState = pViewportState;
            result.pRasterizationState = pRasterizationState;
            result.pMultisampleState = pMultisampleState;
            result.pDepthStencilState = pDepthStencilState;
            result.pColorBlendState = pColorBlendState;
            result.pDynamicState = pDynamicState;
            result.layout = s.Layout?.Handle ?? VkPipelineLayout.HandleType.Null;
            result.renderPass = s.RenderPass?.Handle ?? VkRenderPass.HandleType.Null;
            result.subpass = s.Subpass;
            result.basePipelineHandle = s.BasePipelineHandle?.Handle ?? VkPipeline.HandleType.Null;
            result.basePipelineIndex = s.BasePipelineIndex;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkGraphicsPipelineCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkGraphicsPipelineCreateInfo.Raw.SizeInBytes;

        public static VkGraphicsPipelineCreateInfo.Raw* MarshalIndirect(this IVkGraphicsPipelineCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkGraphicsPipelineCreateInfo.Raw*)0;
            var result = (VkGraphicsPipelineCreateInfo.Raw*)unmanaged;
            unmanaged += VkGraphicsPipelineCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkGraphicsPipelineCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkGraphicsPipelineCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkGraphicsPipelineCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkGraphicsPipelineCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkGraphicsPipelineCreateInfo.Raw*)0;
            var result = (VkGraphicsPipelineCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkGraphicsPipelineCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkGraphicsPipelineCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkGraphicsPipelineCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkGraphicsPipelineCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkGraphicsPipelineCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkGraphicsPipelineCreateInfo.Raw**)0;
            var result = (VkGraphicsPipelineCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkGraphicsPipelineCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
