using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenFence : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyFence(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenFence fence,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("GetStatus")]
        public VkResult GetFenceStatus(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenFence fence)
            => default(VkResult);
    }
}