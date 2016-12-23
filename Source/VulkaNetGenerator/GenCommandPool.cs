﻿using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    [Handled]
    public unsafe struct GenCommandPool : IGenNonDispatchableHandledObject, IGenDeviceChild, IGenAllocatable
    {
        [MethodName("Dispose")]
        public void DestroyCommandPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("Reset")]
        public VkResult ResetCommandPool(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            VkCommandPoolResetFlags flags)
            => default(VkResult);
        
        public void FreeCommandBuffers(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenCommandPool commandPool,
            [CountFor("commandBuffers")] int commandBufferCount,
            [IsArray] GenCommandBuffer* pCommandBuffers)
        { }
    }
}