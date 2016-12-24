using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenEvent : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyEvent(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenEvent eventObj,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("GetStatus")]
        public VkResult GetEventStatus(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenEvent eventObj)
            => default(VkResult);

        [MethodName("Set")]
        public VkResult SetEvent(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenEvent eventObj)
            => default(VkResult);

        [MethodName("Reset")]
        public VkResult ResetEvent(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenEvent eventObj)
            => default(VkResult);
    }
}