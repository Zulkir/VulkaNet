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
    public interface IVkRenderPassBeginInfo
    {
        IVkStructWrapper Next { get; }
        IVkRenderPass RenderPass { get; }
        IVkFramebuffer Framebuffer { get; }
        VkRect2D RenderArea { get; }
        IReadOnlyList<VkClearValue> ClearValues { get; }
    }

    public unsafe class VkRenderPassBeginInfo : IVkRenderPassBeginInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IVkRenderPass RenderPass { get; set; }
        public IVkFramebuffer Framebuffer { get; set; }
        public VkRect2D RenderArea { get; set; }
        public IReadOnlyList<VkClearValue> ClearValues { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkRenderPass.HandleType renderPass;
            public VkFramebuffer.HandleType framebuffer;
            public VkRect2D renderArea;
            public int clearValueCount;
            public VkClearValue* pClearValues;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkRenderPassBeginInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkRenderPassBeginInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.ClearValues.SizeOfMarshalDirect();
        }

        public static VkRenderPassBeginInfo.Raw MarshalDirect(this IVkRenderPassBeginInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pClearValues = s.ClearValues.MarshalDirect(ref unmanaged);

            VkRenderPassBeginInfo.Raw result;
            result.sType = VkStructureType.RenderPassBeginInfo;
            result.pNext = pNext;
            result.renderPass = s.RenderPass?.Handle ?? VkRenderPass.HandleType.Null;
            result.framebuffer = s.Framebuffer?.Handle ?? VkFramebuffer.HandleType.Null;
            result.renderArea = s.RenderArea;
            result.clearValueCount = s.ClearValues?.Count ?? 0;
            result.pClearValues = pClearValues;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkRenderPassBeginInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkRenderPassBeginInfo.Raw.SizeInBytes;

        public static VkRenderPassBeginInfo.Raw* MarshalIndirect(this IVkRenderPassBeginInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkRenderPassBeginInfo.Raw*)0;
            var result = (VkRenderPassBeginInfo.Raw*)unmanaged;
            unmanaged += VkRenderPassBeginInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkRenderPassBeginInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkRenderPassBeginInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkRenderPassBeginInfo.Raw* MarshalDirect(this IReadOnlyList<IVkRenderPassBeginInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkRenderPassBeginInfo.Raw*)0;
            var result = (VkRenderPassBeginInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkRenderPassBeginInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkRenderPassBeginInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkRenderPassBeginInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkRenderPassBeginInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkRenderPassBeginInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkRenderPassBeginInfo.Raw**)0;
            var result = (VkRenderPassBeginInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkRenderPassBeginInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
