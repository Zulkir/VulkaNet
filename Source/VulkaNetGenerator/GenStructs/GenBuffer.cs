using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBuffer : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyBuffer(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenBuffer buffer,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("GetMemoryRequirements")]
        public void GetBufferMemoryRequirements(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenBuffer buffer,
            [Return] VkMemoryRequirements* pMemoryRequirements)
        { }

        [MethodName("BindMemory")]
        public VkResult BindBufferMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenBuffer buffer,
            GenDeviceMemory memory,
            ulong memoryOffset)
            => default(VkResult);
    }
}