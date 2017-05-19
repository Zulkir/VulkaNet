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
    public interface IVkSwapchainKHR : IVkNonDispatchableHandledObject, IVkDeviceChild, IDisposable
    {
        VkSwapchainKHR.HandleType Handle { get; }
        IVkAllocationCallbacks Allocator { get; }
        VkObjectResult<IReadOnlyList<IVkImage>> GetImagesKHR();
        VkObjectResult<int> AcquireNextImageKHR(ulong timeout, IVkSemaphore semaphore, IVkFence fence);
    }

    public unsafe class VkSwapchainKHR : IVkSwapchainKHR
    {
        public IVkDevice Device { get; }
        public HandleType Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }

        private VkDevice.DirectFunctions Direct => Device.Direct;

        public ulong RawHandle => Handle.InternalHandle;

        public VkSwapchainKHR(IVkDevice device, HandleType handle, IVkAllocationCallbacks allocator)
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
                var _swapchain = Handle;
                var _pAllocator = Allocator.MarshalIndirect(ref unmanaged);
                Direct.DestroySwapchainKHR(_device, _swapchain, _pAllocator);
            }
        }

        public VkObjectResult<IReadOnlyList<IVkImage>> GetImagesKHR()
        {
            var _device = Device.Handle;
            var _swapchain = Handle;
            var _pSwapchainImageCount = 0;
            Direct.GetSwapchainImagesKHR(_device, _swapchain, &_pSwapchainImageCount, (VkImage.HandleType*)0);
            var resultArray = new VkImage.HandleType[_pSwapchainImageCount];
            fixed (VkImage.HandleType* pResultArray = resultArray)
            {
                var pResultArrayLoc = pResultArray;
                var result = Direct.GetSwapchainImagesKHR(_device, _swapchain, &_pSwapchainImageCount, pResultArray);
                var resultObjArray = Enumerable.Range(0, _pSwapchainImageCount).Select((x, i) => new VkImage(Device, pResultArrayLoc[i], null)).ToArray();
                return new VkObjectResult<IReadOnlyList<IVkImage>>(result, resultObjArray);
            }
        }

        public VkObjectResult<int> AcquireNextImageKHR(ulong timeout, IVkSemaphore semaphore, IVkFence fence)
        {
            var _device = Device.Handle;
            var _swapchain = Handle;
            var _timeout = timeout;
            var _semaphore = semaphore?.Handle ?? VkSemaphore.HandleType.Null;
            var _fence = fence?.Handle ?? VkFence.HandleType.Null;
            int _pImageIndex;
            var result = Direct.AcquireNextImageKHR(_device, _swapchain, _timeout, _semaphore, _fence, &_pImageIndex);
            return new VkObjectResult<int>(result, _pImageIndex);
        }
    }

    public static unsafe class VkSwapchainKHRExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkSwapchainKHR> list) =>
            list.SizeOfMarshalDirectNonDispatchable();

        public static VkSwapchainKHR.HandleType* MarshalDirect(this IReadOnlyList<IVkSwapchainKHR> list, ref byte* unmanaged) =>
            (VkSwapchainKHR.HandleType*)list.MarshalDirectNonDispatchable(ref unmanaged);
    }
}
