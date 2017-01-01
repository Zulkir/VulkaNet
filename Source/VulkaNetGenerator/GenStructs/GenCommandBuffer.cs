using System;
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenCommandBuffer : IGenHandledObject
    {
        [MethodName("Reset")]
        public VkResult ResetCommandBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkCommandBufferResetFlags flags)
            => default(VkResult);

        [MethodName("Begin")]
        public VkResult BeginCommandBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenCommandBufferBeginInfo* pBeginInfo)
            => default(VkResult);

        [MethodName("End")]
        public VkResult EndCommandBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer)
            => default(VkResult);
        
        public void CmdExecuteCommands(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            [CountFor("commandBuffers")] int commandBufferCount,
            [IsArray] GenCommandBuffer* pCommandBuffers)
        { }

        public void CmdSetEvent(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenEvent eventObj,
            VkPipelineStageFlags stageMask)
        { }

        public void CmdResetEvent(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenEvent eventObj,
            VkPipelineStageFlags stageMask)
        { }

        public void CmdWaitEvents(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            [CountFor("events")] int eventCount,
            [IsArray] GenEvent* pEvents,
            VkPipelineStageFlags srcStageMask,
            VkPipelineStageFlags dstStageMask,
            [CountFor("memoryBarriers")] int memoryBarrierCount,
            [IsArray] GenMemoryBarrier* pMemoryBarriers,
            [CountFor("bufferMemoryBarriers")] int bufferMemoryBarrierCount,
            [IsArray] GenBufferMemoryBarrier* pBufferMemoryBarriers,
            [CountFor("imageMemoryBarriers")] int imageMemoryBarrierCount,
            [IsArray] GenImageMemoryBarrier* pImageMemoryBarriers)
        { }

        public void CmdPipelineBarrier(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkPipelineStageFlags srcStageMask,
            VkPipelineStageFlags dstStageMask,
            VkDependencyFlags dependencyFlags,
            [CountFor("memoryBarriers")] int memoryBarrierCount,
            [IsArray] GenMemoryBarrier* pMemoryBarriers,
            [CountFor("bufferMemoryBarriers")] int bufferMemoryBarrierCount,
            [IsArray] GenBufferMemoryBarrier* pBufferMemoryBarriers,
            [CountFor("imageMemoryBarriers")] int imageMemoryBarrierCount,
            [IsArray] GenImageMemoryBarrier* pImageMemoryBarriers)
        { }

        public void CmdBeginRenderPass(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenRenderPassBeginInfo* pRenderPassBegin,
            VkSubpassContents contents)
        { }

        public void CmdNextSubpass(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkSubpassContents contents)
        { }

        public void CmdBindPipeline(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            GenPipeline pipeline)
        { }

        public void CmdBindDescriptorSets(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkPipelineBindPoint pipelineBindPoint,
            GenPipelineLayout layout,
            int firstSet,
            [CountFor("descriptorSets")] int descriptorSetCount,
            [IsArray] GenDescriptorSet* pDescriptorSets,
            [CountFor("dynamicOffsets")] int dynamicOffsetCount,
            [IsArray] int* pDynamicOffsets)
        { }

        public void CmdPushConstants(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenPipelineLayout layout,
            VkShaderStageFlagBits stageFlags,
            int offset,
            int size,
            IntPtr pValues)
        { }

        public void CmdResetQueryPool(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenQueryPool queryPool,
            int firstQuery,
            int queryCount)
        { }

        public void CmdBeginQuery(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenQueryPool queryPool,
            int query,
            VkQueryControlFlags flags)
        { }

        public void CmdEndQuery(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenQueryPool queryPool,
            int query)
        { }

        public void CmdCopyQueryPoolResults(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenQueryPool queryPool,
            int firstQuery,
            int queryCount,
            GenBuffer dstBuffer,
            ulong dstOffset,
            ulong stride,
            VkQueryResultFlags flags)
        { }

        public void CmdWriteTimestamp(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            VkPipelineStageFlags pipelineStage,
            GenQueryPool queryPool,
            int query)
        { }

        public void CmdClearColorImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage image,
            VkImageLayout imageLayout,
            VkClearColorValue* pColor,
            [CountFor("ranges")] int rangeCount,
            [IsArray] VkImageSubresourceRange* pRanges)
        { }

        public void CmdClearDepthStencilImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage image,
            VkImageLayout imageLayout,
            VkClearDepthStencilValue* pDepthStencil,
            [CountFor("ranges")] int rangeCount,
            [IsArray] VkImageSubresourceRange* pRanges)
        { }

        public void CmdClearAttachments(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            int attachmentCount,
            VkClearAttachment* pAttachments,
            int rectCount,
            VkClearRect* pRects)
        { }

        public void CmdFillBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenBuffer dstBuffer,
            DeviceSize dstOffset,
            DeviceSize size,
            int data)
        { }

        public void CmdUpdateBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenBuffer dstBuffer,
            DeviceSize dstOffset,
            DeviceSize dataSize,
            IntPtr pData)
        { }

        public void CmdCopyBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenBuffer srcBuffer,
            GenBuffer dstBuffer,
            [CountFor("regions")] int regionCount,
            [IsArray] VkBufferCopy* pRegions)
        { }

        public void CmdCopyImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage srcImage,
            VkImageLayout srcImageLayout,
            GenImage dstImage,
            VkImageLayout dstImageLayout,
            [CountFor("regions")] int regionCount,
            [IsArray] VkImageCopy* pRegions)
        { }

        public void CmdCopyBufferToImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenBuffer srcBuffer,
            GenImage dstImage,
            VkImageLayout dstImageLayout,
            [CountFor("regions")] int regionCount,
            [IsArray] VkBufferImageCopy* pRegions)
        { }

        public void CmdCopyImageToBuffer(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage srcImage,
            VkImageLayout srcImageLayout,
            GenBuffer dstBuffer,
            [CountFor("regions")] int regionCount,
            [IsArray] VkBufferImageCopy* pRegions)
        { }

        public void CmdBlitImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage srcImage,
            VkImageLayout srcImageLayout,
            GenImage dstImage,
            VkImageLayout dstImageLayout,
            [CountFor("regions")] int regionCount,
            [IsArray] VkImageBlit* pRegions,
            VkFilter filter)
        { }

        public void CmdResolveImage(
            [FromProperty("this")] GenCommandBuffer commandBuffer,
            GenImage srcImage,
            VkImageLayout srcImageLayout,
            GenImage dstImage,
            VkImageLayout dstImageLayout,
            [CountFor("regions")] int regionCount,
            [IsArray] VkImageResolve* pRegions)
        { }
    }
}