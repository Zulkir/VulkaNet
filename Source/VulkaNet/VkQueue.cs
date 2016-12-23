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
    public interface IVkQueue : IVkHandledObject, IVkDeviceChild
    {
        VkQueue.HandleType Handle { get; }
        VkResult QueueSubmitDelegate(IReadOnlyList<IVkSubmitInfo> submits, IVkFence fence);
    }

    public unsafe class VkQueue : IVkQueue
    {
        public IVkDevice Device { get; }
        public HandleType Handle { get; }

        private VkDevice.DirectFunctions Direct => Device.Direct;

        public IntPtr RawHandle => Handle.InternalHandle;

        public VkQueue(IVkDevice device, HandleType handle)
        {
            Device = device;
            Handle = handle;
        }

        public struct HandleType
        {
            public readonly IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = IntPtr.Size;
            public static HandleType Null => new HandleType(default(IntPtr));
        }

        public VkResult QueueSubmitDelegate(IReadOnlyList<IVkSubmitInfo> submits, IVkFence fence)
        {
            var unmanagedSize =
                submits.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var _queue = Handle;
                var _submitCount = submits?.Count ?? 0;
                var _pSubmits = submits.MarshalDirect(ref unmanaged);
                var _fence = fence?.Handle ?? VkFence.HandleType.Null;
                return Direct.QueueSubmitDelegate(_queue, _submitCount, _pSubmits, _fence);
            }
        }

    }

    public static unsafe class VkQueueExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkQueue> list) =>
            list.SizeOfMarshalDirectDispatchable();

        public static VkQueue.HandleType* MarshalDirect(this IReadOnlyList<IVkQueue> list, ref byte* unmanaged) =>
            (VkQueue.HandleType*)list.MarshalDirectDispatchable(ref unmanaged);
    }
}
