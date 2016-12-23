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
using System.Runtime.InteropServices;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkDevice : IVkInstanceChild, IDisposable
    {
        IVkPhysicalDevice PhysicalDevice { get; }
        VkDevice.HandleType Handle { get; }
        TDelegate GetDeviceDelegate<TDelegate>(string name);
        VkResult WaitIdle();
        IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex);
        VkObjectResult<IVkCommandPool> CreateCommandPool(IVkCommandPoolCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkObjectResult<IVkFence> CreateFence(IVkFenceCreateInfo createInfo, IVkAllocationCallbacks allocator);
        VkResult ResetFences(IReadOnlyList<IVkFence> fences);
        VkResult WaitForFences(IReadOnlyList<IVkFence> fences, bool waitAll, ulong timeout);
    }

    public unsafe class VkDevice : IVkDevice
    {
        public IVkPhysicalDevice PhysicalDevice { get; }
        public HandleType Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }
        public IVkInstance Instance { get; }
        public DirectFunctions Direct { get; }

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
            public IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
        }

        public class DirectFunctions
        {
            private readonly IVkDevice device;

            public GetDeviceProcAddrDelegate GetDeviceProcAddr { get; }
            public delegate IntPtr GetDeviceProcAddrDelegate(
                HandleType device,
                byte* pName);

            public DestroyDeviceDelegate DestroyDevice { get; }
            public delegate void DestroyDeviceDelegate(
                HandleType device,
                VkAllocationCallbacks.Raw* pAllocator);

            public DeviceWaitIdleDelegate DeviceWaitIdle { get; }
            public delegate VkResult DeviceWaitIdleDelegate(
                HandleType device);

            public GetDeviceQueueDelegate GetDeviceQueue { get; }
            public delegate VkResult GetDeviceQueueDelegate(
                HandleType device,
                uint queueFamilyIndex,
                uint queueIndex,
                VkQueue.HandleType* pQueue);

            public CreateCommandPoolDelegate CreateCommandPool { get; }
            public delegate VkResult CreateCommandPoolDelegate(
                HandleType device,
                VkCommandPoolCreateInfo.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkCommandPool.HandleType* pCommandPool);

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

            public DirectFunctions(IVkDevice device)
            {
                this.device = device;

                GetDeviceProcAddr = VkHelpers.GetInstanceDelegate<GetDeviceProcAddrDelegate>(device.Instance, "vkGetDeviceProcAddr");
                DeviceWaitIdle = GetDeviceDelegate<DeviceWaitIdleDelegate>("vkDeviceWaitIdle");
                DestroyDevice = GetDeviceDelegate<DestroyDeviceDelegate>("vkDestroyDevice");
                GetDeviceQueue = GetDeviceDelegate<GetDeviceQueueDelegate>("vkGetDeviceQueue");
                CreateCommandPool = GetDeviceDelegate<CreateCommandPoolDelegate>("vkCreateCommandPool");
                CreateFence = GetDeviceDelegate<CreateFenceDelegate>("vkCreateFence");
                ResetFences = GetDeviceDelegate<ResetFencesDelegate>("vkResetFences");
                WaitForFences = GetDeviceDelegate<WaitForFencesDelegate>("vkWaitForFences");
            }

            public TDelegate GetDeviceDelegate<TDelegate>(string name)
            {
                IntPtr funPtr;
                fixed (byte* pName = name.ToAnsiArray())
                    funPtr = GetDeviceProcAddr(device.Handle, pName);
                return Marshal.GetDelegateForFunctionPointer<TDelegate>(funPtr);
            }
        }

        public void Dispose()
        {
            var size = Allocator.SafeMarshalSize();
            VkHelpers.RunWithUnamangedData(size, DisposeInternal);
        }

        private void DisposeInternal(IntPtr data)
        {
            var unmanaged = (byte*)data;
            var pAllocator = Allocator.SafeMarshalTo(ref unmanaged);
            Direct.DestroyDevice(Handle, pAllocator);
        }

        public TDelegate GetDeviceDelegate<TDelegate>(string name) =>
            Direct.GetDeviceDelegate<TDelegate>(name);

        public VkResult WaitIdle() => 
            Direct.DeviceWaitIdle(Handle);

        public IVkQueue GetDeviceQueue(int queueFamilyIndex, int queueIndex) => 
            queues.GetOrAdd(new ValuePair<int, int>(queueFamilyIndex, queueIndex), DoGetDeviceQueue);

        private IVkQueue DoGetDeviceQueue(ValuePair<int, int> key)
        {
            VkQueue.HandleType handle;
            Direct.GetDeviceQueue(Handle, (uint)key.First, (uint)key.Second, &handle).CheckSuccess();
            return new VkQueue(handle, this);
        }

        public VkObjectResult<IVkCommandPool> CreateCommandPool(IVkCommandPoolCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanagedSize = 
                createInfo.SizeOfMarshalIndirect() + 
                allocator.SafeMarshalSize();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var pAllocator = allocator.SafeMarshalTo(ref unmanaged);
                VkCommandPool.HandleType handle;
                var result = Direct.CreateCommandPool(Handle, pCreateInfo, pAllocator, &handle);
                var instance = result == VkResult.Success ? new VkCommandPool(handle, this, allocator) : null;
                return new VkObjectResult<IVkCommandPool>(result, instance);
            }
        }

        public VkObjectResult<IVkFence> CreateFence(IVkFenceCreateInfo createInfo, IVkAllocationCallbacks allocator)
        {
            var unmanagedSize =
                createInfo.SizeOfMarshalIndirect() +
                allocator.SafeMarshalSize();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var pCreateInfo = createInfo.MarshalIndirect(ref unmanaged);
                var pAllocator = allocator.SafeMarshalTo(ref unmanaged);
                VkFence.HandleType handle;
                var result = Direct.CreateFence(Handle, pCreateInfo, pAllocator, &handle);
                var instance = result == VkResult.Success ? new VkFence(handle, this, allocator) : null;
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
                var pFences = fences.MarshalDirect(ref unmanaged);
                return Direct.ResetFences(Handle, fences.Count, pFences);
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
                var pFences = fences.MarshalDirect(ref unmanaged);
                return Direct.WaitForFences(Handle, fences.Count, pFences, new VkBool32(waitAll), timeout);
            }
        }
    }
}