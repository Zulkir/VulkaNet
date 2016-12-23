using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    [Handled]
    public unsafe struct GenFence : IGenDeviceChild, IGenNonDispatchableHandledObject, IGenAllocatable
    {
        [Dispose]
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