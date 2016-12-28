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
        VkObjectResult<IVkCommandPool> CreateCommandPool(IVkCommandPoolCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IReadOnlyList<IVkCommandBuffer>> AllocateCommandBuffers(IVkCommandBufferAllocateInfo allocateInfo);
        VkObjectResult<IVkFence> CreateFence(IVkFenceCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkResult ResetFences(IReadOnlyList<IVkFence> fences);
        VkResult WaitForFences(IReadOnlyList<IVkFence> fences, bool waitAll, ulong timeout);
        VkObjectResult<IVkSemaphore> CreateSemaphore(IVkSemaphoreCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkEvent> CreateEvent(IVkEventCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkRenderPass> CreateRenderPass(IVkRenderPassCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkFramebuffer> CreateFramebuffer(IVkFramebufferCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkShaderModule> CreateShaderModule(IVkShaderModuleCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkPipeline> CreateComputePipelines(IVkPipelineCache pipelineCache, IReadOnlyList<IVkComputePipelineCreateInfo> createInfos, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkPipeline> CreateGraphicsPipelines(IVkPipelineCache pipelineCache, IReadOnlyList<IVkGraphicsPipelineCreateInfo> createInfos, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkPipelineCache> CreatePipelineCache(IVkPipelineCacheCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkDeviceMemory> AllocateMemory(IVkMemoryAllocateInfo allocateInfo, IVkAllocationCallbacks allocator);
        VkResult FlushMappedMemoryRanges(IReadOnlyList<IVkMappedMemoryRange> memoryRanges);
        VkResult InvalidateMappedMemoryRanges(IReadOnlyList<IVkMappedMemoryRange> memoryRanges);
        VkObjectResult<IVkBuffer> CreateBuffer(IVkBufferCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkBufferView> CreateBufferView(IVkBufferViewCreateInfo createInfo, IVkAllocationCallbacks allocator);
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
            public delegate VkResult GetDeviceQueueDelegate(
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

            public CmdNextSubpassDelegate CmdNextSubpass { get; }
            public delegate void CmdNextSubpassDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkSubpassContents contents);

            public CmdBindPipelineDelegate CmdBindPipeline { get; }
            public delegate void CmdBindPipelineDelegate(
                VkCommandBuffer.HandleType commandBuffer,
                VkPipelineBindPoint pipelineBindPoint,
                VkPipeline.HandleType pipeline);

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

            public DirectFunctions(IVkDevice device)
            {
                this.device = device;

                GetDeviceProcAddr = VkHelpers.GetInstanceDelegate<GetDeviceProcAddrDelegate>(device.Instance, "vkGetDeviceProcAddr");
                GetDeviceQueue = GetDeviceDelegate<GetDeviceQueueDelegate>("vkGetDeviceQueue");
                QueueSubmit = GetDeviceDelegate<QueueSubmitDelegate>("vkQueueSubmit");
                QueueWaitIdle = GetDeviceDelegate<QueueWaitIdleDelegate>("vkQueueWaitIdle");
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
                CmdNextSubpass = GetDeviceDelegate<CmdNextSubpassDelegate>("vkCmdNextSubpass");
                CmdBindPipeline = GetDeviceDelegate<CmdBindPipelineDelegate>("vkCmdBindPipeline");
                DestroyFence = GetDeviceDelegate<DestroyFenceDelegate>("vkDestroyFence");
                GetFenceStatus = GetDeviceDelegate<GetFenceStatusDelegate>("vkGetFenceStatus");
                DestroySemaphore = GetDeviceDelegate<DestroySemaphoreDelegate>("vkDestroySemaphore");
                DestroyEvent = GetDeviceDelegate<DestroyEventDelegate>("vkDestroyEvent");
                GetEventStatus = GetDeviceDelegate<GetEventStatusDelegate>("vkGetEventStatus");
                SetEvent = GetDeviceDelegate<SetEventDelegate>("vkSetEvent");
                ResetEvent = GetDeviceDelegate<ResetEventDelegate>("vkResetEvent");
                DestroyBuffer = GetDeviceDelegate<DestroyBufferDelegate>("vkDestroyBuffer");
                DestroyRenderPass = GetDeviceDelegate<DestroyRenderPassDelegate>("vkDestroyRenderPass");
                GetRenderAreaGranularity = GetDeviceDelegate<GetRenderAreaGranularityDelegate>("vkGetRenderAreaGranularity");
                DestroyFramebuffer = GetDeviceDelegate<DestroyFramebufferDelegate>("vkDestroyFramebuffer");
                DestroyShaderModule = GetDeviceDelegate<DestroyShaderModuleDelegate>("vkDestroyShaderModule");
                DestroyPipeline = GetDeviceDelegate<DestroyPipelineDelegate>("vkDestroyPipeline");
                DestroyPipelineCache = GetDeviceDelegate<DestroyPipelineCacheDelegate>("vkDestroyPipelineCache");
                MergePipelineCaches = GetDeviceDelegate<MergePipelineCachesDelegate>("vkMergePipelineCaches");
                GetPipelineCacheData = GetDeviceDelegate<GetPipelineCacheDataDelegate>("vkGetPipelineCacheData");
                FreeMemory = GetDeviceDelegate<FreeMemoryDelegate>("vkFreeMemory");
                MapMemory = GetDeviceDelegate<MapMemoryDelegate>("vkMapMemory");
                UnmapMemory = GetDeviceDelegate<UnmapMemoryDelegate>("vkUnmapMemory");
                GetDeviceMemoryCommitment = GetDeviceDelegate<GetDeviceMemoryCommitmentDelegate>("vkGetDeviceMemoryCommitment");
                DestroyBufferView = GetDeviceDelegate<DestroyBufferViewDelegate>("vkDestroyBufferView");
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
            Direct.GetDeviceQueue(Handle, (uint)key.First, (uint)key.Second, &handle).CheckSuccess();
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

        public VkObjectResult<IVkCommandPool> CreateCommandPool(IVkCommandPoolCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IReadOnlyList<IVkCommandBuffer>> AllocateCommandBuffers(IVkCommandBufferAllocateInfo allocateInfo)
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

        public VkObjectResult<IVkFence> CreateFence(IVkFenceCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkSemaphore> CreateSemaphore(IVkSemaphoreCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkEvent> CreateEvent(IVkEventCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkRenderPass> CreateRenderPass(IVkRenderPassCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkFramebuffer> CreateFramebuffer(IVkFramebufferCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkShaderModule> CreateShaderModule(IVkShaderModuleCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkPipeline> CreateComputePipelines(IVkPipelineCache pipelineCache, IReadOnlyList<IVkComputePipelineCreateInfo> createInfos, IVkAllocationCallbacks allocator)
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
                VkPipeline.HandleType _pPipelines;
                var result = Direct.CreateComputePipelines(_device, _pipelineCache, _createInfoCount, _pCreateInfos, _pAllocator, &_pPipelines);
                var instance = result == VkResult.Success ? new VkPipeline(this, _pPipelines, allocator) : null;
                return new VkObjectResult<IVkPipeline>(result, instance);
            }
        }

        public VkObjectResult<IVkPipeline> CreateGraphicsPipelines(IVkPipelineCache pipelineCache, IReadOnlyList<IVkGraphicsPipelineCreateInfo> createInfos, IVkAllocationCallbacks allocator)
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
                VkPipeline.HandleType _pPipelines;
                var result = Direct.CreateGraphicsPipelines(_device, _pipelineCache, _createInfoCount, _pCreateInfos, _pAllocator, &_pPipelines);
                var instance = result == VkResult.Success ? new VkPipeline(this, _pPipelines, allocator) : null;
                return new VkObjectResult<IVkPipeline>(result, instance);
            }
        }

        public VkObjectResult<IVkPipelineCache> CreatePipelineCache(IVkPipelineCacheCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkDeviceMemory> AllocateMemory(IVkMemoryAllocateInfo allocateInfo, IVkAllocationCallbacks allocator)
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

        public VkResult FlushMappedMemoryRanges(IReadOnlyList<IVkMappedMemoryRange> memoryRanges)
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

        public VkResult InvalidateMappedMemoryRanges(IReadOnlyList<IVkMappedMemoryRange> memoryRanges)
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

        public VkObjectResult<IVkBuffer> CreateBuffer(IVkBufferCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

        public VkObjectResult<IVkBufferView> CreateBufferView(IVkBufferViewCreateInfo createInfo, IVkAllocationCallbacks allocator)
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

    }

    public static unsafe class VkDeviceExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDevice> list) =>
            list.SizeOfMarshalDirectDispatchable();

        public static VkDevice.HandleType* MarshalDirect(this IReadOnlyList<IVkDevice> list, ref byte* unmanaged) =>
            (VkDevice.HandleType*)list.MarshalDirectDispatchable(ref unmanaged);
    }
}
