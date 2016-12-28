using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorPool : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyDescriptorPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDescriptorPool descriptorPool,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("Reset")]
        public VkResult ResetDescriptorPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDescriptorPool descriptorPool,
            VkDescriptorPoolResetFlags flags)
            => default(VkResult);
    }
}