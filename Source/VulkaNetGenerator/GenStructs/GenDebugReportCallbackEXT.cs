using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDebugReportCallbackEXT : IGenNonDispatchableHandledObject, IGenInstanceChild
    {
        [MethodName("Dispose")]
        public void DestroyDebugReportCallbackEXT(
            [FromProperty("Instance.Handle")] VkInstance instance,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator,
            [FromProperty("this")] GenDebugReportCallbackEXT callback)
        { }
    }
}