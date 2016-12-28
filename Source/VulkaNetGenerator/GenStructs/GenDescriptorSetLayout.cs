using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorSetLayout : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyDescriptorSetLayout(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDescriptorSetLayout descriptorSetLayout,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}