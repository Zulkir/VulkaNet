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
        IVkRenderPass RenderPass { get; }
        int Subpass { get; }
        IVkFramebuffer Framebuffer { get; }
        bool OcclusionQueryEnable { get; }
        VkQueryControlFlags QueryFlags { get; }
        VkQueryPipelineStatisticFlags PipelineStatistics { get; }
    }

    public unsafe class VkCommandBufferInheritanceInfo : IVkCommandBufferInheritanceInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IVkRenderPass RenderPass { get; set; }
        public int Subpass { get; set; }
        public IVkFramebuffer Framebuffer { get; set; }
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
    }

    public static unsafe class VkCommandBufferInheritanceInfoExtensions
    {
        public int SizeOfMarshalDirect(this ICommandBufferInheritanceInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                Next.SizeOfMarshalIndirect();
        }

        public Raw* MarshalDirect(this IVkCommandBufferInheritanceInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkCommandBufferInheritanceInfo.Raw result;
            result.sType = VkStructureType.CommandBufferInheritanceInfo;
            result.pNext = pNext;
            result.renderPass = s.RenderPass;
            result.subpass = s.Subpass;
            result.framebuffer = s.Framebuffer;
            result.occlusionQueryEnable = new VkBool32(s.OcclusionQueryEnable);
            result.queryFlags = s.QueryFlags;
            result.pipelineStatistics = s.PipelineStatistics;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkCommandBufferInheritanceInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkCommandBufferInheritanceInfo.Raw.SizeInBytes;

        public static VkCommandBufferInheritanceInfo.Raw* MarshalIndirect(this IVkCommandBufferInheritanceInfo s, ref byte* unmanaged)
        {
            var result = (VkCommandBufferInheritanceInfo.Raw*)unmanaged;
            unmanaged += VkCommandBufferInheritanceInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandBufferInheritanceInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkCommandBufferInheritanceInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkCommandBufferInheritanceInfo.Raw* MarshalDirect(this IReadOnlyList<IVkCommandBufferInheritanceInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferInheritanceInfo.Raw*)0;
            var result = (VkCommandBufferInheritanceInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkCommandBufferInheritanceInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkCommandBufferInheritanceInfo> list)
            list == null || list.Count == 0
                ? 0
                : sizeof(VkCommandBufferInheritanceInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkCommandBufferInheritanceInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkCommandBufferInheritanceInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkCommandBufferInheritanceInfo.Raw**)0;
            var result = (VkCommandBufferInheritanceInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkCommandBufferInheritanceInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
