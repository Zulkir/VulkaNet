using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenRenderPass : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyRenderPass(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenRenderPass renderPass,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("GetGranularity")]
        public void GetRenderAreaGranularity(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenRenderPass renderPass,
            [Return] VkExtent2D* pGranularity)
        { }
    }
}