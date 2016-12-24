using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    [Handled]
    public unsafe struct GenCommandBuffer : IGenHandledObject, IGenDeviceChild
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
    }
}