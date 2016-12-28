using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenImageView : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyImageView(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenImageView imageView,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}