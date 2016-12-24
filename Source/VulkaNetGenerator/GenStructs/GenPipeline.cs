using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipeline : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyPipeline(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenPipeline pipeline,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}