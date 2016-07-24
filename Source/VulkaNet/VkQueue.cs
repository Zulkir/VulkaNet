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
    public interface IVkQueue : IVkDeviceChild
    {
        VkResult Submit(IReadOnlyList<IVkSubmitInfo> submits, IVkFence fence);
    }

    public unsafe class VkQueue : IVkQueue
    {
        public HandleType Handle { get; }
        public IVkDevice Device { get; }
        public DirectFunctions Direct { get; }

        public VkQueue(HandleType handle, IVkDevice device)
        {
            Handle = handle;
            Device = device;
            Direct = new DirectFunctions(device);
        }

        public struct HandleType
        {
            public readonly IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = sizeof(IntPtr);
        }

        public class DirectFunctions
        {
            public QueueSubmitDelegate QueueSubmit { get; }
            public delegate VkResult QueueSubmitDelegate(
                HandleType queue,
                int submitCount,
                VkSubmitInfo.Raw* pSubmits,
                VkFence.HandleType fence);
            
            public DirectFunctions(IVkDevice device)
            {
                QueueSubmit = device.GetDeviceDelegate<QueueSubmitDelegate>("vkQueueSubmit");
            }
        }

        public VkResult Submit(IReadOnlyList<IVkSubmitInfo> submits, IVkFence fence)
        {
            var unmanagedSize =
                submits.SizeOfMarshalDirect();
            var unmanagedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unmanagedArray)
            {
                var unmanaged = unmanagedStart;
                var pSubmits = submits.MarshalDirect(ref unmanaged);
                return Direct.QueueSubmit(Handle, submits?.Count ?? 0, pSubmits, fence.Handle);
            }
        }
    }
}