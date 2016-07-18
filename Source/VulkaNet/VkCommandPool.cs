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

namespace VulkaNet
{
    public interface IVkCommandPool : IVkDeviceChild, IDisposable
    {
        VkCommandPool.HandleType Handle { get; }
        IVkAllocationCallbacks Allocator { get; }
        VkCommandPool.DirectFunctions Direct { get; }

        VkResult Reset(VkCommandPoolResetFlags flags);
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
            private readonly IVkDevice device;

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

            public DirectFunctions(IVkDevice device)
            {
                this.device = device;

                ResetCommandPool = device.GetDeviceDelegate<ResetCommandPoolDelegate>("vkResetCommandPool");
                DestroyCommandPool = device.GetDeviceDelegate<DestroyCommandPoolDelegate>("vkDestroyCommandPool");
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
    }
}