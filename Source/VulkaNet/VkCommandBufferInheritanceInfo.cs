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

using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkCommandBufferInheritanceInfo : IVkStructWrapper
    {
        IVkStructWrapper Next { get; }
        VkRenderPass.HandleType RenderPass { get; }
        int Subpass { get; }
        VkFramebuffer.HandleType Framebuffer { get; }
        bool OcclusionQueryEnable { get; }
        VkQueryControlFlags QueryFlags { get; }
        VkQueryPipelineStatisticFlags PipelineStatistics { get; }
    }

    public unsafe class VkCommandBufferInheritanceInfo : IVkCommandBufferInheritanceInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkRenderPass.HandleType RenderPass { get; set; }
        public int Subpass { get; set; }
        public VkFramebuffer.HandleType Framebuffer { get; set; }
        public bool OcclusionQueryEnable { get; set; }
        public VkQueryControlFlags QueryFlags { get; set; }
        public VkQueryPipelineStatisticFlags PipelineStatistics { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkRenderPass.HandleType renderPass;
            public int subpass;
            public VkFramebuffer.HandleType framebuffer;
            public VkBool32 occlusionQueryEnable;
            public VkQueryControlFlags queryFlags;
            public VkQueryPipelineStatisticFlags pipelineStatistics;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public int MarshalSize() =>
            Next.SafeMarshalSize() +
            Raw.SizeInBytes;

        public Raw* MarshalTo(ref byte* unmanaged)
        {
            var pNext = Next.SafeMarshalTo(ref unmanaged);

            var result = (Raw*)unmanaged;
            unmanaged += Raw.SizeInBytes;
            result->sType = VkStructureType.CommandBufferInheritanceInfo;
            result->pNext = pNext;
            result->renderPass = RenderPass;
            result->subpass = Subpass;
            result->framebuffer = Framebuffer;
            result->occlusionQueryEnable = new VkBool32(OcclusionQueryEnable);
            result->queryFlags = QueryFlags;
            result->pipelineStatistics = PipelineStatistics;
            return result;
        }

        void* IVkStructWrapper.MarshalTo(ref byte* unmanaged) =>
            MarshalTo(ref unmanaged);
    }

    public static unsafe class VkCommandBufferInheritanceInfoExtensions
    {
        public static int SafeMarshalSize(this IVkCommandBufferInheritanceInfo s) =>
            s?.MarshalSize() ?? 0;

        public static VkCommandBufferInheritanceInfo.Raw* SafeMarshalTo(this IVkCommandBufferInheritanceInfo s, ref byte* unmanaged) =>
            (VkCommandBufferInheritanceInfo.Raw*)(s != null ? s.MarshalTo(ref unmanaged) : (void*)0);
    }
}
