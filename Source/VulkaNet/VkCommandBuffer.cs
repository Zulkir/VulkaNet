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

namespace VulkaNet
{
    public interface IVkCommandBuffer : IVkHandledObject, IVkDeviceChild
    {
        VkCommandBuffer.HandleType Handle { get; }
        VkResult Reset(VkCommandBufferResetFlags flags);
        VkResult Begin(IVkCommandBufferBeginInfo beginInfo);
        VkResult End();
        void CmdExecuteCommands(IReadOnlyList<IVkCommandBuffer> commandBuffers);
        void CmdSetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask);
        void CmdResetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask);
        void CmdWaitEvents(IReadOnlyList<IVkEvent> events, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, IReadOnlyList<IVkMemoryBarrier> memoryBarriers, IReadOnlyList<IVkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<IVkImageMemoryBarrier> imageMemoryBarriers);
        void CmdPipelineBarrier(VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, IReadOnlyList<IVkMemoryBarrier> memoryBarriers, IReadOnlyList<IVkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<IVkImageMemoryBarrier> imageMemoryBarriers);
    }

    public unsafe class VkCommandBuffer : IVkCommandBuffer
    {
        public IVkDevice Device { get; }
        public HandleType Handle { get; }

        private VkDevice.DirectFunctions Direct => Device.Direct;

        public IntPtr RawHandle => Handle.InternalHandle;

        public VkCommandBuffer(IVkDevice device, HandleType handle)
        {
            Device = device;
            Handle = handle;
        }

        public struct HandleType
        {
            public readonly IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = IntPtr.Size;
            public static HandleType Null => new HandleType(default(IntPtr));
        }

        public VkResult Reset(VkCommandBufferResetFlags flags)
        {
            var _commandBuffer = Handle;
            var _flags = flags;
            return Direct.ResetCommandBuffer(_commandBuffer, _flags);
        }

        public VkResult Begin(IVkCommandBufferBeginInfo beginInfo)
        {
            var unmanagedSize =
                beginInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _pBeginInfo = beginInfo.MarshalIndirect(ref unmanaged);
                return Direct.BeginCommandBuffer(_commandBuffer, _pBeginInfo);
            }
        }

        public VkResult End()
        {
            var _commandBuffer = Handle;
            return Direct.EndCommandBuffer(_commandBuffer);
        }

        public void CmdExecuteCommands(IReadOnlyList<IVkCommandBuffer> commandBuffers)
        {
            var unmanagedSize =
                commandBuffers.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _commandBufferCount = commandBuffers?.Count ?? 0;
                var _pCommandBuffers = commandBuffers.MarshalDirect(ref unmanaged);
                Direct.CmdExecuteCommands(_commandBuffer, _commandBufferCount, _pCommandBuffers);
            }
        }

        public void CmdSetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask)
        {
            var _commandBuffer = Handle;
            var _eventObj = eventObj?.Handle ?? VkEvent.HandleType.Null;
            var _stageMask = stageMask;
            Direct.CmdSetEvent(_commandBuffer, _eventObj, _stageMask);
        }

        public void CmdResetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask)
        {
            var _commandBuffer = Handle;
            var _eventObj = eventObj?.Handle ?? VkEvent.HandleType.Null;
            var _stageMask = stageMask;
            Direct.CmdResetEvent(_commandBuffer, _eventObj, _stageMask);
        }

        public void CmdWaitEvents(IReadOnlyList<IVkEvent> events, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, IReadOnlyList<IVkMemoryBarrier> memoryBarriers, IReadOnlyList<IVkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<IVkImageMemoryBarrier> imageMemoryBarriers)
        {
            var unmanagedSize =
                events.SizeOfMarshalDirect() +
                memoryBarriers.SizeOfMarshalDirect() +
                bufferMemoryBarriers.SizeOfMarshalDirect() +
                imageMemoryBarriers.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _eventCount = events?.Count ?? 0;
                var _pEvents = events.MarshalDirect(ref unmanaged);
                var _srcStageMask = srcStageMask;
                var _dstStageMask = dstStageMask;
                var _memoryBarrierCount = memoryBarriers?.Count ?? 0;
                var _pMemoryBarriers = memoryBarriers.MarshalDirect(ref unmanaged);
                var _bufferMemoryBarrierCount = bufferMemoryBarriers?.Count ?? 0;
                var _pBufferMemoryBarriers = bufferMemoryBarriers.MarshalDirect(ref unmanaged);
                var _imageMemoryBarrierCount = imageMemoryBarriers?.Count ?? 0;
                var _pImageMemoryBarriers = imageMemoryBarriers.MarshalDirect(ref unmanaged);
                Direct.CmdWaitEvents(_commandBuffer, _eventCount, _pEvents, _srcStageMask, _dstStageMask, _memoryBarrierCount, _pMemoryBarriers, _bufferMemoryBarrierCount, _pBufferMemoryBarriers, _imageMemoryBarrierCount, _pImageMemoryBarriers);
            }
        }

        public void CmdPipelineBarrier(VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, IReadOnlyList<IVkMemoryBarrier> memoryBarriers, IReadOnlyList<IVkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<IVkImageMemoryBarrier> imageMemoryBarriers)
        {
            var unmanagedSize =
                memoryBarriers.SizeOfMarshalDirect() +
                bufferMemoryBarriers.SizeOfMarshalDirect() +
                imageMemoryBarriers.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcStageMask = srcStageMask;
                var _dstStageMask = dstStageMask;
                var _dependencyFlags = dependencyFlags;
                var _memoryBarrierCount = memoryBarriers?.Count ?? 0;
                var _pMemoryBarriers = memoryBarriers.MarshalDirect(ref unmanaged);
                var _bufferMemoryBarrierCount = bufferMemoryBarriers?.Count ?? 0;
                var _pBufferMemoryBarriers = bufferMemoryBarriers.MarshalDirect(ref unmanaged);
                var _imageMemoryBarrierCount = imageMemoryBarriers?.Count ?? 0;
                var _pImageMemoryBarriers = imageMemoryBarriers.MarshalDirect(ref unmanaged);
                Direct.CmdPipelineBarrier(_commandBuffer, _srcStageMask, _dstStageMask, _dependencyFlags, _memoryBarrierCount, _pMemoryBarriers, _bufferMemoryBarrierCount, _pBufferMemoryBarriers, _imageMemoryBarrierCount, _pImageMemoryBarriers);
            }
        }

    }

    public static unsafe class VkCommandBufferExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandBuffer> list) =>
            list.SizeOfMarshalDirectDispatchable();

        public static VkCommandBuffer.HandleType* MarshalDirect(this IReadOnlyList<IVkCommandBuffer> list, ref byte* unmanaged) =>
            (VkCommandBuffer.HandleType*)list.MarshalDirectDispatchable(ref unmanaged);
    }
}
