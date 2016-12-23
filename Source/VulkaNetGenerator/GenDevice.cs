﻿using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    [Handled]
    public unsafe struct GenDevice
    {
        [MethodName("Dispose")]
        public void DestroyDevice(
            [FromProperty("this")] GenDevice device,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        public VkResult DeviceWaitIdle(
            [FromProperty("this")] GenDevice device)
            => default(VkResult);

        public VkResult CreateCommandPool(
            [FromProperty("this")] GenDevice device,
            GenCommandPoolCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenCommandPool* pCommandPool)
            => default(VkResult);

        public VkResult CreateFence(
            [FromProperty("this")] GenDevice device,
            GenFenceCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenFence* pFence)
            => default(VkResult);

        public VkResult ResetFences(
            [FromProperty("this")] GenDevice device,
            [CountFor("fences")] int fenceCount,
            [IsArray] GenFence* pFences)
            => default(VkResult);

        public VkResult WaitForFences(
            [FromProperty("this")] GenDevice device,
            [CountFor("fences")] int fenceCount,
            [IsArray] GenFence* pFences,
            VkBool32 waitAll,
            ulong timeout)
            => default(VkResult);
    }
}