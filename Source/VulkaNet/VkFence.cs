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
    public interface IVkFence : IVkNonDisptatchableHandledObject, IVkDeviceChild, IDisposable
    {
        VkFence.HandleType Handle { get; }
        VkResult GetStatus();
    }

    public unsafe class VkFence : IVkFence
    {
        public HandleType Handle { get; }
        public IVkDevice Device { get; }
        public IVkAllocationCallbacks Allocator { get; }
        public DirectFunctions Direct { get; }

        public ulong RawHandle => Handle.InternalHandle;

        public VkFence(HandleType handle, IVkDevice device, IVkAllocationCallbacks allocator)
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
            public static int SizeInBytes { get; } = sizeof(ulong);
        }

        public class DirectFunctions
        {
            public DestroyFenceDelegate DestroyFence { get; }
            public delegate void DestroyFenceDelegate(
                VkDevice.HandleType device,
                HandleType fence,
                VkAllocationCallbacks.Raw* pAllocator);

            public GetFenceStatusDelegate GetFenceStatus { get; }
            public delegate VkResult GetFenceStatusDelegate(
                VkDevice.HandleType device,
                HandleType fence);

            public DirectFunctions(IVkDevice device)
            {
                DestroyFence = device.GetDeviceDelegate<DestroyFenceDelegate>("vkDestroyFence");
                GetFenceStatus = device.GetDeviceDelegate<GetFenceStatusDelegate>("vkGetFenceStatus");
            }
        }

        public void Dispose()
        {
            var unmanagedSize = Allocator.SafeMarshalSize();
            var unamangedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unamangedArray)
            {
                var unamanged = unmanagedStart;
                var pAllocator = Allocator.SafeMarshalTo(ref unamanged);
                Direct.DestroyFence(Device.Handle, Handle, pAllocator);
            }
        }

        public VkResult GetStatus()
        {
            return Direct.GetFenceStatus(Device.Handle, Handle);
        }
    }

    public static unsafe class VkFenceExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkFence> list) =>
            list.SizeOfMarshalDirect(0f);

        public static VkFence.HandleType* MarshalDirect(this IReadOnlyList<IVkFence> list, ref byte* unmanaged) =>
            (VkFence.HandleType*)list.MarshalDirect(ref unmanaged, 0f);
    }
}