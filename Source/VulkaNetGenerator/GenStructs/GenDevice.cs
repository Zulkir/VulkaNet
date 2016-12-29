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

        public VkResult AllocateMemory(
            [FromProperty("this")] GenDevice device,
            GenMemoryAllocateInfo* pAllocateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenDeviceMemory* pMemory)
            => default (VkResult);

        public VkResult FlushMappedMemoryRanges(
            [FromProperty("this")] GenDevice device,
            [CountFor("memoryRanges")] int memoryRangeCount,
            [IsArray] GenMappedMemoryRange* pMemoryRanges)
            => default(VkResult);

        public VkResult InvalidateMappedMemoryRanges(
            [FromProperty("this")] GenDevice device,
            [CountFor("memoryRanges")] int memoryRangeCount,
            [IsArray] GenMappedMemoryRange* pMemoryRanges)
            => default(VkResult);

        public VkResult CreateBuffer(
            [FromProperty("this")] GenDevice device,
            GenBufferCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenBuffer* pBuffer)
            => default(VkResult);

        public VkResult CreateBufferView(
            [FromProperty("this")] GenDevice device,
            GenBufferViewCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenBufferView* pView)
            => default(VkResult);

        public VkResult CreateImage(
            [FromProperty("this")] GenDevice device,
            GenImageCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenImage* pImage)
            => default(VkResult);

        public VkResult CreateImageView(
            [FromProperty("this")] GenDevice device,
            GenImageViewCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenImageView* pView)
            => default(VkResult);

        public VkResult CreateSampler(
            [FromProperty("this")] GenDevice device,
            GenSamplerCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenSampler* pSampler)
            => default(VkResult);

        public VkResult CreateDescriptorSetLayout(
            [FromProperty("this")] GenDevice device,
            GenDescriptorSetLayoutCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenDescriptorSetLayout* pSetLayout)
            => default(VkResult);

        public VkResult CreatePipelineLayout(
            [FromProperty("this")] GenDevice device,
            GenPipelineLayoutCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenPipelineLayout* pPipelineLayout)
            => default(VkResult);

        public VkResult CreateDescriptorPool(
            [FromProperty("this")] GenDevice device,
            GenDescriptorPoolCreateInfo* pCreateInfo,
            GenAllocationCallbacks* pAllocator,
            [Return] GenDescriptorPool* pDescriptorPool)
            => default(VkResult);

        // todo: to GenDescriptorPool
        public VkResult AllocateDescriptorSets(
            [FromProperty("this")] GenDevice device,
            GenDescriptorSetAllocateInfo* pAllocateInfo,
            [Return, ReturnCount("allocateInfo.SetLayouts.Count"), IsArray] GenDescriptorSet* pDescriptorSets)
            => default(VkResult);

        // todo: to GenDescriptorPool
        public VkResult FreeDescriptorSets(
            [FromProperty("this")] GenDevice device,
            GenDescriptorPool descriptorPool,
            [CountFor("descriptorSets")] int descriptorSetCount,
            [IsArray] GenDescriptorSet* pDescriptorSets)
            => default(VkResult);

        public void UpdateDescriptorSets(
            [FromProperty("this")] GenDevice device,
            [CountFor("descriptorWrites")] int descriptorWriteCount,
            [IsArray] GenWriteDescriptorSet* pDescriptorWrites,
            [CountFor("descriptorCopies")] int descriptorCopyCount,
            [IsArray] GenCopyDescriptorSet* pDescriptorCopies)
            { }
    }
}