#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkDevice : IVkHandledObject, IDisposable, IVkInstanceChild
    {
        IVkPhysicalDevice PhysicalDevice { get; }
        VkDevice.HandleType Handle { get; }
        VkDevice.DirectFunctions Direct { get; }
        IVkAllocationCallbacks Allocator { get; }
        IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex);
        VkResult WaitIdle();
        VkObjectResult<IVkCommandPool> CreateCommandPool(VkCommandPoolCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkCommandBuffer>> AllocateCommandBuffers(VkCommandBufferAllocateInfo allocateInfo);
        VkObjectResult<IVkFence> CreateFence(VkFenceCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkResult ResetFences(IReadOnlyList<IVkFence> fences);
        VkResult WaitForFences(IReadOnlyList<IVkFence> fences, bool waitAll, ulong timeout);
        VkObjectResult<IVkSemaphore> CreateSemaphore(VkSemaphoreCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkEvent> CreateEvent(VkEventCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkRenderPass> CreateRenderPass(VkRenderPassCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkFramebuffer> CreateFramebuffer(VkFramebufferCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkShaderModule> CreateShaderModule(VkShaderModuleCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkPipeline>> CreateComputePipelines(IVkPipelineCache pipelineCache, IReadOnlyList<VkComputePipelineCreateInfo> createInfos, VkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkPipeline>> CreateGraphicsPipelines(IVkPipelineCache pipelineCache, IReadOnlyList<VkGraphicsPipelineCreateInfo> createInfos, VkAllocationCallbacks allocator);
        VkObjectResult<IVkPipelineCache> CreatePipelineCache(VkPipelineCacheCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkDeviceMemory> AllocateMemory(VkMemoryAllocateInfo allocateInfo, VkAllocationCallbacks allocator);
        VkResult FlushMappedMemoryRanges(IReadOnlyList<VkMappedMemoryRange> memoryRanges);
        VkResult InvalidateMappedMemoryRanges(IReadOnlyList<VkMappedMemoryRange> memoryRanges);
        VkObjectResult<IVkBuffer> CreateBuffer(VkBufferCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkBufferView> CreateBufferView(VkBufferViewCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkImage> CreateImage(VkImageCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkImageView> CreateImageView(VkImageViewCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkSampler> CreateSampler(VkSamplerCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkDescriptorSetLayout> CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkPipelineLayout> CreatePipelineLayout(VkPipelineLayoutCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IVkDescriptorPool> CreateDescriptorPool(VkDescriptorPoolCreateInfo createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkDescriptorSet>> AllocateDescriptorSets(VkDescriptorSetAllocateInfo allocateInfo);
        VkResult FreeDescriptorSets(IVkDescriptorPool descriptorPool, IReadOnlyList<IVkDescriptorSet> descriptorSets);
        void UpdateDescriptorSets(IReadOnlyList<VkWriteDescriptorSet> descriptorWrites, IReadOnlyList<VkCopyDescriptorSet> descriptorCopies);
        VkObjectResult<IVkQueryPool> CreateQueryPool(VkQueryPoolCreateInfo createInfo, VkAllocationCallbacks allocator);
        IReadOnlyList<VkSparseImageMemoryRequirements> GetImageSparseMemoryRequirements(IVkImage image);
        VkObjectResult<IVkSwapchainKHR> CreateSwapchainKHR(VkSwapchainCreateInfoKHR createInfo, VkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkSwapchainKHR>> CreateSharedSwapchainsKHR(IReadOnlyList<VkSwapchainCreateInfoKHR> createInfos, VkAllocationCallbacks allocator);
        VkResult DebugMarkerSetObjectNameEXT(VkDebugMarkerObjectNameInfoEXT nameInfo);
        VkResult DebugMarkerSetObjectTagEXT(VkDebugMarkerObjectTagInfoEXT tagInfo);
    }

    public unsafe class VkDevice : IVkDevice
    {
        public IVkInstance Instance { get; }
        public IVkPhysicalDevice PhysicalDevice { get; }
        public HandleType Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }
        public DirectFunctions Direct { get; }

        public IntPtr RawHandle => Handle.InternalHandle;

        private readonly ConcurrentDictionary<ValuePair<int, int>, IVkQueue> queues;

        public VkDevice(IVkPhysicalDevice physicalDevice, HandleType handle, IVkAllocationCallbacks allocator)
        {
            PhysicalDevice = physicalDevice;
            Instance = physicalDevice.Instance;
            Handle = handle;
            Allocator = allocator;
            Direct = new DirectFunctions(this);
            queues = new ConcurrentDictionary<ValuePair<int, int>, IVkQueue>();
        }

        public struct HandleType
        {
            public readonly IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = IntPtr.Size;
            public static HandleType Null => new HandleType(default(IntPtr));
        }

        public class DirectFunctions
        {
            private readonly IVkDevice device;

            public GetDeviceProcAddrDelegate GetDeviceProcAddr { get; }
            public delegate IntPtr GetDeviceProcAddrDelegate(
                HandleType device,
                byte* pName);

            public GetDeviceQueueDelegate GetDeviceQueue { get; }
            public delegate void GetDeviceQueueDelegate(
                HandleType device,
                uint queueFamilyIndex,
                uint queueIndex,
                VkQueue.HandleType* pQueue);

            public QueueSubmitDelegate QueueSubmit { get; }
            public delegate VkResult QueueSubmitDelegate(
                VkQueue.HandleType queue,
                int submitCount,
                VkSubmitInfo.Raw* pSubmits,
                VkFence.HandleType fence);

            public QueueWaitIdleDelegate QueueWaitIdle { get; }
            public delegate VkResult QueueWaitIdleDelegate(
                VkQueue.HandleType queue);

            public QueueBindSparseDelegate QueueBindSparse { get; }
            public delegate VkResult QueueBindSparseDelegate(
                VkQueue.HandleType queue,
                int bindInfoCount,
                VkBindSparseInfo.Raw* pBindInfo,
                VkFence.HandleType fence);

            public QueuePresentKHRDelegate QueuePresentKHR { get; }
            public delegate VkResult QueuePresentKHRDelegate(
                VkQueue.HandleType queue,
                VkPresentInfoKHR.Raw* pPresentInfo);

            public DestroyCommandPoolDelegate DestroyCommandPool { get; }
            public delegate void DestroyCommandPoolDelegate(
                HandleType device,
                VkCommandPool.HandleType commandPool,
                VkAllocationCallbacks.Raw* pAllocator);

            public ResetCommandPoolDelegate ResetCommandPool { get; }
            public delegate VkResult ResetCommandPoolDelegate(
                HandleType device,
                VkCommandPool.HandleType commandPool,
                VkCommandPoolResetFlags flags);

            public FreeCommandBuffersDelegate FreeCommandBuffers { get; }
            public delegate void FreeCommandBuffersDelegate(
                HandleType device,
                VkCommandPool.HandleType commandPool,
                int commandBufferCount,
                VkCommandBuffer.HandleType* pCommandBuffers);

            public ResetCommandBufferDelegate ResetCommandBuffer { get; }
            public delegate VkResult ResetCommandBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkCommandBufferResetFlags flags);

            public BeginCommandBufferDelegate BeginCommandBuffer { get; }
            public delegate VkResult BeginCommandBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkCommandBufferBeginInfo.Raw* pBeginInfo);

            public EndCommandBufferDelegate EndCommandBuffer { get; }
            public delegate VkResult EndCommandBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer);

            public CmdExecuteCommandsDelegate CmdExecuteCommands { get; }
            public delegate void CmdExecuteCommandsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int commandBufferCount,
                VkCommandBuffer.HandleType* pCommandBuffers);

            public CmdSetEventDelegate CmdSetEvent { get; }
            public delegate void CmdSetEventDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkEvent.HandleType eventObj,
                VkPipelineStageFlags stageMask);

            public CmdResetEventDelegate CmdResetEvent { get; }
            public delegate void CmdResetEventDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkEvent.HandleType eventObj,
                VkPipelineStageFlags stageMask);

            public CmdWaitEventsDelegate CmdWaitEvents { get; }
            public delegate void CmdWaitEventsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int eventCount,
                VkEvent.HandleType* pEvents,
                VkPipelineStageFlags srcStageMask,
                VkPipelineStageFlags dstStageMask,
                int memoryBarrierCount,
                VkMemoryBarrier.Raw* pMemoryBarriers,
                int bufferMemoryBarrierCount,
                VkBufferMemoryBarrier.Raw* pBufferMemoryBarriers,
                int imageMemoryBarrierCount,
                VkImageMemoryBarrier.Raw* pImageMemoryBarriers);

            public CmdPipelineBarrierDelegate CmdPipelineBarrier { get; }
            public delegate void CmdPipelineBarrierDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineStageFlags srcStageMask,
                VkPipelineStageFlags dstStageMask,
                VkDependencyFlags dependencyFlags,
                int memoryBarrierCount,
                VkMemoryBarrier.Raw* pMemoryBarriers,
                int bufferMemoryBarrierCount,
                VkBufferMemoryBarrier.Raw* pBufferMemoryBarriers,
                int imageMemoryBarrierCount,
                VkImageMemoryBarrier.Raw* pImageMemoryBarriers);

            public CmdBeginRenderPassDelegate CmdBeginRenderPass { get; }
            public delegate void CmdBeginRenderPassDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkRenderPassBeginInfo.Raw* pRenderPassBegin,
                VkSubpassContents contents);

            public CmdEndRenderPassDelegate CmdEndRenderPass { get; }
            public delegate void CmdEndRenderPassDelegate(
                VkCommandBuffer.HandleType commandBuffer);

            public CmdNextSubpassDelegate CmdNextSubpass { get; }
            public delegate void CmdNextSubpassDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkSubpassContents contents);

            public CmdBindPipelineDelegate CmdBindPipeline { get; }
            public delegate void CmdBindPipelineDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineBindPoint pipelineBindPoint,
                VkPipeline.HandleType pipeline);

            public CmdBindDescriptorSetsDelegate CmdBindDescriptorSets { get; }
            public delegate void CmdBindDescriptorSetsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineBindPoint pipelineBindPoint,
                VkPipelineLayout.HandleType layout,
                int firstSet,
                int descriptorSetCount,
                VkDescriptorSet.HandleType* pDescriptorSets,
                int dynamicOffsetCount,
                int* pDynamicOffsets);

            public CmdPushConstantsDelegate CmdPushConstants { get; }
            public delegate void CmdPushConstantsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineLayout.HandleType layout,
                VkShaderStage stageFlags,
                int offset,
                int size,
                IntPtr pValues);

            public CmdResetQueryPoolDelegate CmdResetQueryPool { get; }
            public delegate void CmdResetQueryPoolDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkQueryPool.HandleType queryPool,
                int firstQuery,
                int queryCount);

            public CmdBeginQueryDelegate CmdBeginQuery { get; }
            public delegate void CmdBeginQueryDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkQueryPool.HandleType queryPool,
                int query,
                VkQueryControlFlags flags);

            public CmdEndQueryDelegate CmdEndQuery { get; }
            public delegate void CmdEndQueryDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkQueryPool.HandleType queryPool,
                int query);

            public CmdCopyQueryPoolResultsDelegate CmdCopyQueryPoolResults { get; }
            public delegate void CmdCopyQueryPoolResultsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkQueryPool.HandleType queryPool,
                int firstQuery,
                int queryCount,
                VkBuffer.HandleType dstBuffer,
                ulong dstOffset,
                ulong stride,
                VkQueryResultFlags flags);

            public CmdWriteTimestampDelegate CmdWriteTimestamp { get; }
            public delegate void CmdWriteTimestampDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineStageFlags pipelineStage,
                VkQueryPool.HandleType queryPool,
                int query);

            public CmdClearColorImageDelegate CmdClearColorImage { get; }
            public delegate void CmdClearColorImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType image,
                VkImageLayout imageLayout,
                VkClearColorValue* pColor,
                int rangeCount,
                VkImageSubresourceRange* pRanges);

            public CmdClearDepthStencilImageDelegate CmdClearDepthStencilImage { get; }
            public delegate void CmdClearDepthStencilImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType image,
                VkImageLayout imageLayout,
                VkClearDepthStencilValue* pDepthStencil,
                int rangeCount,
                VkImageSubresourceRange* pRanges);

            public CmdClearAttachmentsDelegate CmdClearAttachments { get; }
            public delegate void CmdClearAttachmentsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int attachmentCount,
                VkClearAttachment* pAttachments,
                int rectCount,
                VkClearRect* pRects);

            public CmdFillBufferDelegate CmdFillBuffer { get; }
            public delegate void CmdFillBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType dstBuffer,
                ulong dstOffset,
                ulong size,
                int data);

            public CmdUpdateBufferDelegate CmdUpdateBuffer { get; }
            public delegate void CmdUpdateBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType dstBuffer,
                ulong dstOffset,
                ulong dataSize,
                IntPtr pData);

            public CmdCopyBufferDelegate CmdCopyBuffer { get; }
            public delegate void CmdCopyBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType srcBuffer,
                VkBuffer.HandleType dstBuffer,
                int regionCount,
                VkBufferCopy* pRegions);

            public CmdCopyImageDelegate CmdCopyImage { get; }
            public delegate void CmdCopyImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType srcImage,
                VkImageLayout srcImageLayout,
                VkImage.HandleType dstImage,
                VkImageLayout dstImageLayout,
                int regionCount,
                VkImageCopy* pRegions);

            public CmdCopyBufferToImageDelegate CmdCopyBufferToImage { get; }
            public delegate void CmdCopyBufferToImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType srcBuffer,
                VkImage.HandleType dstImage,
                VkImageLayout dstImageLayout,
                int regionCount,
                VkBufferImageCopy* pRegions);

            public CmdCopyImageToBufferDelegate CmdCopyImageToBuffer { get; }
            public delegate void CmdCopyImageToBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType srcImage,
                VkImageLayout srcImageLayout,
                VkBuffer.HandleType dstBuffer,
                int regionCount,
                VkBufferImageCopy* pRegions);

            public CmdBlitImageDelegate CmdBlitImage { get; }
            public delegate void CmdBlitImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType srcImage,
                VkImageLayout srcImageLayout,
                VkImage.HandleType dstImage,
                VkImageLayout dstImageLayout,
                int regionCount,
                VkImageBlit* pRegions,
                VkFilter filter);

            public CmdResolveImageDelegate CmdResolveImage { get; }
            public delegate void CmdResolveImageDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkImage.HandleType srcImage,
                VkImageLayout srcImageLayout,
                VkImage.HandleType dstImage,
                VkImageLayout dstImageLayout,
                int regionCount,
                VkImageResolve* pRegions);

            public CmdBindIndexBufferDelegate CmdBindIndexBuffer { get; }
            public delegate void CmdBindIndexBufferDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType buffer,
                ulong offset,
                VkIndexType indexType);

            public CmdDrawDelegate CmdDraw { get; }
            public delegate void CmdDrawDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int vertexCount,
                int instanceCount,
                int firstVertex,
                int firstInstance);

            public CmdDrawIndexedDelegate CmdDrawIndexed { get; }
            public delegate void CmdDrawIndexedDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int indexCount,
                int instanceCount,
                int firstIndex,
                int vertexOffset,
                int firstInstance);

            public CmdDrawIndirectDelegate CmdDrawIndirect { get; }
            public delegate void CmdDrawIndirectDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType buffer,
                ulong offset,
                int drawCount,
                int stride);

            public CmdDrawIndexedIndirectDelegate CmdDrawIndexedIndirect { get; }
            public delegate void CmdDrawIndexedIndirectDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType buffer,
                ulong offset,
                int drawCount,
                int stride);

            public CmdBindVertexBuffersDelegate CmdBindVertexBuffers { get; }
            public delegate void CmdBindVertexBuffersDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int firstBinding,
                int bindingCount,
                VkBuffer.HandleType* pBuffers,
                ulong* pOffsets);

            public CmdSetViewportDelegate CmdSetViewport { get; }
            public delegate void CmdSetViewportDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int firstViewport,
                int viewportCount,
                VkViewport* pViewports);

            public CmdSetLineWidthDelegate CmdSetLineWidth { get; }
            public delegate void CmdSetLineWidthDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                float lineWidth);

            public CmdSetDepthBiasDelegate CmdSetDepthBias { get; }
            public delegate void CmdSetDepthBiasDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                float depthBiasConstantFactor,
                float depthBiasClamp,
                float depthBiasSlopeFactor);

            public CmdSetScissorDelegate CmdSetScissor { get; }
            public delegate void CmdSetScissorDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int firstScissor,
                int scissorCount,
                VkRect2D* pScissors);

            public CmdSetDepthBoundsDelegate CmdSetDepthBounds { get; }
            public delegate void CmdSetDepthBoundsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                float minDepthBounds,
                float maxDepthBounds);

            public CmdSetStencilCompareMaskDelegate CmdSetStencilCompareMask { get; }
            public delegate void CmdSetStencilCompareMaskDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkStencilFaceFlags faceMask,
                int compareMask);

            public CmdSetStencilWriteMaskDelegate CmdSetStencilWriteMask { get; }
            public delegate void CmdSetStencilWriteMaskDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkStencilFaceFlags faceMask,
                int writeMask);

            public CmdSetStencilReferenceDelegate CmdSetStencilReference { get; }
            public delegate void CmdSetStencilReferenceDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkStencilFaceFlags faceMask,
                int reference);

            public CmdSetBlendConstantsDelegate CmdSetBlendConstants { get; }
            public delegate void CmdSetBlendConstantsDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkColor4* blendConstants);

            public CmdDispatchDelegate CmdDispatch { get; }
            public delegate void CmdDispatchDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                int x,
                int y,
                int z);

            public CmdDispatchIndirectDelegate CmdDispatchIndirect { get; }
            public delegate void CmdDispatchIndirectDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkBuffer.HandleType buffer,
                ulong offset);

            public CmdDebugMarkerBeginEXTDelegate CmdDebugMarkerBeginEXT { get; }
            public delegate void CmdDebugMarkerBeginEXTDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkDebugMarkerMarkerInfoEXT.Raw* pMarkerInfo);

            public CmdDebugMarkerEndEXTDelegate CmdDebugMarkerEndEXT { get; }
            public delegate void CmdDebugMarkerEndEXTDelegate(
                VkCommandBuffer.HandleType commandBuffer);

            public CmdDebugMarkerInsertEXTDelegate CmdDebugMarkerInsertEXT { get; }
            public delegate void CmdDebugMarkerInsertEXTDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkDebugMarkerMarkerInfoEXT.Raw* pMarkerInfo);

            public DestroyFenceDelegate DestroyFence { get; }
            public delegate void DestroyFenceDelegate(
                HandleType device,
                VkFence.HandleType fence,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetFenceStatusDelegate GetFenceStatus { get; }
            public delegate VkResult GetFenceStatusDelegate(
                HandleType device,
                VkFence.HandleType fence);

            public DestroySemaphoreDelegate DestroySemaphore { get; }
            public delegate void DestroySemaphoreDelegate(
                HandleType device,
                VkSemaphore.HandleType semaphore,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyEventDelegate DestroyEvent { get; }
            public delegate void DestroyEventDelegate(
                HandleType device,
                VkEvent.HandleType eventObj,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetEventStatusDelegate GetEventStatus { get; }
            public delegate VkResult GetEventStatusDelegate(
                HandleType device,
                VkEvent.HandleType eventObj);

            public SetEventDelegate SetEvent { get; }
            public delegate VkResult SetEventDelegate(
                HandleType device,
                VkEvent.HandleType eventObj);

            public ResetEventDelegate ResetEvent { get; }
            public delegate VkResult ResetEventDelegate(
                HandleType device,
                VkEvent.HandleType eventObj);

            public DestroyBufferDelegate DestroyBuffer { get; }
            public delegate void DestroyBufferDelegate(
                HandleType device,
                VkBuffer.HandleType buffer,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetBufferMemoryRequirementsDelegate GetBufferMemoryRequirements { get; }
            public delegate void GetBufferMemoryRequirementsDelegate(
                HandleType device,
                VkBuffer.HandleType buffer,
                VkMemoryRequirements* pMemoryRequirements);

            public BindBufferMemoryDelegate BindBufferMemory { get; }
            public delegate VkResult BindBufferMemoryDelegate(
                HandleType device,
                VkBuffer.HandleType buffer,
                VkDeviceMemory.HandleType memory,
                ulong memoryOffset);

            public DestroyImageDelegate DestroyImage { get; }
            public delegate void DestroyImageDelegate(
                HandleType device,
                VkImage.HandleType image,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetImageSubresourceLayoutDelegate GetImageSubresourceLayout { get; }
            public delegate void GetImageSubresourceLayoutDelegate(
                HandleType device,
                VkImage.HandleType image,
                VkImageSubresource* pSubresource,
                VkSubresourceLayout* pLayout);

            public GetImageMemoryRequirementsDelegate GetImageMemoryRequirements { get; }
            public delegate void GetImageMemoryRequirementsDelegate(
                HandleType device,
                VkImage.HandleType image,
                VkMemoryRequirements* pMemoryRequirements);

            public BindImageMemoryDelegate BindImageMemory { get; }
            public delegate VkResult BindImageMemoryDelegate(
                HandleType device,
                VkImage.HandleType image,
                VkDeviceMemory.HandleType memory,
                ulong memoryOffset);

            public DestroyRenderPassDelegate DestroyRenderPass { get; }
            public delegate void DestroyRenderPassDelegate(
                HandleType device,
                VkRenderPass.HandleType renderPass,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetRenderAreaGranularityDelegate GetRenderAreaGranularity { get; }
            public delegate void GetRenderAreaGranularityDelegate(
                HandleType device,
                VkRenderPass.HandleType renderPass,
                VkExtent2D* pGranularity);

            public DestroyFramebufferDelegate DestroyFramebuffer { get; }
            public delegate void DestroyFramebufferDelegate(
                HandleType device,
                VkFramebuffer.HandleType framebuffer,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyImageViewDelegate DestroyImageView { get; }
            public delegate void DestroyImageViewDelegate(
                HandleType device,
                VkImageView.HandleType imageView,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyShaderModuleDelegate DestroyShaderModule { get; }
            public delegate void DestroyShaderModuleDelegate(
                HandleType device,
                VkShaderModule.HandleType shaderModule,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyPipelineDelegate DestroyPipeline { get; }
            public delegate void DestroyPipelineDelegate(
                HandleType device,
                VkPipeline.HandleType pipeline,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyPipelineLayoutDelegate DestroyPipelineLayout { get; }
            public delegate void DestroyPipelineLayoutDelegate(
                HandleType device,
                VkPipelineLayout.HandleType pipelineLayout,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyPipelineCacheDelegate DestroyPipelineCache { get; }
            public delegate void DestroyPipelineCacheDelegate(
                HandleType device,
                VkPipelineCache.HandleType pipelineCache,
                VkAllocationCallbacks.Raw* pAllocator);

            public MergePipelineCachesDelegate MergePipelineCaches { get; }
            public delegate VkResult MergePipelineCachesDelegate(
                HandleType device,
                VkPipelineCache.HandleType dstCache,
                int srcCacheCount,
                VkPipelineCache.HandleType* pSrcCaches);

            public GetPipelineCacheDataDelegate GetPipelineCacheData { get; }
            public delegate VkResult GetPipelineCacheDataDelegate(
                HandleType device,
                VkPipelineCache.HandleType pipelineCache,
                IntPtr* pDataSize,
                void* pData);

            public FreeMemoryDelegate FreeMemory { get; }
            public delegate void FreeMemoryDelegate(
                HandleType device,
                VkDeviceMemory.HandleType memory,
                VkAllocationCallbacks.Raw* pAllocator);

            public MapMemoryDelegate MapMemory { get; }
            public delegate VkResult MapMemoryDelegate(
                HandleType device,
                VkDeviceMemory.HandleType memory,
                ulong offset,
                ulong size,
                VkMemoryMapFlags flags,
                IntPtr* ppData);

            public UnmapMemoryDelegate UnmapMemory { get; }
            public delegate void UnmapMemoryDelegate(
                HandleType device,
                VkDeviceMemory.HandleType memory);

            public GetDeviceMemoryCommitmentDelegate GetDeviceMemoryCommitment { get; }
            public delegate void GetDeviceMemoryCommitmentDelegate(
                HandleType device,
                VkDeviceMemory.HandleType memory,
                ulong* pCommittedMemoryInBytes);

            public DestroyBufferViewDelegate DestroyBufferView { get; }
            public delegate void DestroyBufferViewDelegate(
                HandleType device,
                VkBufferView.HandleType bufferView,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroySamplerDelegate DestroySampler { get; }
            public delegate void DestroySamplerDelegate(
                HandleType device,
                VkSampler.HandleType sampler,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyDescriptorSetLayoutDelegate DestroyDescriptorSetLayout { get; }
            public delegate void DestroyDescriptorSetLayoutDelegate(
                HandleType device,
                VkDescriptorSetLayout.HandleType descriptorSetLayout,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyDescriptorPoolDelegate DestroyDescriptorPool { get; }
            public delegate void DestroyDescriptorPoolDelegate(
                HandleType device,
                VkDescriptorPool.HandleType descriptorPool,
                VkAllocationCallbacks.Raw* pAllocator);

            public ResetDescriptorPoolDelegate ResetDescriptorPool { get; }
            public delegate VkResult ResetDescriptorPoolDelegate(
                HandleType device,
                VkDescriptorPool.HandleType descriptorPool,
                VkDescriptorPoolResetFlags flags);

            public DestroyQueryPoolDelegate DestroyQueryPool { get; }
            public delegate void DestroyQueryPoolDelegate(
                HandleType device,
                VkQueryPool.HandleType queryPool,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetQueryPoolResultsDelegate GetQueryPoolResults { get; }
            public delegate VkResult GetQueryPoolResultsDelegate(
                HandleType device,
                VkQueryPool.HandleType queryPool,
                int firstQuery,
                int queryCount,
                IntPtr dataSize,
                IntPtr pData,
                ulong stride,
                VkQueryResultFlags flags);

            public DestroySurfaceKHRDelegate DestroySurfaceKHR { get; }
            public delegate void DestroySurfaceKHRDelegate(
                VkInstance instance,
                VkSurfaceKHR.HandleType surface,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroySwapchainKHRDelegate DestroySwapchainKHR { get; }
            public delegate void DestroySwapchainKHRDelegate(
                HandleType device,
                VkSwapchainKHR.HandleType swapchain,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetSwapchainImagesKHRDelegate GetSwapchainImagesKHR { get; }
            public delegate VkResult GetSwapchainImagesKHRDelegate(
                HandleType device,
                VkSwapchainKHR.HandleType swapchain,
                int* pSwapchainImageCount,
                VkImage.HandleType* pSwapchainImages);

            public AcquireNextImageKHRDelegate AcquireNextImageKHR { get; }
            public delegate VkResult AcquireNextImageKHRDelegate(
                HandleType device,
                VkSwapchainKHR.HandleType swapchain,
                ulong timeout,
                VkSemaphore.HandleType semaphore,
                VkFence.HandleType fence,
                int* pImageIndex);

            public DestroyDebugReportCallbackEXTDelegate DestroyDebugReportCallbackEXT { get; }
            public delegate void DestroyDebugReportCallbackEXTDelegate(
                VkInstance instance,
                VkDebugReportCallbackEXT.HandleType callback,
                VkAllocationCallbacks.Raw* pAllocator);

            public DestroyDeviceDelegate DestroyDevice { get; }
            public delegate void DestroyDeviceDelegate(
                HandleType device,
                VkAllocationCallbacks.Raw* pAllocator);

            public DeviceWaitIdleDelegate DeviceWaitIdle { get; }
            public delegate VkResult DeviceWaitIdleDelegate(
                HandleType device);

            public CreateCommandPoolDelegate CreateCommandPool { get; }
            public delegate VkResult CreateCommandPoolDelegate(
                HandleType device,
                VkCommandPoolCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkCommandPool.HandleType* pCommandPool);

            public AllocateCommandBuffersDelegate AllocateCommandBuffers { get; }
            public delegate VkResult AllocateCommandBuffersDelegate(
                HandleType device,
                VkCommandBufferAllocateInfo.Raw* pAllocateInfo,
                VkCommandBuffer.HandleType* pCommandBuffers);

            public CreateFenceDelegate CreateFence { get; }
            public delegate VkResult CreateFenceDelegate(
                HandleType device,
                VkFenceCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkFence.HandleType* pFence);

            public ResetFencesDelegate ResetFences { get; }
            public delegate VkResult ResetFencesDelegate(
                HandleType device,
                int fenceCount,
                VkFence.HandleType* pFences);

            public WaitForFencesDelegate WaitForFences { get; }
            public delegate VkResult WaitForFencesDelegate(
                HandleType device,
                int fenceCount,
                VkFence.HandleType* pFences,
                VkBool32 waitAll,
                ulong timeout);

            public CreateSemaphoreDelegate CreateSemaphore { get; }
            public delegate VkResult CreateSemaphoreDelegate(
                HandleType device,
                VkSemaphoreCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSemaphore.HandleType* pSemaphore);

            public CreateEventDelegate CreateEvent { get; }
            public delegate VkResult CreateEventDelegate(
                HandleType device,
                VkEventCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkEvent.HandleType* pEvent);

            public CreateRenderPassDelegate CreateRenderPass { get; }
            public delegate VkResult CreateRenderPassDelegate(
                HandleType device,
                VkRenderPassCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkRenderPass.HandleType* pRenderPass);

            public CreateFramebufferDelegate CreateFramebuffer { get; }
            public delegate VkResult CreateFramebufferDelegate(
                HandleType device,
                VkFramebufferCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkFramebuffer.HandleType* pFramebuffer);

            public CreateShaderModuleDelegate CreateShaderModule { get; }
            public delegate VkResult CreateShaderModuleDelegate(
                HandleType device,
                VkShaderModuleCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkShaderModule.HandleType* pShaderModule);

            public CreateComputePipelinesDelegate CreateComputePipelines { get; }
            public delegate VkResult CreateComputePipelinesDelegate(
                HandleType device,
                VkPipelineCache.HandleType pipelineCache,
                int createInfoCount,
                VkComputePipelineCreateInfo.Raw* pCreateInfos,
                VkAllocationCallbacks.Raw* pAllocator,
                VkPipeline.HandleType* pPipelines);

            public CreateGraphicsPipelinesDelegate CreateGraphicsPipelines { get; }
            public delegate VkResult CreateGraphicsPipelinesDelegate(
                HandleType device,
                VkPipelineCache.HandleType pipelineCache,
                int createInfoCount,
                VkGraphicsPipelineCreateInfo.Raw* pCreateInfos,
                VkAllocationCallbacks.Raw* pAllocator,
                VkPipeline.HandleType* pPipelines);

            public CreatePipelineCacheDelegate CreatePipelineCache { get; }
            public delegate VkResult CreatePipelineCacheDelegate(
                HandleType device,
                VkPipelineCacheCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkPipelineCache.HandleType* pPipelineCache);

            public AllocateMemoryDelegate AllocateMemory { get; }
            public delegate VkResult AllocateMemoryDelegate(
                HandleType device,
                VkMemoryAllocateInfo.Raw* pAllocateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkDeviceMemory.HandleType* pMemory);

            public FlushMappedMemoryRangesDelegate FlushMappedMemoryRanges { get; }
            public delegate VkResult FlushMappedMemoryRangesDelegate(
                HandleType device,
                int memoryRangeCount,
                VkMappedMemoryRange.Raw* pMemoryRanges);

            public InvalidateMappedMemoryRangesDelegate InvalidateMappedMemoryRanges { get; }
            public delegate VkResult InvalidateMappedMemoryRangesDelegate(
                HandleType device,
                int memoryRangeCount,
                VkMappedMemoryRange.Raw* pMemoryRanges);

            public CreateBufferDelegate CreateBuffer { get; }
            public delegate VkResult CreateBufferDelegate(
                HandleType device,
                VkBufferCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkBuffer.HandleType* pBuffer);

            public CreateBufferViewDelegate CreateBufferView { get; }
            public delegate VkResult CreateBufferViewDelegate(
                HandleType device,
                VkBufferViewCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkBufferView.HandleType* pView);

            public CreateImageDelegate CreateImage { get; }
            public delegate VkResult CreateImageDelegate(
                HandleType device,
                VkImageCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkImage.HandleType* pImage);

            public CreateImageViewDelegate CreateImageView { get; }
            public delegate VkResult CreateImageViewDelegate(
                HandleType device,
                VkImageViewCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkImageView.HandleType* pView);

            public CreateSamplerDelegate CreateSampler { get; }
            public delegate VkResult CreateSamplerDelegate(
                HandleType device,
                VkSamplerCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSampler.HandleType* pSampler);

            public CreateDescriptorSetLayoutDelegate CreateDescriptorSetLayout { get; }
            public delegate VkResult CreateDescriptorSetLayoutDelegate(
                HandleType device,
                VkDescriptorSetLayoutCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkDescriptorSetLayout.HandleType* pSetLayout);

            public CreatePipelineLayoutDelegate CreatePipelineLayout { get; }
            public delegate VkResult CreatePipelineLayoutDelegate(
                HandleType device,
                VkPipelineLayoutCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkPipelineLayout.HandleType* pPipelineLayout);

            public CreateDescriptorPoolDelegate CreateDescriptorPool { get; }
            public delegate VkResult CreateDescriptorPoolDelegate(
                HandleType device,
                VkDescriptorPoolCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkDescriptorPool.HandleType* pDescriptorPool);

            public AllocateDescriptorSetsDelegate AllocateDescriptorSets { get; }
            public delegate VkResult AllocateDescriptorSetsDelegate(
                HandleType device,
                VkDescriptorSetAllocateInfo.Raw* pAllocateInfo,
                VkDescriptorSet.HandleType* pDescriptorSets);

            public FreeDescriptorSetsDelegate FreeDescriptorSets { get; }
            public delegate VkResult FreeDescriptorSetsDelegate(
                HandleType device,
                VkDescriptorPool.HandleType descriptorPool,
                int descriptorSetCount,
                VkDescriptorSet.HandleType* pDescriptorSets);

            public UpdateDescriptorSetsDelegate UpdateDescriptorSets { get; }
            public delegate void UpdateDescriptorSetsDelegate(
                HandleType device,
                int descriptorWriteCount,
                VkWriteDescriptorSet.Raw* pDescriptorWrites,
                int descriptorCopyCount,
                VkCopyDescriptorSet.Raw* pDescriptorCopies);

            public CreateQueryPoolDelegate CreateQueryPool { get; }
            public delegate VkResult CreateQueryPoolDelegate(
                HandleType device,
                VkQueryPoolCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkQueryPool.HandleType* pQueryPool);

            public GetImageSparseMemoryRequirementsDelegate GetImageSparseMemoryRequirements { get; }
            public delegate void GetImageSparseMemoryRequirementsDelegate(
                HandleType device,
                VkImage.HandleType image,
                int* pSparseMemoryRequirementCount,
                VkSparseImageMemoryRequirements* pSparseMemoryRequirements);

            public CreateSwapchainKHRDelegate CreateSwapchainKHR { get; }
            public delegate VkResult CreateSwapchainKHRDelegate(
                HandleType device,
                VkSwapchainCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSwapchainKHR.HandleType* pSwapchain);

            public CreateSharedSwapchainsKHRDelegate CreateSharedSwapchainsKHR { get; }
            public delegate VkResult CreateSharedSwapchainsKHRDelegate(
                HandleType device,
                int swapchainCount,
                VkSwapchainCreateInfoKHR.Raw* pCreateInfos,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSwapchainKHR.HandleType* pSwapchains);

            public DebugMarkerSetObjectNameEXTDelegate DebugMarkerSetObjectNameEXT { get; }
            public delegate VkResult DebugMarkerSetObjectNameEXTDelegate(
                HandleType device,
                VkDebugMarkerObjectNameInfoEXT.Raw* pNameInfo);

            public DebugMarkerSetObjectTagEXTDelegate DebugMarkerSetObjectTagEXT { get; }
            public delegate VkResult DebugMarkerSetObjectTagEXTDelegate(
                HandleType device,
                VkDebugMarkerObjectTagInfoEXT.Raw* pTagInfo);

            public DirectFunctions(IVkDevice device)
            {
                this.device = device;

                GetDeviceProcAddr = VkHelpers.GetInstanceDelegate<GetDeviceProcAddrDelegate>(device.Instance, "vkGetDeviceProcAddr");
                GetDeviceQueue = GetDeviceDelegate<GetDeviceQueueDelegate>("vkGetDeviceQueue");
                QueueSubmit = GetDeviceDelegate<QueueSubmitDelegate>("vkQueueSubmit");
                QueueWaitIdle = GetDeviceDelegate<QueueWaitIdleDelegate>("vkQueueWaitIdle");
                QueueBindSparse = GetDeviceDelegate<QueueBindSparseDelegate>("vkQueueBindSparse");
                QueuePresentKHR = GetDeviceDelegate<QueuePresentKHRDelegate>("vkQueuePresentKHR");
                DestroyCommandPool = GetDeviceDelegate<DestroyCommandPoolDelegate>("vkDestroyCommandPool");
                ResetCommandPool = GetDeviceDelegate<ResetCommandPoolDelegate>("vkResetCommandPool");
                FreeCommandBuffers = GetDeviceDelegate<FreeCommandBuffersDelegate>("vkFreeCommandBuffers");
                ResetCommandBuffer = GetDeviceDelegate<ResetCommandBufferDelegate>("vkResetCommandBuffer");
                BeginCommandBuffer = GetDeviceDelegate<BeginCommandBufferDelegate>("vkBeginCommandBuffer");
                EndCommandBuffer = GetDeviceDelegate<EndCommandBufferDelegate>("vkEndCommandBuffer");
                CmdExecuteCommands = GetDeviceDelegate<CmdExecuteCommandsDelegate>("vkCmdExecuteCommands");
                CmdSetEvent = GetDeviceDelegate<CmdSetEventDelegate>("vkCmdSetEvent");
                CmdResetEvent = GetDeviceDelegate<CmdResetEventDelegate>("vkCmdResetEvent");
                CmdWaitEvents = GetDeviceDelegate<CmdWaitEventsDelegate>("vkCmdWaitEvents");
                CmdPipelineBarrier = GetDeviceDelegate<CmdPipelineBarrierDelegate>("vkCmdPipelineBarrier");
                CmdBeginRenderPass = GetDeviceDelegate<CmdBeginRenderPassDelegate>("vkCmdBeginRenderPass");
                CmdEndRenderPass = GetDeviceDelegate<CmdEndRenderPassDelegate>("vkCmdEndRenderPass");
                CmdNextSubpass = GetDeviceDelegate<CmdNextSubpassDelegate>("vkCmdNextSubpass");
                CmdBindPipeline = GetDeviceDelegate<CmdBindPipelineDelegate>("vkCmdBindPipeline");
                CmdBindDescriptorSets = GetDeviceDelegate<CmdBindDescriptorSetsDelegate>("vkCmdBindDescriptorSets");
                CmdPushConstants = GetDeviceDelegate<CmdPushConstantsDelegate>("vkCmdPushConstants");
                CmdResetQueryPool = GetDeviceDelegate<CmdResetQueryPoolDelegate>("vkCmdResetQueryPool");
                CmdBeginQuery = GetDeviceDelegate<CmdBeginQueryDelegate>("vkCmdBeginQuery");
                CmdEndQuery = GetDeviceDelegate<CmdEndQueryDelegate>("vkCmdEndQuery");
                CmdCopyQueryPoolResults = GetDeviceDelegate<CmdCopyQueryPoolResultsDelegate>("vkCmdCopyQueryPoolResults");
                CmdWriteTimestamp = GetDeviceDelegate<CmdWriteTimestampDelegate>("vkCmdWriteTimestamp");
                CmdClearColorImage = GetDeviceDelegate<CmdClearColorImageDelegate>("vkCmdClearColorImage");
                CmdClearDepthStencilImage = GetDeviceDelegate<CmdClearDepthStencilImageDelegate>("vkCmdClearDepthStencilImage");
                CmdClearAttachments = GetDeviceDelegate<CmdClearAttachmentsDelegate>("vkCmdClearAttachments");
                CmdFillBuffer = GetDeviceDelegate<CmdFillBufferDelegate>("vkCmdFillBuffer");
                CmdUpdateBuffer = GetDeviceDelegate<CmdUpdateBufferDelegate>("vkCmdUpdateBuffer");
                CmdCopyBuffer = GetDeviceDelegate<CmdCopyBufferDelegate>("vkCmdCopyBuffer");
                CmdCopyImage = GetDeviceDelegate<CmdCopyImageDelegate>("vkCmdCopyImage");
                CmdCopyBufferToImage = GetDeviceDelegate<CmdCopyBufferToImageDelegate>("vkCmdCopyBufferToImage");
                CmdCopyImageToBuffer = GetDeviceDelegate<CmdCopyImageToBufferDelegate>("vkCmdCopyImageToBuffer");
                CmdBlitImage = GetDeviceDelegate<CmdBlitImageDelegate>("vkCmdBlitImage");
                CmdResolveImage = GetDeviceDelegate<CmdResolveImageDelegate>("vkCmdResolveImage");
                CmdBindIndexBuffer = GetDeviceDelegate<CmdBindIndexBufferDelegate>("vkCmdBindIndexBuffer");
                CmdDraw = GetDeviceDelegate<CmdDrawDelegate>("vkCmdDraw");
                CmdDrawIndexed = GetDeviceDelegate<CmdDrawIndexedDelegate>("vkCmdDrawIndexed");
                CmdDrawIndirect = GetDeviceDelegate<CmdDrawIndirectDelegate>("vkCmdDrawIndirect");
                CmdDrawIndexedIndirect = GetDeviceDelegate<CmdDrawIndexedIndirectDelegate>("vkCmdDrawIndexedIndirect");
                CmdBindVertexBuffers = GetDeviceDelegate<CmdBindVertexBuffersDelegate>("vkCmdBindVertexBuffers");
                CmdSetViewport = GetDeviceDelegate<CmdSetViewportDelegate>("vkCmdSetViewport");
                CmdSetLineWidth = GetDeviceDelegate<CmdSetLineWidthDelegate>("vkCmdSetLineWidth");
                CmdSetDepthBias = GetDeviceDelegate<CmdSetDepthBiasDelegate>("vkCmdSetDepthBias");
                CmdSetScissor = GetDeviceDelegate<CmdSetScissorDelegate>("vkCmdSetScissor");
                CmdSetDepthBounds = GetDeviceDelegate<CmdSetDepthBoundsDelegate>("vkCmdSetDepthBounds");
                CmdSetStencilCompareMask = GetDeviceDelegate<CmdSetStencilCompareMaskDelegate>("vkCmdSetStencilCompareMask");
                CmdSetStencilWriteMask = GetDeviceDelegate<CmdSetStencilWriteMaskDelegate>("vkCmdSetStencilWriteMask");
                CmdSetStencilReference = GetDeviceDelegate<CmdSetStencilReferenceDelegate>("vkCmdSetStencilReference");
                CmdSetBlendConstants = GetDeviceDelegate<CmdSetBlendConstantsDelegate>("vkCmdSetBlendConstants");
                CmdDispatch = GetDeviceDelegate<CmdDispatchDelegate>("vkCmdDispatch");
                CmdDispatchIndirect = GetDeviceDelegate<CmdDispatchIndirectDelegate>("vkCmdDispatchIndirect");
                CmdDebugMarkerBeginEXT = GetDeviceDelegate<CmdDebugMarkerBeginEXTDelegate>("vkCmdDebugMarkerBeginEXT");
                CmdDebugMarkerEndEXT = GetDeviceDelegate<CmdDebugMarkerEndEXTDelegate>("vkCmdDebugMarkerEndEXT");
                CmdDebugMarkerInsertEXT = GetDeviceDelegate<CmdDebugMarkerInsertEXTDelegate>("vkCmdDebugMarkerInsertEXT");
                DestroyFence = GetDeviceDelegate<DestroyFenceDelegate>("vkDestroyFence");
                GetFenceStatus = GetDeviceDelegate<GetFenceStatusDelegate>("vkGetFenceStatus");
                DestroySemaphore = GetDeviceDelegate<DestroySemaphoreDelegate>("vkDestroySemaphore");
                DestroyEvent = GetDeviceDelegate<DestroyEventDelegate>("vkDestroyEvent");
                GetEventStatus = GetDeviceDelegate<GetEventStatusDelegate>("vkGetEventStatus");
                SetEvent = GetDeviceDelegate<SetEventDelegate>("vkSetEvent");
                ResetEvent = GetDeviceDelegate<ResetEventDelegate>("vkResetEvent");
                DestroyBuffer = GetDeviceDelegate<DestroyBufferDelegate>("vkDestroyBuffer");
                GetBufferMemoryRequirements = GetDeviceDelegate<GetBufferMemoryRequirementsDelegate>("vkGetBufferMemoryRequirements");
                BindBufferMemory = GetDeviceDelegate<BindBufferMemoryDelegate>("vkBindBufferMemory");
                DestroyImage = GetDeviceDelegate<DestroyImageDelegate>("vkDestroyImage");
                GetImageSubresourceLayout = GetDeviceDelegate<GetImageSubresourceLayoutDelegate>("vkGetImageSubresourceLayout");
                GetImageMemoryRequirements = GetDeviceDelegate<GetImageMemoryRequirementsDelegate>("vkGetImageMemoryRequirements");
                BindImageMemory = GetDeviceDelegate<BindImageMemoryDelegate>("vkBindImageMemory");
                DestroyRenderPass = GetDeviceDelegate<DestroyRenderPassDelegate>("vkDestroyRenderPass");
                GetRenderAreaGranularity = GetDeviceDelegate<GetRenderAreaGranularityDelegate>("vkGetRenderAreaGranularity");
                DestroyFramebuffer = GetDeviceDelegate<DestroyFramebufferDelegate>("vkDestroyFramebuffer");
                DestroyImageView = GetDeviceDelegate<DestroyImageViewDelegate>("vkDestroyImageView");
                DestroyShaderModule = GetDeviceDelegate<DestroyShaderModuleDelegate>("vkDestroyShaderModule");
                DestroyPipeline = GetDeviceDelegate<DestroyPipelineDelegate>("vkDestroyPipeline");
                DestroyPipelineLayout = GetDeviceDelegate<DestroyPipelineLayoutDelegate>("vkDestroyPipelineLayout");
                DestroyPipelineCache = GetDeviceDelegate<DestroyPipelineCacheDelegate>("vkDestroyPipelineCache");
                MergePipelineCaches = GetDeviceDelegate<MergePipelineCachesDelegate>("vkMergePipelineCaches");
                GetPipelineCacheData = GetDeviceDelegate<GetPipelineCacheDataDelegate>("vkGetPipelineCacheData");
                FreeMemory = GetDeviceDelegate<FreeMemoryDelegate>("vkFreeMemory");
                MapMemory = GetDeviceDelegate<MapMemoryDelegate>("vkMapMemory");
                UnmapMemory = GetDeviceDelegate<UnmapMemoryDelegate>("vkUnmapMemory");
                GetDeviceMemoryCommitment = GetDeviceDelegate<GetDeviceMemoryCommitmentDelegate>("vkGetDeviceMemoryCommitment");
                DestroyBufferView = GetDeviceDelegate<DestroyBufferViewDelegate>("vkDestroyBufferView");
                DestroySampler = GetDeviceDelegate<DestroySamplerDelegate>("vkDestroySampler");
                DestroyDescriptorSetLayout = GetDeviceDelegate<DestroyDescriptorSetLayoutDelegate>("vkDestroyDescriptorSetLayout");
                DestroyDescriptorPool = GetDeviceDelegate<DestroyDescriptorPoolDelegate>("vkDestroyDescriptorPool");
                ResetDescriptorPool = GetDeviceDelegate<ResetDescriptorPoolDelegate>("vkResetDescriptorPool");
                DestroyQueryPool = GetDeviceDelegate<DestroyQueryPoolDelegate>("vkDestroyQueryPool");
                GetQueryPoolResults = GetDeviceDelegate<GetQueryPoolResultsDelegate>("vkGetQueryPoolResults");
                DestroySurfaceKHR = GetDeviceDelegate<DestroySurfaceKHRDelegate>("vkDestroySurfaceKHR");
                DestroySwapchainKHR = GetDeviceDelegate<DestroySwapchainKHRDelegate>("vkDestroySwapchainKHR");
                GetSwapchainImagesKHR = GetDeviceDelegate<GetSwapchainImagesKHRDelegate>("vkGetSwapchainImagesKHR");
                AcquireNextImageKHR = GetDeviceDelegate<AcquireNextImageKHRDelegate>("vkAcquireNextImageKHR");
                DestroyDebugReportCallbackEXT = GetDeviceDelegate<DestroyDebugReportCallbackEXTDelegate>("vkDestroyDebugReportCallbackEXT");
                DestroyDevice = GetDeviceDelegate<DestroyDeviceDelegate>("vkDestroyDevice");
                DeviceWaitIdle = GetDeviceDelegate<DeviceWaitIdleDelegate>("vkDeviceWaitIdle");
                CreateCommandPool = GetDeviceDelegate<CreateCommandPoolDelegate>("vkCreateCommandPool");
                AllocateCommandBuffers = GetDeviceDelegate<AllocateCommandBuffersDelegate>("vkAllocateCommandBuffers");
                CreateFence = GetDeviceDelegate<CreateFenceDelegate>("vkCreateFence");
                ResetFences = GetDeviceDelegate<ResetFencesDelegate>("vkResetFences");
                WaitForFences = GetDeviceDelegate<WaitForFencesDelegate>("vkWaitForFences");
                CreateSemaphore = GetDeviceDelegate<CreateSemaphoreDelegate>("vkCreateSemaphore");
                CreateEvent = GetDeviceDelegate<CreateEventDelegate>("vkCreateEvent");
                CreateRenderPass = GetDeviceDelegate<CreateRenderPassDelegate>("vkCreateRenderPass");
                CreateFramebuffer = GetDeviceDelegate<CreateFramebufferDelegate>("vkCreateFramebuffer");
                CreateShaderModule = GetDeviceDelegate<CreateShaderModuleDelegate>("vkCreateShaderModule");
                CreateComputePipelines = GetDeviceDelegate<CreateComputePipelinesDelegate>("vkCreateComputePipelines");
                CreateGraphicsPipelines = GetDeviceDelegate<CreateGraphicsPipelinesDelegate>("vkCreateGraphicsPipelines");
                CreatePipelineCache = GetDeviceDelegate<CreatePipelineCacheDelegate>("vkCreatePipelineCache");
                AllocateMemory = GetDeviceDelegate<AllocateMemoryDelegate>("vkAllocateMemory");
                FlushMappedMemoryRanges = GetDeviceDelegate<FlushMappedMemoryRangesDelegate>("vkFlushMappedMemoryRanges");
                InvalidateMappedMemoryRanges = GetDeviceDelegate<InvalidateMappedMemoryRangesDelegate>("vkInvalidateMappedMemoryRanges");
                CreateBuffer = GetDeviceDelegate<CreateBufferDelegate>("vkCreateBuffer");
                CreateBufferView = GetDeviceDelegate<CreateBufferViewDelegate>("vkCreateBufferView");
                CreateImage = GetDeviceDelegate<CreateImageDelegate>("vkCreateImage");
                CreateImageView = GetDeviceDelegate<CreateImageViewDelegate>("vkCreateImageView");
                CreateSampler = GetDeviceDelegate<CreateSamplerDelegate>("vkCreateSampler");
                CreateDescriptorSetLayout = GetDeviceDelegate<CreateDescriptorSetLayoutDelegate>("vkCreateDescriptorSetLayout");
                CreatePipelineLayout = GetDeviceDelegate<CreatePipelineLayoutDelegate>("vkCreatePipelineLayout");
                CreateDescriptorPool = GetDeviceDelegate<CreateDescriptorPoolDelegate>("vkCreateDescriptorPool");
                AllocateDescriptorSets = GetDeviceDelegate<AllocateDescriptorSetsDelegate>("vkAllocateDescriptorSets");
                FreeDescriptorSets = GetDeviceDelegate<FreeDescriptorSetsDelegate>("vkFreeDescriptorSets");
                UpdateDescriptorSets = GetDeviceDelegate<UpdateDescriptorSetsDelegate>("vkUpdateDescriptorSets");
                CreateQueryPool = GetDeviceDelegate<CreateQueryPoolDelegate>("vkCreateQueryPool");
                GetImageSparseMemoryRequirements = GetDeviceDelegate<GetImageSparseMemoryRequirementsDelegate>("vkGetImageSparseMemoryRequirements");
                CreateSwapchainKHR = GetDeviceDelegate<CreateSwapchainKHRDelegate>("vkCreateSwapchainKHR");
                CreateSharedSwapchainsKHR = GetDeviceDelegate<CreateSharedSwapchainsKHRDelegate>("vkCreateSharedSwapchainsKHR");
                DebugMarkerSetObjectNameEXT = GetDeviceDelegate<DebugMarkerSetObjectNameEXTDelegate>("vkDebugMarkerSetObjectNameEXT");
                DebugMarkerSetObjectTagEXT = GetDeviceDelegate<DebugMarkerSetObjectTagEXTDelegate>("vkDebugMarkerSetObjectTagEXT");
            }

            public TDelegate GetDeviceDelegate<TDelegate>(string name)
            {
                IntPtr funPtr;
                fixed (byte* pName = name.ToAnsiArray())
                    funPtr = GetDeviceProcAddr(device.Handle, pName);
                return Marshal.GetDelegateForFunctionPointer<TDelegate>(funPtr);
            }
        }

        public IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex) =>
            queues.GetOrAdd(new ValuePair<int, int>(queueFamilyIndex, queueIndex), DoGetDeviceQueue);

        private IVkQueue DoGetDeviceQueue(ValuePair<int, int> key)
        {
            VkQueue.HandleType handle;
            Direct.GetDeviceQueue(Handle, (uint)key.First, (uint)key.Second, &handle);
            return new VkQueue(this, handle);
        }

        public void Dispose()
        {
            var unmanagedSize =
                Allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pAllocator = Allocator.MarshalIndirect(ref unmanaged);
                Direct.DestroyDevice(_device, _pAllocator);
            }
        }

        public VkResult WaitIdle()
        {
            var _device = Handle;
            return Direct.DeviceWaitIdle(_device);
        }

        public VkObjectResult<IVkCommandPool> CreateCommandPool(VkCommandPoolCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkCommandPool.HandleType _pCommandPool;
                var result = Direct.CreateCommandPool(_device, _pCreateInfo, _pAllocator, &_pCommandPool);
                var instance = result == VkResult.Success ? new VkCommandPool(this, _pCommandPool, allocator) : null;
                return new VkObjectResult<IVkCommandPool>(result, instance);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkCommandBuffer>> AllocateCommandBuffers(VkCommandBufferAllocateInfo allocateInfo)
        {
            var unmanagedSize =
                allocateInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pAllocateInfo = allocateInfo.MarshalIndirect(ref unmanaged);
                var handleArray = new VkCommandBuffer.HandleType[allocateInfo.CommandBufferCount];
                fixed (VkCommandBuffer.HandleType* _pCommandBuffers = handleArray)
                {
                    var result = Direct.AllocateCommandBuffers(_device, _pAllocateInfo, _pCommandBuffers);
                    var instance = result == VkResult.Success ? Enumerable.Range(0, handleArray.Length).Select(i => (IVkCommandBuffer)new VkCommandBuffer(this, handleArray[i])).ToArray() : null;
                    return new VkObjectResult<IReadOnlyList<IVkCommandBuffer>>(result, instance);
                }
            }
        }

        public VkObjectResult<IVkFence> CreateFence(VkFenceCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkFence.HandleType _pFence;
                var result = Direct.CreateFence(_device, _pCreateInfo, _pAllocator, &_pFence);
                var instance = result == VkResult.Success ? new VkFence(this, _pFence, allocator) : null;
                return new VkObjectResult<IVkFence>(result, instance);
            }
        }

        public VkResult ResetFences(IReadOnlyList<IVkFence> fences)
        {
            var unmanagedSize =
                fences.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _fenceCount = fences?.Count ?? 0;
                var _pFences = fences.MarshalDirect(ref unmanaged);
                return Direct.ResetFences(_device, _fenceCount, _pFences);
            }
        }

        public VkResult WaitForFences(IReadOnlyList<IVkFence> fences, bool waitAll, ulong timeout)
        {
            var unmanagedSize =
                fences.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _fenceCount = fences?.Count ?? 0;
                var _pFences = fences.MarshalDirect(ref unmanaged);
                var _waitAll = new VkBool32(waitAll);
                var _timeout = timeout;
                return Direct.WaitForFences(_device, _fenceCount, _pFences, _waitAll, _timeout);
            }
        }

        public VkObjectResult<IVkSemaphore> CreateSemaphore(VkSemaphoreCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkSemaphore.HandleType _pSemaphore;
                var result = Direct.CreateSemaphore(_device, _pCreateInfo, _pAllocator, &_pSemaphore);
                var instance = result == VkResult.Success ? new VkSemaphore(this, _pSemaphore, allocator) : null;
                return new VkObjectResult<IVkSemaphore>(result, instance);
            }
        }

        public VkObjectResult<IVkEvent> CreateEvent(VkEventCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkEvent.HandleType _pEvent;
                var result = Direct.CreateEvent(_device, _pCreateInfo, _pAllocator, &_pEvent);
                var instance = result == VkResult.Success ? new VkEvent(this, _pEvent, allocator) : null;
                return new VkObjectResult<IVkEvent>(result, instance);
            }
        }

        public VkObjectResult<IVkRenderPass> CreateRenderPass(VkRenderPassCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkRenderPass.HandleType _pRenderPass;
                var result = Direct.CreateRenderPass(_device, _pCreateInfo, _pAllocator, &_pRenderPass);
                var instance = result == VkResult.Success ? new VkRenderPass(this, _pRenderPass, allocator) : null;
                return new VkObjectResult<IVkRenderPass>(result, instance);
            }
        }

        public VkObjectResult<IVkFramebuffer> CreateFramebuffer(VkFramebufferCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkFramebuffer.HandleType _pFramebuffer;
                var result = Direct.CreateFramebuffer(_device, _pCreateInfo, _pAllocator, &_pFramebuffer);
                var instance = result == VkResult.Success ? new VkFramebuffer(this, _pFramebuffer, allocator) : null;
                return new VkObjectResult<IVkFramebuffer>(result, instance);
            }
        }

        public VkObjectResult<IVkShaderModule> CreateShaderModule(VkShaderModuleCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkShaderModule.HandleType _pShaderModule;
                var result = Direct.CreateShaderModule(_device, _pCreateInfo, _pAllocator, &_pShaderModule);
                var instance = result == VkResult.Success ? new VkShaderModule(this, _pShaderModule, allocator) : null;
                return new VkObjectResult<IVkShaderModule>(result, instance);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkPipeline>> CreateComputePipelines(IVkPipelineCache pipelineCache, IReadOnlyList<VkComputePipelineCreateInfo> createInfos, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfos.SizeOfMarshalDirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pipelineCache = pipelineCache?.Handle ?? VkPipelineCache.HandleType.Null;
                var _createInfoCount = createInfos?.Count ?? 0;
                var _pCreateInfos = createInfos.MarshalDirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                var handleArray = new VkPipeline.HandleType[_createInfoCount];
                fixed (VkPipeline.HandleType* _pPipelines = handleArray)
                {
                    var result = Direct.CreateComputePipelines(_device, _pipelineCache, _createInfoCount, _pCreateInfos, _pAllocator, _pPipelines);
                    var instance = result == VkResult.Success ? Enumerable.Range(0, handleArray.Length).Select(i => (IVkPipeline)new VkPipeline(this, handleArray[i], allocator)).ToArray() : null;
                    return new VkObjectResult<IReadOnlyList<IVkPipeline>>(result, instance);
                }
            }
        }

        public VkObjectResult<IReadOnlyList<IVkPipeline>> CreateGraphicsPipelines(IVkPipelineCache pipelineCache, IReadOnlyList<VkGraphicsPipelineCreateInfo> createInfos, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfos.SizeOfMarshalDirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pipelineCache = pipelineCache?.Handle ?? VkPipelineCache.HandleType.Null;
                var _createInfoCount = createInfos?.Count ?? 0;
                var _pCreateInfos = createInfos.MarshalDirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                var handleArray = new VkPipeline.HandleType[_createInfoCount];
                fixed (VkPipeline.HandleType* _pPipelines = handleArray)
                {
                    var result = Direct.CreateGraphicsPipelines(_device, _pipelineCache, _createInfoCount, _pCreateInfos, _pAllocator, _pPipelines);
                    var instance = result == VkResult.Success ? Enumerable.Range(0, handleArray.Length).Select(i => (IVkPipeline)new VkPipeline(this, handleArray[i], allocator)).ToArray() : null;
                    return new VkObjectResult<IReadOnlyList<IVkPipeline>>(result, instance);
                }
            }
        }

        public VkObjectResult<IVkPipelineCache> CreatePipelineCache(VkPipelineCacheCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkPipelineCache.HandleType _pPipelineCache;
                var result = Direct.CreatePipelineCache(_device, _pCreateInfo, _pAllocator, &_pPipelineCache);
                var instance = result == VkResult.Success ? new VkPipelineCache(this, _pPipelineCache, allocator) : null;
                return new VkObjectResult<IVkPipelineCache>(result, instance);
            }
        }

        public VkObjectResult<IVkDeviceMemory> AllocateMemory(VkMemoryAllocateInfo allocateInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                allocateInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pAllocateInfo = allocateInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkDeviceMemory.HandleType _pMemory;
                var result = Direct.AllocateMemory(_device, _pAllocateInfo, _pAllocator, &_pMemory);
                var instance = result == VkResult.Success ? new VkDeviceMemory(this, _pMemory, allocator) : null;
                return new VkObjectResult<IVkDeviceMemory>(result, instance);
            }
        }

        public VkResult FlushMappedMemoryRanges(IReadOnlyList<VkMappedMemoryRange> memoryRanges)
        {
            var unmanagedSize =
                memoryRanges.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _memoryRangeCount = memoryRanges?.Count ?? 0;
                var _pMemoryRanges = memoryRanges.MarshalDirect(ref unmanaged);
                return Direct.FlushMappedMemoryRanges(_device, _memoryRangeCount, _pMemoryRanges);
            }
        }

        public VkResult InvalidateMappedMemoryRanges(IReadOnlyList<VkMappedMemoryRange> memoryRanges)
        {
            var unmanagedSize =
                memoryRanges.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _memoryRangeCount = memoryRanges?.Count ?? 0;
                var _pMemoryRanges = memoryRanges.MarshalDirect(ref unmanaged);
                return Direct.InvalidateMappedMemoryRanges(_device, _memoryRangeCount, _pMemoryRanges);
            }
        }

        public VkObjectResult<IVkBuffer> CreateBuffer(VkBufferCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkBuffer.HandleType _pBuffer;
                var result = Direct.CreateBuffer(_device, _pCreateInfo, _pAllocator, &_pBuffer);
                var instance = result == VkResult.Success ? new VkBuffer(this, _pBuffer, allocator) : null;
                return new VkObjectResult<IVkBuffer>(result, instance);
            }
        }

        public VkObjectResult<IVkBufferView> CreateBufferView(VkBufferViewCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkBufferView.HandleType _pView;
                var result = Direct.CreateBufferView(_device, _pCreateInfo, _pAllocator, &_pView);
                var instance = result == VkResult.Success ? new VkBufferView(this, _pView, allocator) : null;
                return new VkObjectResult<IVkBufferView>(result, instance);
            }
        }

        public VkObjectResult<IVkImage> CreateImage(VkImageCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkImage.HandleType _pImage;
                var result = Direct.CreateImage(_device, _pCreateInfo, _pAllocator, &_pImage);
                var instance = result == VkResult.Success ? new VkImage(this, _pImage, allocator) : null;
                return new VkObjectResult<IVkImage>(result, instance);
            }
        }

        public VkObjectResult<IVkImageView> CreateImageView(VkImageViewCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkImageView.HandleType _pView;
                var result = Direct.CreateImageView(_device, _pCreateInfo, _pAllocator, &_pView);
                var instance = result == VkResult.Success ? new VkImageView(this, _pView, allocator) : null;
                return new VkObjectResult<IVkImageView>(result, instance);
            }
        }

        public VkObjectResult<IVkSampler> CreateSampler(VkSamplerCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkSampler.HandleType _pSampler;
                var result = Direct.CreateSampler(_device, _pCreateInfo, _pAllocator, &_pSampler);
                var instance = result == VkResult.Success ? new VkSampler(this, _pSampler, allocator) : null;
                return new VkObjectResult<IVkSampler>(result, instance);
            }
        }

        public VkObjectResult<IVkDescriptorSetLayout> CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkDescriptorSetLayout.HandleType _pSetLayout;
                var result = Direct.CreateDescriptorSetLayout(_device, _pCreateInfo, _pAllocator, &_pSetLayout);
                var instance = result == VkResult.Success ? new VkDescriptorSetLayout(this, _pSetLayout, allocator) : null;
                return new VkObjectResult<IVkDescriptorSetLayout>(result, instance);
            }
        }

        public VkObjectResult<IVkPipelineLayout> CreatePipelineLayout(VkPipelineLayoutCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkPipelineLayout.HandleType _pPipelineLayout;
                var result = Direct.CreatePipelineLayout(_device, _pCreateInfo, _pAllocator, &_pPipelineLayout);
                var instance = result == VkResult.Success ? new VkPipelineLayout(this, _pPipelineLayout, allocator) : null;
                return new VkObjectResult<IVkPipelineLayout>(result, instance);
            }
        }

        public VkObjectResult<IVkDescriptorPool> CreateDescriptorPool(VkDescriptorPoolCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkDescriptorPool.HandleType _pDescriptorPool;
                var result = Direct.CreateDescriptorPool(_device, _pCreateInfo, _pAllocator, &_pDescriptorPool);
                var instance = result == VkResult.Success ? new VkDescriptorPool(this, _pDescriptorPool, allocator) : null;
                return new VkObjectResult<IVkDescriptorPool>(result, instance);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkDescriptorSet>> AllocateDescriptorSets(VkDescriptorSetAllocateInfo allocateInfo)
        {
            var unmanagedSize =
                allocateInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pAllocateInfo = allocateInfo.MarshalIndirect(ref unmanaged);
                var handleArray = new VkDescriptorSet.HandleType[allocateInfo.SetLayouts.Count];
                fixed (VkDescriptorSet.HandleType* _pDescriptorSets = handleArray)
                {
                    var result = Direct.AllocateDescriptorSets(_device, _pAllocateInfo, _pDescriptorSets);
                    var instance = result == VkResult.Success ? Enumerable.Range(0, handleArray.Length).Select(i => (IVkDescriptorSet)new VkDescriptorSet(this, handleArray[i])).ToArray() : null;
                    return new VkObjectResult<IReadOnlyList<IVkDescriptorSet>>(result, instance);
                }
            }
        }

        public VkResult FreeDescriptorSets(IVkDescriptorPool descriptorPool, IReadOnlyList<IVkDescriptorSet> descriptorSets)
        {
            var unmanagedSize =
                descriptorSets.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _descriptorPool = descriptorPool?.Handle ?? VkDescriptorPool.HandleType.Null;
                var _descriptorSetCount = descriptorSets?.Count ?? 0;
                var _pDescriptorSets = descriptorSets.MarshalDirect(ref unmanaged);
                return Direct.FreeDescriptorSets(_device, _descriptorPool, _descriptorSetCount, _pDescriptorSets);
            }
        }

        public void UpdateDescriptorSets(IReadOnlyList<VkWriteDescriptorSet> descriptorWrites, IReadOnlyList<VkCopyDescriptorSet> descriptorCopies)
        {
            var unmanagedSize =
                descriptorWrites.SizeOfMarshalDirect() +
                descriptorCopies.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _descriptorWriteCount = descriptorWrites?.Count ?? 0;
                var _pDescriptorWrites = descriptorWrites.MarshalDirect(ref unmanaged);
                var _descriptorCopyCount = descriptorCopies?.Count ?? 0;
                var _pDescriptorCopies = descriptorCopies.MarshalDirect(ref unmanaged);
                Direct.UpdateDescriptorSets(_device, _descriptorWriteCount, _pDescriptorWrites, _descriptorCopyCount, _pDescriptorCopies);
            }
        }

        public VkObjectResult<IVkQueryPool> CreateQueryPool(VkQueryPoolCreateInfo createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkQueryPool.HandleType _pQueryPool;
                var result = Direct.CreateQueryPool(_device, _pCreateInfo, _pAllocator, &_pQueryPool);
                var instance = result == VkResult.Success ? new VkQueryPool(this, _pQueryPool, allocator) : null;
                return new VkObjectResult<IVkQueryPool>(result, instance);
            }
        }

        public IReadOnlyList<VkSparseImageMemoryRequirements> GetImageSparseMemoryRequirements(IVkImage image)
        {
            var _device = Handle;
            var _image = image?.Handle ?? VkImage.HandleType.Null;
            var _pSparseMemoryRequirementCount = (int)0;
            Direct.GetImageSparseMemoryRequirements(_device, _image, &_pSparseMemoryRequirementCount, (VkSparseImageMemoryRequirements*)0);
            var resultArray = new VkSparseImageMemoryRequirements[(int)_pSparseMemoryRequirementCount];
            fixed (VkSparseImageMemoryRequirements* pResultArray = resultArray)
            {
                Direct.GetImageSparseMemoryRequirements(_device, _image, &_pSparseMemoryRequirementCount, (VkSparseImageMemoryRequirements*)pResultArray);
                return resultArray;
            }
        }

        public VkObjectResult<IVkSwapchainKHR> CreateSwapchainKHR(VkSwapchainCreateInfoKHR createInfo, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                VkSwapchainKHR.HandleType _pSwapchain;
                var result = Direct.CreateSwapchainKHR(_device, _pCreateInfo, _pAllocator, &_pSwapchain);
                var instance = result == VkResult.Success ? new VkSwapchainKHR(this, _pSwapchain, allocator) : null;
                return new VkObjectResult<IVkSwapchainKHR>(result, instance);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkSwapchainKHR>> CreateSharedSwapchainsKHR(IReadOnlyList<VkSwapchainCreateInfoKHR> createInfos, VkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfos.SizeOfMarshalDirect() +
                allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _swapchainCount = createInfos?.Count ?? 0;
                var _pCreateInfos = createInfos.MarshalDirect(ref unmanaged);
                var _pAllocator = allocator.MarshalIndirect(ref unmanaged);
                var handleArray = new VkSwapchainKHR.HandleType[createInfos?.Count ?? 0];
                fixed (VkSwapchainKHR.HandleType* _pSwapchains = handleArray)
                {
                    var result = Direct.CreateSharedSwapchainsKHR(_device, _swapchainCount, _pCreateInfos, _pAllocator, _pSwapchains);
                    var instance = result == VkResult.Success ? Enumerable.Range(0, handleArray.Length).Select(i => (IVkSwapchainKHR)new VkSwapchainKHR(this, handleArray[i], allocator)).ToArray() : null;
                    return new VkObjectResult<IReadOnlyList<IVkSwapchainKHR>>(result, instance);
                }
            }
        }

        public VkResult DebugMarkerSetObjectNameEXT(VkDebugMarkerObjectNameInfoEXT nameInfo)
        {
            var unmanagedSize =
                nameInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pNameInfo = nameInfo.MarshalIndirect(ref unmanaged);
                return Direct.DebugMarkerSetObjectNameEXT(_device, _pNameInfo);
            }
        }

        public VkResult DebugMarkerSetObjectTagEXT(VkDebugMarkerObjectTagInfoEXT tagInfo)
        {
            var unmanagedSize =
                tagInfo.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Handle;
                var _pTagInfo = tagInfo.MarshalIndirect(ref unmanaged);
                return Direct.DebugMarkerSetObjectTagEXT(_device, _pTagInfo);
            }
        }

    }

    public static unsafe class VkDeviceExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDevice> list) =>
            list.SizeOfMarshalDirectDispatchable();

        public static VkDevice.HandleType* MarshalDirect(this IReadOnlyList<IVkDevice> list, ref byte* unmanaged) =>
            (VkDevice.HandleType*)list.MarshalDirectDispatchable(ref unmanaged);
    }
}
