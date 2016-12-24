using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenCommandPool : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyCommandPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("Reset")]
        public VkResult ResetCommandPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            VkCommandPoolResetFlags flags)
            => default(VkResult);
        
        public void FreeCommandBuffers(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            [CountFor("commandBuffers")] int commandBufferCount,
            [IsArray] GenCommandBuffer* pCommandBuffers)
        { }
    }
}