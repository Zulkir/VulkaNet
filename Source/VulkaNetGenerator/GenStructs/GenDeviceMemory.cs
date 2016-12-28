using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDeviceMemory : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void FreeMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDeviceMemory memory,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}