using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenEvent : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyEvent(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenEvent eventObj,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}