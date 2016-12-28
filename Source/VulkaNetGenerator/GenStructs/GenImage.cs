using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenImage : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyImage(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenImage image,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        public void GetImageSubresourceLayout(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenImage image,
            VkImageSubresource* pSubresource,
            [Return] VkSubresourceLayout* pLayout)
        { }

        [MethodName("GetMemoryRequirements")]
        public void GetImageMemoryRequirements(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenImage image,
            [Return] VkMemoryRequirements* pMemoryRequirements)
        { }

        [MethodName("BindMemory")]
        public VkResult BindImageMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenImage image,
            GenDeviceMemory memory,
            ulong memoryOffset)
            => default(VkResult);
    }
}