using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenQueryPool : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyQueryPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenQueryPool queryPool,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}