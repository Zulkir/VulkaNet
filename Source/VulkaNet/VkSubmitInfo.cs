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
    public interface IVkSubmitInfo
    {
        IVkStructWrapper Next { get; }
        IReadOnlyList<IVkSemaphore> WaitSemaphores { get; }
        IReadOnlyList<VkPipelineStageFlags> WaitDstStageMask { get; }
        IReadOnlyList<IVkCommandBuffer> CommandBuffers { get; }
        IReadOnlyList<IVkSemaphore> SignalSemaphores { get; }
    }

    public unsafe class VkSubmitInfo : IVkSubmitInfo
    {
        public IVkStructWrapper Next { get; set; }
        public IReadOnlyList<IVkSemaphore> WaitSemaphores { get; set; }
        public IReadOnlyList<VkPipelineStageFlags> WaitDstStageMask { get; set; }
        public IReadOnlyList<IVkCommandBuffer> CommandBuffers { get; set; }
        public IReadOnlyList<IVkSemaphore> SignalSemaphores { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public int waitSemaphoreCount;
            public VkSemaphore.HandleType* pWaitSemaphores;
            public VkPipelineStageFlags* pWaitDstStageMask;
            public int commandBufferCount;
            public VkCommandBuffer.HandleType* pCommandBuffers;
            public int signalSemaphoreCount;
            public VkSemaphore.HandleType* pSignalSemaphores;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSubmitInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkSubmitInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.WaitSemaphores.SizeOfMarshalDirect() +
                s.WaitDstStageMask.SizeOfMarshalDirect() +
                s.CommandBuffers.SizeOfMarshalDirect() +
                s.SignalSemaphores.SizeOfMarshalDirect();
        }

        public static VkSubmitInfo.Raw MarshalDirect(this IVkSubmitInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pWaitSemaphores = s.WaitSemaphores.MarshalDirect(ref unmanaged);
            var pWaitDstStageMask = s.WaitDstStageMask.MarshalDirect(ref unmanaged);
            var pCommandBuffers = s.CommandBuffers.MarshalDirect(ref unmanaged);
            var pSignalSemaphores = s.SignalSemaphores.MarshalDirect(ref unmanaged);

            VkSubmitInfo.Raw result;
            result.sType = VkStructureType.SubmitInfo;
            result.pNext = pNext;
            result.waitSemaphoreCount = s.WaitSemaphores?.Count ?? 0;
            result.pWaitSemaphores = pWaitSemaphores;
            result.pWaitDstStageMask = pWaitDstStageMask;
            result.commandBufferCount = s.CommandBuffers?.Count ?? 0;
            result.pCommandBuffers = pCommandBuffers;
            result.signalSemaphoreCount = s.SignalSemaphores?.Count ?? 0;
            result.pSignalSemaphores = pSignalSemaphores;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkSubmitInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSubmitInfo.Raw.SizeInBytes;

        public static VkSubmitInfo.Raw* MarshalIndirect(this IVkSubmitInfo s, ref byte* unmanaged)
        {
            var result = (VkSubmitInfo.Raw*)unmanaged;
            unmanaged += VkSubmitInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkSubmitInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSubmitInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSubmitInfo.Raw* MarshalDirect(this IReadOnlyList<IVkSubmitInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubmitInfo.Raw*)0;
            var result = (VkSubmitInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSubmitInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkSubmitInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkSubmitInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkSubmitInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkSubmitInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubmitInfo.Raw**)0;
            var result = (VkSubmitInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkSubmitInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
