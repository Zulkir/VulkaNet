using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBuffer : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyBuffer(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenBuffer buffer,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}