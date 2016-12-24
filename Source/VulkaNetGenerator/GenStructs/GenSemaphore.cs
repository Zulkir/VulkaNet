using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSemaphore : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroySemaphore(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSemaphore semaphore,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
            { }
    }
}