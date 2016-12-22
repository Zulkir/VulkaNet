using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    public unsafe interface GenFence : IGenDeviceChild, IGenNonDispatchableHandledObject, IGenAllocatable
    {
        [Dispose]
        [MethodName("Dispose")]
        void DestroyFence(
            GenDevice device,
            [Self] GenFence fence,
            GenAllocationCallbacks* pAllocator);

        [MethodName("GetStatus")]
        VkResult GetFenceStatus(
            GenDevice device, 
            [Self] GenFence fence);
    }
}