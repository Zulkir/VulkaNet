using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenFramebuffer : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyFramebuffer(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenFramebuffer framebuffer,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}