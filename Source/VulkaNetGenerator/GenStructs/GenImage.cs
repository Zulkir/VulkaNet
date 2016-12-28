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
    }
}