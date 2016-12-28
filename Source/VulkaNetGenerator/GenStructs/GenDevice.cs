using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDevice : IGenHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyDevice(
            [FromProperty("this")] GenDevice device,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("WaitIdle")]
        public VkResult DeviceWaitIdle(
            [FromProperty("this")] GenDevice device)
            => default(VkResult);

        public VkResult CreateCommandPool(
            [FromProperty("this")] GenDevice device,
            GenCommandPoolCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenCommandPool* pCommandPool)
            => default(VkResult);

        public VkResult AllocateCommandBuffers(
            [FromProperty("this")] GenDevice device,
            GenCommandBufferAllocateInfo* pAllocateInfo,
            [Return, IsArray, ReturnCount("allocateInfo.CommandBufferCount")] GenCommandBuffer* pCommandBuffers)
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

        public VkResult CreateSemaphore(
            [FromProperty("this")] GenDevice device,
            GenSemaphoreCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenSemaphore* pSemaphore)
            => default(VkResult);

        public VkResult CreateEvent(
            [FromProperty("this")] GenDevice device,
            GenEventCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenEvent* pEvent)
            => default(VkResult);

        public VkResult CreateRenderPass(
            [FromProperty("this")] GenDevice device,
            GenRenderPassCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenRenderPass* pRenderPass)
            => default(VkResult);

        public VkResult CreateFramebuffer(
            [FromProperty("this")] GenDevice device,
            GenFramebufferCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenFramebuffer* pFramebuffer)
            => default(VkResult);

        public VkResult CreateShaderModule(
            [FromProperty("this")] GenDevice device,
            GenShaderModuleCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenShaderModule* pShaderModule)
            => default(VkResult);

        public VkResult CreateComputePipelines(
            [FromProperty("this")] GenDevice device,
            GenPipelineCache pipelineCache,
            [CountFor("createInfos")] int createInfoCount,
            [IsArray] GenComputePipelineCreateInfo* pCreateInfos,
            GenAllocationCallbacks* pAllocator,
            [Return] GenPipeline* pPipelines)
            => default(VkResult);

        public VkResult CreateGraphicsPipelines(
            [FromProperty("this")] GenDevice device,
            GenPipelineCache pipelineCache,
            [CountFor("createInfos")] int createInfoCount,
            [IsArray] GenGraphicsPipelineCreateInfo* pCreateInfos,
            GenAllocationCallbacks* pAllocator,
            [Return] GenPipeline* pPipelines)
            => default(VkResult);

        public VkResult CreatePipelineCache(
            [FromProperty("this")] GenDevice device,
            GenPipelineCacheCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenPipelineCache* pPipelineCache)
            => default(VkResult);
    }
}