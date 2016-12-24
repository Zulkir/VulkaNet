using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenShaderModule : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyShaderModule(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenShaderModule shaderModule,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }
    }
}