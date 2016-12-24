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
    }
}