using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSwapchainKHR : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroySwapchainKHR(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSwapchainKHR swapchain,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("GetImagesKHR")]
        public VkResult GetSwapchainImagesKHR(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSwapchainKHR swapchain,
            [ReturnSize] int* pSwapchainImageCount,
            [Return, IsArray] GenImage* pSwapchainImages)
            => default(VkResult);

        public VkResult AcquireNextImageKHR(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSwapchainKHR swapchain,
            ulong timeout,
            GenSemaphore semaphore,
            GenFence fence,
            [Return] int* pImageIndex)
            => default(VkResult);
    }
}