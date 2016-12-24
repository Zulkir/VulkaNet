using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    [Handled]
    public unsafe struct GenSemaphore : IGenNonDispatchableHandledObject, IGenDeviceChild
    {
        [MethodName("Dispose")]
        public void DestroySemaphore(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenSemaphore semaphore,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
            { }
    }
}