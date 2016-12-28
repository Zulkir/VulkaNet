using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBufferView : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyBufferView(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenBufferView bufferView,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}