using System;
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDeviceMemory : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void FreeMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDeviceMemory memory,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("Map")]
        public VkResult MapMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDeviceMemory memory,
            ulong offset,
            ulong size,
            VkMemoryMapFlags flags,
            [Return] IntPtr* ppData)
            => default(VkResult);

        [MethodName("Unmap")]
        public void UnmapMemory(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDeviceMemory memory)
        { }

        [MethodName("GetCommitment")]
        public void GetDeviceMemoryCommitment(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenDeviceMemory memory,
            [Return] ulong* pCommittedMemoryInBytes)
        { }
    }
}