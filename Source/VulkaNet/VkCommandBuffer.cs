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
        VkResult Begin(VkCommandBufferBeginInfo beginInfo);
        VkResult End();
        void CmdExecuteCommands(IReadOnlyList<IVkCommandBuffer> commandBuffers);
        void CmdSetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask);
        void CmdResetEvent(IVkEvent eventObj, VkPipelineStageFlags stageMask);
        void CmdWaitEvents(IReadOnlyList<IVkEvent> events, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, IReadOnlyList<VkMemoryBarrier> memoryBarriers, IReadOnlyList<VkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<VkImageMemoryBarrier> imageMemoryBarriers);
        void CmdPipelineBarrier(VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, IReadOnlyList<VkMemoryBarrier> memoryBarriers, IReadOnlyList<VkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<VkImageMemoryBarrier> imageMemoryBarriers);
        void CmdBeginRenderPass(VkRenderPassBeginInfo renderPassBegin, VkSubpassContents contents);
        void CmdEndRenderPass();
        void CmdNextSubpass(VkSubpassContents contents);
        void CmdBindPipeline(VkPipelineBindPoint pipelineBindPoint, IVkPipeline pipeline);
        void CmdBindDescriptorSets(VkPipelineBindPoint pipelineBindPoint, IVkPipelineLayout layout, int firstSet, IReadOnlyList<IVkDescriptorSet> descriptorSets, IReadOnlyList<int> dynamicOffsets);
        void CmdPushConstants(IVkPipelineLayout layout, VkShaderStage stageFlags, int offset, int size, IntPtr values);
        void CmdResetQueryPool(IVkQueryPool queryPool, int firstQuery, int queryCount);
        void CmdBeginQuery(IVkQueryPool queryPool, int query, VkQueryControlFlags flags);
        void CmdEndQuery(IVkQueryPool queryPool, int query);
        void CmdCopyQueryPoolResults(IVkQueryPool queryPool, int firstQuery, int queryCount, IVkBuffer dstBuffer, ulong dstOffset, ulong stride, VkQueryResultFlags flags);
        void CmdWriteTimestamp(VkPipelineStageFlags pipelineStage, IVkQueryPool queryPool, int query);
        void CmdClearColorImage(IVkImage image, VkImageLayout imageLayout, VkClearColorValue color, IReadOnlyList<VkImageSubresourceRange> ranges);
        void CmdClearDepthStencilImage(IVkImage image, VkImageLayout imageLayout, VkClearDepthStencilValue depthStencil, IReadOnlyList<VkImageSubresourceRange> ranges);
        void CmdClearAttachments(IReadOnlyList<VkClearAttachment> attachments, IReadOnlyList<VkClearRect> rects);
        void CmdFillBuffer(IVkBuffer dstBuffer, ulong dstOffset, ulong size, int data);
        void CmdUpdateBuffer(IVkBuffer dstBuffer, ulong dstOffset, ulong dataSize, IntPtr data);
        void CmdCopyBuffer(IVkBuffer srcBuffer, IVkBuffer dstBuffer, IReadOnlyList<VkBufferCopy> regions);
        void CmdCopyImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageCopy> regions);
        void CmdCopyBufferToImage(IVkBuffer srcBuffer, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkBufferImageCopy> regions);
        void CmdCopyImageToBuffer(IVkImage srcImage, VkImageLayout srcImageLayout, IVkBuffer dstBuffer, IReadOnlyList<VkBufferImageCopy> regions);
        void CmdBlitImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageBlit> regions, VkFilter filter);
        void CmdResolveImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageResolve> regions);
        void CmdBindIndexBuffer(IVkBuffer buffer, ulong offset, VkIndexType indexType);
        void CmdDraw(int vertexCount, int instanceCount, int firstVertex, int firstInstance);
        void CmdDrawIndexed(int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance);
        void CmdDrawIndirect(IVkBuffer buffer, ulong offset, int drawCount, int stride);
        void CmdDrawIndexedIndirect(IVkBuffer buffer, ulong offset, int drawCount, int stride);
        void CmdBindVertexBuffers(int firstBinding, IReadOnlyList<IVkBuffer> buffers, IReadOnlyList<ulong> offsets);
        void CmdSetViewport(int firstViewport, IReadOnlyList<VkViewport> viewports);
        void CmdSetLineWidth(float lineWidth);
        void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
        void CmdSetScissor(int firstScissor, IReadOnlyList<VkRect2D> scissors);
        void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds);
        void CmdSetStencilCompareMask(VkStencilFaceFlags faceMask, int compareMask);
        void CmdSetStencilWriteMask(VkStencilFaceFlags faceMask, int writeMask);
        void CmdSetStencilReference(VkStencilFaceFlags faceMask, int reference);
        void CmdSetBlendConstants(VkColor4 blendConstants);
        void CmdDispatch(int x, int y, int z);
        void CmdDispatchIndirect(IVkBuffer buffer, ulong offset);
        void CmdDebugMarkerBeginEXT(VkDebugMarkerMarkerInfoEXT markerInfo);
        void CmdDebugMarkerEndEXT();
        void CmdDebugMarkerInsertEXT(VkDebugMarkerMarkerInfoEXT markerInfo);
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

        public VkResult Begin(VkCommandBufferBeginInfo beginInfo)
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

        public void CmdWaitEvents(IReadOnlyList<IVkEvent> events, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, IReadOnlyList<VkMemoryBarrier> memoryBarriers, IReadOnlyList<VkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<VkImageMemoryBarrier> imageMemoryBarriers)
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

        public void CmdPipelineBarrier(VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, IReadOnlyList<VkMemoryBarrier> memoryBarriers, IReadOnlyList<VkBufferMemoryBarrier> bufferMemoryBarriers, IReadOnlyList<VkImageMemoryBarrier> imageMemoryBarriers)
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

        public void CmdBeginRenderPass(VkRenderPassBeginInfo renderPassBegin, VkSubpassContents contents)
        {
            var unmanagedSize =
                renderPassBegin.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _pRenderPassBegin = renderPassBegin.MarshalIndirect(ref unmanaged);
                var _contents = contents;
                Direct.CmdBeginRenderPass(_commandBuffer, _pRenderPassBegin, _contents);
            }
        }

        public void CmdEndRenderPass()
        {
            var _commandBuffer = Handle;
            Direct.CmdEndRenderPass(_commandBuffer);
        }

        public void CmdNextSubpass(VkSubpassContents contents)
        {
            var _commandBuffer = Handle;
            var _contents = contents;
            Direct.CmdNextSubpass(_commandBuffer, _contents);
        }

        public void CmdBindPipeline(VkPipelineBindPoint pipelineBindPoint, IVkPipeline pipeline)
        {
            var _commandBuffer = Handle;
            var _pipelineBindPoint = pipelineBindPoint;
            var _pipeline = pipeline?.Handle ?? VkPipeline.HandleType.Null;
            Direct.CmdBindPipeline(_commandBuffer, _pipelineBindPoint, _pipeline);
        }

        public void CmdBindDescriptorSets(VkPipelineBindPoint pipelineBindPoint, IVkPipelineLayout layout, int firstSet, IReadOnlyList<IVkDescriptorSet> descriptorSets, IReadOnlyList<int> dynamicOffsets)
        {
            var unmanagedSize =
                descriptorSets.SizeOfMarshalDirect() +
                dynamicOffsets.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _pipelineBindPoint = pipelineBindPoint;
                var _layout = layout?.Handle ?? VkPipelineLayout.HandleType.Null;
                var _firstSet = firstSet;
                var _descriptorSetCount = descriptorSets?.Count ?? 0;
                var _pDescriptorSets = descriptorSets.MarshalDirect(ref unmanaged);
                var _dynamicOffsetCount = dynamicOffsets?.Count ?? 0;
                var _pDynamicOffsets = dynamicOffsets.MarshalDirect(ref unmanaged);
                Direct.CmdBindDescriptorSets(_commandBuffer, _pipelineBindPoint, _layout, _firstSet, _descriptorSetCount, _pDescriptorSets, _dynamicOffsetCount, _pDynamicOffsets);
            }
        }

        public void CmdPushConstants(IVkPipelineLayout layout, VkShaderStage stageFlags, int offset, int size, IntPtr values)
        {
            var _commandBuffer = Handle;
            var _layout = layout?.Handle ?? VkPipelineLayout.HandleType.Null;
            var _stageFlags = stageFlags;
            var _offset = offset;
            var _size = size;
            var _pValues = values;
            Direct.CmdPushConstants(_commandBuffer, _layout, _stageFlags, _offset, _size, _pValues);
        }

        public void CmdResetQueryPool(IVkQueryPool queryPool, int firstQuery, int queryCount)
        {
            var _commandBuffer = Handle;
            var _queryPool = queryPool?.Handle ?? VkQueryPool.HandleType.Null;
            var _firstQuery = firstQuery;
            var _queryCount = queryCount;
            Direct.CmdResetQueryPool(_commandBuffer, _queryPool, _firstQuery, _queryCount);
        }

        public void CmdBeginQuery(IVkQueryPool queryPool, int query, VkQueryControlFlags flags)
        {
            var _commandBuffer = Handle;
            var _queryPool = queryPool?.Handle ?? VkQueryPool.HandleType.Null;
            var _query = query;
            var _flags = flags;
            Direct.CmdBeginQuery(_commandBuffer, _queryPool, _query, _flags);
        }

        public void CmdEndQuery(IVkQueryPool queryPool, int query)
        {
            var _commandBuffer = Handle;
            var _queryPool = queryPool?.Handle ?? VkQueryPool.HandleType.Null;
            var _query = query;
            Direct.CmdEndQuery(_commandBuffer, _queryPool, _query);
        }

        public void CmdCopyQueryPoolResults(IVkQueryPool queryPool, int firstQuery, int queryCount, IVkBuffer dstBuffer, ulong dstOffset, ulong stride, VkQueryResultFlags flags)
        {
            var _commandBuffer = Handle;
            var _queryPool = queryPool?.Handle ?? VkQueryPool.HandleType.Null;
            var _firstQuery = firstQuery;
            var _queryCount = queryCount;
            var _dstBuffer = dstBuffer?.Handle ?? VkBuffer.HandleType.Null;
            var _dstOffset = dstOffset;
            var _stride = stride;
            var _flags = flags;
            Direct.CmdCopyQueryPoolResults(_commandBuffer, _queryPool, _firstQuery, _queryCount, _dstBuffer, _dstOffset, _stride, _flags);
        }

        public void CmdWriteTimestamp(VkPipelineStageFlags pipelineStage, IVkQueryPool queryPool, int query)
        {
            var _commandBuffer = Handle;
            var _pipelineStage = pipelineStage;
            var _queryPool = queryPool?.Handle ?? VkQueryPool.HandleType.Null;
            var _query = query;
            Direct.CmdWriteTimestamp(_commandBuffer, _pipelineStage, _queryPool, _query);
        }

        public void CmdClearColorImage(IVkImage image, VkImageLayout imageLayout, VkClearColorValue color, IReadOnlyList<VkImageSubresourceRange> ranges)
        {
            var unmanagedSize =
                ranges.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _image = image?.Handle ?? VkImage.HandleType.Null;
                var _imageLayout = imageLayout;
                var _pColor = &color;
                var _rangeCount = ranges?.Count ?? 0;
                var _pRanges = ranges.MarshalDirect(ref unmanaged);
                Direct.CmdClearColorImage(_commandBuffer, _image, _imageLayout, _pColor, _rangeCount, _pRanges);
            }
        }

        public void CmdClearDepthStencilImage(IVkImage image, VkImageLayout imageLayout, VkClearDepthStencilValue depthStencil, IReadOnlyList<VkImageSubresourceRange> ranges)
        {
            var unmanagedSize =
                ranges.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _image = image?.Handle ?? VkImage.HandleType.Null;
                var _imageLayout = imageLayout;
                var _pDepthStencil = &depthStencil;
                var _rangeCount = ranges?.Count ?? 0;
                var _pRanges = ranges.MarshalDirect(ref unmanaged);
                Direct.CmdClearDepthStencilImage(_commandBuffer, _image, _imageLayout, _pDepthStencil, _rangeCount, _pRanges);
            }
        }

        public void CmdClearAttachments(IReadOnlyList<VkClearAttachment> attachments, IReadOnlyList<VkClearRect> rects)
        {
            var unmanagedSize =
                attachments.SizeOfMarshalDirect() +
                rects.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _attachmentCount = attachments?.Count ?? 0;
                var _pAttachments = attachments.MarshalDirect(ref unmanaged);
                var _rectCount = rects?.Count ?? 0;
                var _pRects = rects.MarshalDirect(ref unmanaged);
                Direct.CmdClearAttachments(_commandBuffer, _attachmentCount, _pAttachments, _rectCount, _pRects);
            }
        }

        public void CmdFillBuffer(IVkBuffer dstBuffer, ulong dstOffset, ulong size, int data)
        {
            var _commandBuffer = Handle;
            var _dstBuffer = dstBuffer?.Handle ?? VkBuffer.HandleType.Null;
            var _dstOffset = dstOffset;
            var _size = size;
            var _data = data;
            Direct.CmdFillBuffer(_commandBuffer, _dstBuffer, _dstOffset, _size, _data);
        }

        public void CmdUpdateBuffer(IVkBuffer dstBuffer, ulong dstOffset, ulong dataSize, IntPtr data)
        {
            var _commandBuffer = Handle;
            var _dstBuffer = dstBuffer?.Handle ?? VkBuffer.HandleType.Null;
            var _dstOffset = dstOffset;
            var _dataSize = dataSize;
            var _pData = data;
            Direct.CmdUpdateBuffer(_commandBuffer, _dstBuffer, _dstOffset, _dataSize, _pData);
        }

        public void CmdCopyBuffer(IVkBuffer srcBuffer, IVkBuffer dstBuffer, IReadOnlyList<VkBufferCopy> regions)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcBuffer = srcBuffer?.Handle ?? VkBuffer.HandleType.Null;
                var _dstBuffer = dstBuffer?.Handle ?? VkBuffer.HandleType.Null;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                Direct.CmdCopyBuffer(_commandBuffer, _srcBuffer, _dstBuffer, _regionCount, _pRegions);
            }
        }

        public void CmdCopyImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageCopy> regions)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcImage = srcImage?.Handle ?? VkImage.HandleType.Null;
                var _srcImageLayout = srcImageLayout;
                var _dstImage = dstImage?.Handle ?? VkImage.HandleType.Null;
                var _dstImageLayout = dstImageLayout;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                Direct.CmdCopyImage(_commandBuffer, _srcImage, _srcImageLayout, _dstImage, _dstImageLayout, _regionCount, _pRegions);
            }
        }

        public void CmdCopyBufferToImage(IVkBuffer srcBuffer, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkBufferImageCopy> regions)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcBuffer = srcBuffer?.Handle ?? VkBuffer.HandleType.Null;
                var _dstImage = dstImage?.Handle ?? VkImage.HandleType.Null;
                var _dstImageLayout = dstImageLayout;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                Direct.CmdCopyBufferToImage(_commandBuffer, _srcBuffer, _dstImage, _dstImageLayout, _regionCount, _pRegions);
            }
        }

        public void CmdCopyImageToBuffer(IVkImage srcImage, VkImageLayout srcImageLayout, IVkBuffer dstBuffer, IReadOnlyList<VkBufferImageCopy> regions)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcImage = srcImage?.Handle ?? VkImage.HandleType.Null;
                var _srcImageLayout = srcImageLayout;
                var _dstBuffer = dstBuffer?.Handle ?? VkBuffer.HandleType.Null;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                Direct.CmdCopyImageToBuffer(_commandBuffer, _srcImage, _srcImageLayout, _dstBuffer, _regionCount, _pRegions);
            }
        }

        public void CmdBlitImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageBlit> regions, VkFilter filter)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcImage = srcImage?.Handle ?? VkImage.HandleType.Null;
                var _srcImageLayout = srcImageLayout;
                var _dstImage = dstImage?.Handle ?? VkImage.HandleType.Null;
                var _dstImageLayout = dstImageLayout;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                var _filter = filter;
                Direct.CmdBlitImage(_commandBuffer, _srcImage, _srcImageLayout, _dstImage, _dstImageLayout, _regionCount, _pRegions, _filter);
            }
        }

        public void CmdResolveImage(IVkImage srcImage, VkImageLayout srcImageLayout, IVkImage dstImage, VkImageLayout dstImageLayout, IReadOnlyList<VkImageResolve> regions)
        {
            var unmanagedSize =
                regions.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _srcImage = srcImage?.Handle ?? VkImage.HandleType.Null;
                var _srcImageLayout = srcImageLayout;
                var _dstImage = dstImage?.Handle ?? VkImage.HandleType.Null;
                var _dstImageLayout = dstImageLayout;
                var _regionCount = regions?.Count ?? 0;
                var _pRegions = regions.MarshalDirect(ref unmanaged);
                Direct.CmdResolveImage(_commandBuffer, _srcImage, _srcImageLayout, _dstImage, _dstImageLayout, _regionCount, _pRegions);
            }
        }

        public void CmdBindIndexBuffer(IVkBuffer buffer, ulong offset, VkIndexType indexType)
        {
            var _commandBuffer = Handle;
            var _buffer = buffer?.Handle ?? VkBuffer.HandleType.Null;
            var _offset = offset;
            var _indexType = indexType;
            Direct.CmdBindIndexBuffer(_commandBuffer, _buffer, _offset, _indexType);
        }

        public void CmdDraw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            var _commandBuffer = Handle;
            var _vertexCount = vertexCount;
            var _instanceCount = instanceCount;
            var _firstVertex = firstVertex;
            var _firstInstance = firstInstance;
            Direct.CmdDraw(_commandBuffer, _vertexCount, _instanceCount, _firstVertex, _firstInstance);
        }

        public void CmdDrawIndexed(int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance)
        {
            var _commandBuffer = Handle;
            var _indexCount = indexCount;
            var _instanceCount = instanceCount;
            var _firstIndex = firstIndex;
            var _vertexOffset = vertexOffset;
            var _firstInstance = firstInstance;
            Direct.CmdDrawIndexed(_commandBuffer, _indexCount, _instanceCount, _firstIndex, _vertexOffset, _firstInstance);
        }

        public void CmdDrawIndirect(IVkBuffer buffer, ulong offset, int drawCount, int stride)
        {
            var _commandBuffer = Handle;
            var _buffer = buffer?.Handle ?? VkBuffer.HandleType.Null;
            var _offset = offset;
            var _drawCount = drawCount;
            var _stride = stride;
            Direct.CmdDrawIndirect(_commandBuffer, _buffer, _offset, _drawCount, _stride);
        }

        public void CmdDrawIndexedIndirect(IVkBuffer buffer, ulong offset, int drawCount, int stride)
        {
            var _commandBuffer = Handle;
            var _buffer = buffer?.Handle ?? VkBuffer.HandleType.Null;
            var _offset = offset;
            var _drawCount = drawCount;
            var _stride = stride;
            Direct.CmdDrawIndexedIndirect(_commandBuffer, _buffer, _offset, _drawCount, _stride);
        }

        public void CmdBindVertexBuffers(int firstBinding, IReadOnlyList<IVkBuffer> buffers, IReadOnlyList<ulong> offsets)
        {
            var unmanagedSize =
                buffers.SizeOfMarshalDirect() +
                offsets.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _firstBinding = firstBinding;
                var _bindingCount = Math.Min(buffers?.Count ?? 0, offsets?.Count ?? 0);
                var _pBuffers = buffers.MarshalDirect(ref unmanaged);
                var _pOffsets = offsets.MarshalDirect(ref unmanaged);
                Direct.CmdBindVertexBuffers(_commandBuffer, _firstBinding, _bindingCount, _pBuffers, _pOffsets);
            }
        }

        public void CmdSetViewport(int firstViewport, IReadOnlyList<VkViewport> viewports)
        {
            var unmanagedSize =
                viewports.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _firstViewport = firstViewport;
                var _viewportCount = viewports?.Count ?? 0;
                var _pViewports = viewports.MarshalDirect(ref unmanaged);
                Direct.CmdSetViewport(_commandBuffer, _firstViewport, _viewportCount, _pViewports);
            }
        }

        public void CmdSetLineWidth(float lineWidth)
        {
            var _commandBuffer = Handle;
            var _lineWidth = lineWidth;
            Direct.CmdSetLineWidth(_commandBuffer, _lineWidth);
        }

        public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
        {
            var _commandBuffer = Handle;
            var _depthBiasConstantFactor = depthBiasConstantFactor;
            var _depthBiasClamp = depthBiasClamp;
            var _depthBiasSlopeFactor = depthBiasSlopeFactor;
            Direct.CmdSetDepthBias(_commandBuffer, _depthBiasConstantFactor, _depthBiasClamp, _depthBiasSlopeFactor);
        }

        public void CmdSetScissor(int firstScissor, IReadOnlyList<VkRect2D> scissors)
        {
            var unmanagedSize =
                scissors.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _firstScissor = firstScissor;
                var _scissorCount = scissors?.Count ?? 0;
                var _pScissors = scissors.MarshalDirect(ref unmanaged);
                Direct.CmdSetScissor(_commandBuffer, _firstScissor, _scissorCount, _pScissors);
            }
        }

        public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
        {
            var _commandBuffer = Handle;
            var _minDepthBounds = minDepthBounds;
            var _maxDepthBounds = maxDepthBounds;
            Direct.CmdSetDepthBounds(_commandBuffer, _minDepthBounds, _maxDepthBounds);
        }

        public void CmdSetStencilCompareMask(VkStencilFaceFlags faceMask, int compareMask)
        {
            var _commandBuffer = Handle;
            var _faceMask = faceMask;
            var _compareMask = compareMask;
            Direct.CmdSetStencilCompareMask(_commandBuffer, _faceMask, _compareMask);
        }

        public void CmdSetStencilWriteMask(VkStencilFaceFlags faceMask, int writeMask)
        {
            var _commandBuffer = Handle;
            var _faceMask = faceMask;
            var _writeMask = writeMask;
            Direct.CmdSetStencilWriteMask(_commandBuffer, _faceMask, _writeMask);
        }

        public void CmdSetStencilReference(VkStencilFaceFlags faceMask, int reference)
        {
            var _commandBuffer = Handle;
            var _faceMask = faceMask;
            var _reference = reference;
            Direct.CmdSetStencilReference(_commandBuffer, _faceMask, _reference);
        }

        public void CmdSetBlendConstants(VkColor4 blendConstants)
        {
            var _commandBuffer = Handle;
            var _blendConstants = &blendConstants;
            Direct.CmdSetBlendConstants(_commandBuffer, _blendConstants);
        }

        public void CmdDispatch(int x, int y, int z)
        {
            var _commandBuffer = Handle;
            var _x = x;
            var _y = y;
            var _z = z;
            Direct.CmdDispatch(_commandBuffer, _x, _y, _z);
        }

        public void CmdDispatchIndirect(IVkBuffer buffer, ulong offset)
        {
            var _commandBuffer = Handle;
            var _buffer = buffer?.Handle ?? VkBuffer.HandleType.Null;
            var _offset = offset;
            Direct.CmdDispatchIndirect(_commandBuffer, _buffer, _offset);
        }

        public void CmdDebugMarkerBeginEXT(VkDebugMarkerMarkerInfoEXT markerInfo)
        {
            var unmanagedSize =
                markerInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _pMarkerInfo = markerInfo.MarshalIndirect(ref unmanaged);
                Direct.CmdDebugMarkerBeginEXT(_commandBuffer, _pMarkerInfo);
            }
        }

        public void CmdDebugMarkerEndEXT()
        {
            var _commandBuffer = Handle;
            Direct.CmdDebugMarkerEndEXT(_commandBuffer);
        }

        public void CmdDebugMarkerInsertEXT(VkDebugMarkerMarkerInfoEXT markerInfo)
        {
            var unmanagedSize =
                markerInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _commandBuffer = Handle;
                var _pMarkerInfo = markerInfo.MarshalIndirect(ref unmanaged);
                Direct.CmdDebugMarkerInsertEXT(_commandBuffer, _pMarkerInfo);
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
