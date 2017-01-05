using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSurfaceKHR : IGenNonDispatchableHandledObject, IGenInstanceChild
    {
        [MethodName("Dispose")]
        public void DestroySurfaceKHR(
            [FromProperty("Instance.Handle")] VkInstance instance,
            [FromProperty("this")] GenSurfaceKHR surface,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}