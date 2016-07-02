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
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkDevice : IVkInstanceChild, IDisposable
    {
        VkResult WaitIdle();
    }

    public unsafe class VkDevice : IVkDevice
    {
        public IntPtr Handle { get; }
        public IVkAllocationCallbacks Allocator { get; }
        public IVkInstance Instance { get; }
        public DirectFunctions Direct { get; }

        public VkDevice(IntPtr handle, IVkAllocationCallbacks allocator, IVkPhysicalDevice physicalDevice)
        {
            Handle = handle;
            Allocator = allocator;
            Instance = physicalDevice.Instance;
            Direct = new DirectFunctions(Instance);
        }

        public class DirectFunctions
        {
            public DestroyDeviceDelegate DestroyDevice { get; }
            public delegate void DestroyDeviceDelegate(
                IntPtr device,
                VkAllocationCallbacks.Raw* pAllocator);

            public DeviceWaitIdleDelegate DeviceWaitIdle { get; }
            public delegate VkResult DeviceWaitIdleDelegate(
                IntPtr device);

            public DirectFunctions(IVkInstance instance)
            {
                DeviceWaitIdle =
                    VkHelpers.GetDelegate<DeviceWaitIdleDelegate>(instance, "vkDeviceWaitIdle");
                DestroyDevice = 
                    VkHelpers.GetDelegate<DestroyDeviceDelegate>(instance, "vkDestroyDevice");
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

        public VkResult WaitIdle() 
            => Direct.DeviceWaitIdle(Handle);
    }
}