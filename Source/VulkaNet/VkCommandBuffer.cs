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
    public interface IVkCommandBuffer : IVkHandledObject, IVkDeviceChild
    {
        VkCommandBuffer.HandleType Handle { get; }
        VkResult Begin(IVkCommandBufferBeginInfo beginInfo);
        VkResult End();
    }

    public unsafe class VkCommandBuffer : IVkCommandBuffer
    {
        public HandleType Handle { get; }
        public IVkDevice Device { get; }
        public DirectFunctions Direct { get; }

        public IntPtr RawHandle => Handle.InternalHandle;

        public VkCommandBuffer(HandleType handle, IVkDevice device)
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
            public static int SizeInBytes { get; } = IntPtr.Size;
        }

        public class DirectFunctions
        {
            public BeginCommandBufferDelegate BeginCommandBuffer { get; }
            public delegate VkResult BeginCommandBufferDelegate(
                HandleType commandBuffer,
                VkCommandBufferBeginInfo.Raw* pBeginInfo);

            public EndCommandBufferDelegate EndCommandBuffer { get; }
            public delegate VkResult EndCommandBufferDelegate(
                HandleType commandBuffer);

            public DirectFunctions(IVkDevice device)
            {
                BeginCommandBuffer = device.GetDeviceDelegate<BeginCommandBufferDelegate>("vkBeginCommandBuffer");
                EndCommandBuffer = device.GetDeviceDelegate<EndCommandBufferDelegate>("vkEndCommandBuffer");
            }
        }

        public VkResult Begin(IVkCommandBufferBeginInfo beginInfo)
        {
            var unmanagedSize = beginInfo.SizeOfMarshalIndirect();
            var unamangedArray = new byte[unmanagedSize];
            fixed (byte* unmanagedStart = unamangedArray)
            {
                var unamanged = unmanagedStart;
                var pBeginInfo = beginInfo.MarshalIndirect(ref unamanged);
                return Direct.BeginCommandBuffer(Handle, pBeginInfo);
            }
        }

        public VkResult End() => 
            Direct.EndCommandBuffer(Handle);
    }

    public static unsafe class VkCommandBufferExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkCommandBuffer> list) =>
            list.SizeOfMarshalDirect(0);

        public static VkCommandBuffer.HandleType* MarshalDirect(this IReadOnlyList<IVkCommandBuffer> list, ref byte* unmanaged) =>
            (VkCommandBuffer.HandleType*)list.MarshalDirect(ref unmanaged, 0);
    }
}