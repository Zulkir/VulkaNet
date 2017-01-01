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

namespace VulkaNet
{
    public interface IVkDescriptorPool : IVkNonDispatchableHandledObject, IVkDeviceChild, IDisposable
    {
        VkDescriptorPool.HandleType Handle { get; }
        IVkAllocationCallbacks Allocator { get; }
        VkResult Reset(VkDescriptorPoolResetFlags flags);
    }

    public unsafe class VkDescriptorPool : IVkDescriptorPool
    {
        public IVkDevice Device { get; }
        public HandleType Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }

        private VkDevice.DirectFunctions Direct => Device.Direct;

        public ulong RawHandle => Handle.InternalHandle;

        public VkDescriptorPool(IVkDevice device, HandleType handle, IVkAllocationCallbacks allocator)
        {
            Device = device;
            Handle = handle;
            Allocator = allocator;
        }

        public struct HandleType
        {
            public readonly ulong InternalHandle;
            public HandleType(ulong internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = sizeof(ulong);
            public static HandleType Null => new HandleType(default(ulong));
        }

        public void Dispose()
        {
            var unmanagedSize =
                Allocator.SizeOfMarshalIndirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _device = Device.Handle;
                var _descriptorPool = Handle;
                var _pAllocator = Allocator.MarshalIndirect(ref unmanaged);
                Direct.DestroyDescriptorPool(_device, _descriptorPool, _pAllocator);
            }
        }

        public VkResult Reset(VkDescriptorPoolResetFlags flags)
        {
            var _device = Device.Handle;
            var _descriptorPool = Handle;
            var _flags = flags;
            return Direct.ResetDescriptorPool(_device, _descriptorPool, _flags);
        }

    }

    public static unsafe class VkDescriptorPoolExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDescriptorPool> list) =>
            list.SizeOfMarshalDirectNonDispatchable();

        public static VkDescriptorPool.HandleType* MarshalDirect(this IReadOnlyList<IVkDescriptorPool> list, ref byte* unmanaged) =>
            (VkDescriptorPool.HandleType*)list.MarshalDirectNonDispatchable(ref unmanaged);
    }
}