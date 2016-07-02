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
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkInstance : IVkHandledObject, IDisposable
    {
        VkInstance.DirectFunctions Direct { get; }
        IReadOnlyList<IVkPhysicalDevice> PhysicalDevices { get; }
    }

    public class VkInstance : IVkInstance
    {
        public IntPtr Handle { get; }
        private VkAllocationCallbacks Allocator { get; }
        public DirectFunctions Direct { get; }
        public IReadOnlyList<IVkPhysicalDevice> PhysicalDevices { get; }

        public VkInstance(IntPtr handle, VkAllocationCallbacks allocator)
        {
            Handle = handle;
            Allocator = allocator;
            Direct = new DirectFunctions(this);
            PhysicalDevices = EnumeratePhysicalDevices();
        }

        public unsafe class DirectFunctions
        {
            public DestroyInstanceDelegate DestroyInstance { get; }
            public delegate void DestroyInstanceDelegate(
                IntPtr instance, 
                VkAllocationCallbacks.Raw* pAllocator);

            public EnumeratePhysicalDevicesDelegate EnumeratePhysicalDevices { get; }
            public delegate VkResult EnumeratePhysicalDevicesDelegate(
                IntPtr instance,
                int* pPhysicalDeviceCount,
                IntPtr* pPhysicalDevices);

            public DirectFunctions(IVkInstance instance)
            {
                DestroyInstance =
                    VkHelpers.GetDelegate<DestroyInstanceDelegate>(instance, "vkDestroyInstance");
                EnumeratePhysicalDevices =
                    VkHelpers.GetDelegate<EnumeratePhysicalDevicesDelegate>(instance, "vkEnumeratePhysicalDevices");
            }
        }

        public void Dispose()
        {
            var size = Allocator.SafeMarshalSize();
            VkHelpers.RunWithUnamangedData(size, DisposeInternal);
        }

        private unsafe void DisposeInternal(IntPtr data)
        {
            var unmanaged = (byte*)data;
            var pAllocator = Allocator.SafeMarshalTo(ref unmanaged);
            Direct.DestroyInstance(Handle, pAllocator);
        }

        private unsafe IReadOnlyList<IVkPhysicalDevice> EnumeratePhysicalDevices()
        {
            int count;
            Direct.EnumeratePhysicalDevices(Handle, &count, (IntPtr*)0).CheckSuccess();
            var rawArray = new IntPtr[count];
            fixed (IntPtr* pRawArray = rawArray)
            {
                Direct.EnumeratePhysicalDevices(Handle, &count, pRawArray).CheckSuccess();
            }
            return rawArray.Select(x => new VkPhysicalDevice(this, x)).ToArray();
        }
    }
}