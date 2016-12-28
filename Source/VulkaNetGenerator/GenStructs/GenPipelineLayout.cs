using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineLayout : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyPipelineLayout(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenPipelineLayout pipelineLayout,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}