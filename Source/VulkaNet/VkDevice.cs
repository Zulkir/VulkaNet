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

            public QueueSubmitDelegateDelegate QueueSubmitDelegate { get; }
            public delegate VkResult QueueSubmitDelegateDelegate(
                VkQueue.HandleType queue,
                int submitCount,
                VkSubmitInfo.Raw* pSubmits,
                VkFence.HandleType fence);

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

            public DirectFunctions(IVkDevice device)
            {
                this.device = device;

                GetDeviceProcAddr = VkHelpers.GetInstanceDelegate<GetDeviceProcAddrDelegate>(device.Instance, "vkGetDeviceProcAddr");
                GetDeviceQueue = GetDeviceDelegate<GetDeviceQueueDelegate>("vkGetDeviceQueue");
                QueueSubmitDelegate = GetDeviceDelegate<QueueSubmitDelegateDelegate>("vkQueueSubmitDelegate");
                DestroyCommandPool = GetDeviceDelegate<DestroyCommandPoolDelegate>("vkDestroyCommandPool");
                ResetCommandPool = GetDeviceDelegate<ResetCommandPoolDelegate>("vkResetCommandPool");
                FreeCommandBuffers = GetDeviceDelegate<FreeCommandBuffersDelegate>("vkFreeCommandBuffers");
                ResetCommandBuffer = GetDeviceDelegate<ResetCommandBufferDelegate>("vkResetCommandBuffer");
                BeginCommandBuffer = GetDeviceDelegate<BeginCommandBufferDelegate>("vkBeginCommandBuffer");
                EndCommandBuffer = GetDeviceDelegate<EndCommandBufferDelegate>("vkEndCommandBuffer");
                CmdExecuteCommands = GetDeviceDelegate<CmdExecuteCommandsDelegate>("vkCmdExecuteCommands");
                DestroyFence = GetDeviceDelegate<DestroyFenceDelegate>("vkDestroyFence");
                GetFenceStatus = GetDeviceDelegate<GetFenceStatusDelegate>("vkGetFenceStatus");
                DestroySemaphore = GetDeviceDelegate<DestroySemaphoreDelegate>("vkDestroySemaphore");
                DestroyEvent = GetDeviceDelegate<DestroyEventDelegate>("vkDestroyEvent");
                DestroyDevice = GetDeviceDelegate<DestroyDeviceDelegate>("vkDestroyDevice");
                DeviceWaitIdle = GetDeviceDelegate<DeviceWaitIdleDelegate>("vkDeviceWaitIdle");
                CreateCommandPool = GetDeviceDelegate<CreateCommandPoolDelegate>("vkCreateCommandPool");
                AllocateCommandBuffers = GetDeviceDelegate<AllocateCommandBuffersDelegate>("vkAllocateCommandBuffers");
                CreateFence = GetDeviceDelegate<CreateFenceDelegate>("vkCreateFence");
                ResetFences = GetDeviceDelegate<ResetFencesDelegate>("vkResetFences");
                WaitForFences = GetDeviceDelegate<WaitForFencesDelegate>("vkWaitForFences");
                CreateSemaphore = GetDeviceDelegate<CreateSemaphoreDelegate>("vkCreateSemaphore");
                CreateEvent = GetDeviceDelegate<CreateEventDelegate>("vkCreateEvent");
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

    }

    public static unsafe class VkDeviceExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDevice> list) =>
            list.SizeOfMarshalDirectDispatchable();

        public static VkDevice.HandleType* MarshalDirect(this IReadOnlyList<IVkDevice> list, ref byte* unmanaged) =>
            (VkDevice.HandleType*)list.MarshalDirectDispatchable(ref unmanaged);
    }
}
