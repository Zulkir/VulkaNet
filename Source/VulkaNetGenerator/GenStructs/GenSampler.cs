using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSampler : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroySampler(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSampler sampler,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}