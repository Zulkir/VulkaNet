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
    public interface IVkBuffer : IVkNonDispatchableHandledObject, IVkDeviceChild, IDisposable
    {
        VkBuffer.HandleType Handle { get; }
        IVkAllocationCallbacks Allocator { get; }
        VkMemoryRequirements GetMemoryRequirements();
        VkResult BindMemory(IVkDeviceMemory memory, ulong memoryOffset);
    }

    public unsafe class VkBuffer : IVkBuffer
    {
        public IVkDevice Device { get; }
        public HandleType Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }

        private VkDevice.DirectFunctions Direct => Device.Direct;

        public ulong RawHandle => Handle.InternalHandle;

        public VkBuffer(IVkDevice device, HandleType handle, IVkAllocationCallbacks allocator)
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
                var _buffer = Handle;
                var _pAllocator = Allocator.MarshalIndirect(ref unmanaged);
                Direct.DestroyBuffer(_device, _buffer, _pAllocator);
            }
        }

        public VkMemoryRequirements GetMemoryRequirements()
        {
            var _device = Device.Handle;
            var _buffer = Handle;
            VkMemoryRequirements _pMemoryRequirements;
            Direct.GetBufferMemoryRequirements(_device, _buffer, &_pMemoryRequirements);
            return _pMemoryRequirements;
        }

        public VkResult BindMemory(IVkDeviceMemory memory, ulong memoryOffset)
        {
            var _device = Device.Handle;
            var _buffer = Handle;
            var _memory = memory?.Handle ?? VkDeviceMemory.HandleType.Null;
            var _memoryOffset = memoryOffset;
            return Direct.BindBufferMemory(_device, _buffer, _memory, _memoryOffset);
        }

    }

    public static unsafe class VkBufferExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkBuffer> list) =>
            list.SizeOfMarshalDirectNonDispatchable();

        public static VkBuffer.HandleType* MarshalDirect(this IReadOnlyList<IVkBuffer> list, ref byte* unmanaged) =>
            (VkBuffer.HandleType*)list.MarshalDirectNonDispatchable(ref unmanaged);
    }
}
