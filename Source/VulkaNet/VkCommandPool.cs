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
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public interface IVkCommandPool : IVkDeviceChild, IDisposable
    {
        VkCommandPool.HandleType Handle { get; }
        IVkAllocationCallbacks Allocator { get; }
        VkCommandPool.DirectFunctions Direct { get; }

        VkResult Reset(VkCommandPoolResetFlags flags);
        VkObjectResult<IVkCommandBuffer[]> AllocateCommandBuffers(IVkCommandBufferAllocateInfo allocateInfo);
        void FreeCommandBuffers(IReadOnlyList<IVkCommandBuffer> commandBuffers);
    }

    public unsafe class VkCommandPool : IVkCommandPool
    {
        public HandleType Handle { get; }
        public IVkDevice Device { get; }
        public IVkAllocationCallbacks Allocator { get; }
        public DirectFunctions Direct { get; }
        
        public VkCommandPool(HandleType handle, IVkDevice device, IVkAllocationCallbacks allocator)
        {
            Handle = handle;
            Device = device;
            Allocator = allocator;
            Direct = new DirectFunctions(device);
        }

        public struct HandleType
        {
            public readonly ulong InternalHandle;
            public HandleType(ulong internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
        }

        public class DirectFunctions
        {
            public ResetCommandPoolDelegate ResetCommandPool { get; }
            public delegate VkResult ResetCommandPoolDelegate(
                VkDevice.HandleType device,
                HandleType commandPool,
                VkCommandPoolResetFlags flags);

            public DestroyCommandPoolDelegate DestroyCommandPool { get; }
            public delegate void DestroyCommandPoolDelegate(
                VkDevice.HandleType device,
                HandleType commandPool,
                VkAllocationCallbacks.Raw* pAllocator);

            public AllocateCommandBuffersDelegate AllocateCommandBuffers { get; }
            public delegate VkResult AllocateCommandBuffersDelegate(
                VkDevice.HandleType device,
                VkCommandBufferAllocateInfo.Raw* pAllocateInfo,
                VkCommandBuffer.HandleType* pCommandBuffers);

            public FreeCommandBuffersDelegate FreeCommandBuffers { get; }
            public delegate void FreeCommandBuffersDelegate(
                VkDevice.HandleType device,
                HandleType commandPool,
                int commandBufferCount,
                VkCommandBuffer.HandleType* pCommandBuffers);

            public DirectFunctions(IVkDevice device)
            {
                ResetCommandPool = device.GetDeviceDelegate<ResetCommandPoolDelegate>("vkResetCommandPool");
                DestroyCommandPool = device.GetDeviceDelegate<DestroyCommandPoolDelegate>("vkDestroyCommandPool");
                AllocateCommandBuffers = device.GetDeviceDelegate<AllocateCommandBuffersDelegate>("vkAllocateCommandBuffers");
                FreeCommandBuffers = device.GetDeviceDelegate<FreeCommandBuffersDelegate>("vkFreeCommandBuffers");
            }
        }

        public VkResult Reset(VkCommandPoolResetFlags flags) => 
            Direct.ResetCommandPool(Device.Handle, Handle, flags);

        public void Dispose()
        {
            var unmanagedSize = Allocator.SafeMarshalSize();
            var unamangedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unamangedArray)
            {
                var unamanged = unmanagedStart;
                var pAllocator = Allocator.SafeMarshalTo(ref unamanged);
                Direct.DestroyCommandPool(Device.Handle, Handle, pAllocator);
            }
        }

        public VkObjectResult<IVkCommandBuffer[]> AllocateCommandBuffers(IVkCommandBufferAllocateInfo allocateInfo)
        {
            var unmanagedSize = allocateInfo.SafeMarshalSize();
            var unamangedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unamangedArray)
            {
                var unamanged = unmanagedStart;
                var pAllocateInfo = allocateInfo.SafeMarshalTo(ref unamanged, this);
                var commandBufferHandles = new VkCommandBuffer.HandleType[allocateInfo.CommandBufferCount];
                fixed (VkCommandBuffer.HandleType* pCommandBuffers = commandBufferHandles)
                {
                    var result = Direct.AllocateCommandBuffers(Device.Handle, pAllocateInfo, pCommandBuffers);
                    if (result != VkResult.Success)
                        return new VkObjectResult<IVkCommandBuffer[]>(result, null);
                    var commandBuffers = commandBufferHandles.Select(x => (IVkCommandBuffer)new VkCommandBuffer(x, Device)).ToArray();
                    return new VkObjectResult<IVkCommandBuffer[]>(result, commandBuffers);
                }
            }
        }

        public void FreeCommandBuffers(IReadOnlyList<IVkCommandBuffer> commandBuffers)
        {
            var commandBufferHandles = commandBuffers.Select(x => x.Handle).ToArray();
            fixed (VkCommandBuffer.HandleType* pCommandBuffers = commandBufferHandles)
                Direct.FreeCommandBuffers(Device.Handle, Handle, commandBuffers.Count, pCommandBuffers);
        }
    }
}