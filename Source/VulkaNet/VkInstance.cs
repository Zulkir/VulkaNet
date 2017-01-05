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
    public interface IVkInstance : IVkHandledObject, IDisposable
    {
        VkInstance.HandleType Handle { get; }
        VkInstance.DirectFunctions Direct { get; }
        IReadOnlyList<IVkPhysicalDevice> PhysicalDevices { get; }
    }

    public class VkInstance : IVkInstance
    {
        public HandleType Handle { get; }
        private VkAllocationCallbacks Allocator { get; }
        public DirectFunctions Direct { get; }
        public IReadOnlyList<IVkPhysicalDevice> PhysicalDevices { get; }

        public IntPtr RawHandle => Handle.InternalHandle;

        public VkInstance(HandleType handle, VkAllocationCallbacks allocator)
        {
            Handle = handle;
            Allocator = allocator;
            Direct = new DirectFunctions(this);
            PhysicalDevices = EnumeratePhysicalDevices();
        }

        public struct HandleType
        {
            public readonly IntPtr InternalHandle;
            public HandleType(IntPtr internalHandle) { InternalHandle = internalHandle; }
            public override string ToString() => InternalHandle.ToString();
            public static int SizeInBytes { get; } = IntPtr.Size;
            public static HandleType Null => new HandleType(default(IntPtr));
        }

        public unsafe class DirectFunctions
        {
            public DestroyInstanceDelegate DestroyInstance { get; }
            public delegate void DestroyInstanceDelegate(
                HandleType instance, 
                VkAllocationCallbacks.Raw* pAllocator);

            public EnumeratePhysicalDevicesDelegate EnumeratePhysicalDevices { get; }
            public delegate VkResult EnumeratePhysicalDevicesDelegate(
                HandleType instance,
                int* pPhysicalDeviceCount,
                IntPtr* pPhysicalDevices);

            public CreateAndroidSurfaceKHRDelegate CreateAndroidSurfaceKHR { get; }
            public delegate VkResult CreateAndroidSurfaceKHRDelegate(
                HandleType instance,
                VkAndroidSurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public CreateMirSurfaceKHRDelegate CreateMirSurfaceKHR { get; }
            public delegate VkResult CreateMirSurfaceKHRDelegate(
                HandleType instance,
                VkMirSurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public CreateWaylandSurfaceKHRDelegate CreateWaylandSurfaceKHR { get; }
            public delegate VkResult CreateWaylandSurfaceKHRDelegate(
                HandleType instance,
                VkWaylandSurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public CreateWin32SurfaceKHRDelegate CreateWin32SurfaceKHR { get; }
            public delegate VkResult CreateWin32SurfaceKHRDelegate(
                HandleType instance,
                VkWin32SurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public CreateXcbSurfaceKHRDelegate CreateXcbSurfaceKHR { get; }
            public delegate VkResult CreateXcbSurfaceKHRDelegate(
                HandleType instance,
                VkXcbSurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public CreateXlibSurfaceKHRDelegate CreateXlibSurfaceKHR { get; }
            public delegate VkResult CreateXlibSurfaceKHRDelegate(
                HandleType instance,
                VkXlibSurfaceCreateInfoKHR.Raw* pCreateInfo,
                VkAllocationCallbacks.Raw* pAllocator,
                VkSurfaceKHR.HandleType* pSurface);

            public DestroySurfaceKHRDelegate DestroySurfaceKHR { get; }
            public delegate VkResult DestroySurfaceKHRDelegate(
                HandleType instance,
                VkSurfaceKHR.HandleType surface,
                VkAllocationCallbacks.Raw* pAllocator);

            public DirectFunctions(IVkInstance instance)
            {
                DestroyInstance = VkHelpers.GetInstanceDelegate<DestroyInstanceDelegate>(instance, "vkDestroyInstance");
                EnumeratePhysicalDevices = VkHelpers.GetInstanceDelegate<EnumeratePhysicalDevicesDelegate>(instance, "vkEnumeratePhysicalDevices");
                CreateAndroidSurfaceKHR = VkHelpers.GetInstanceDelegate<CreateAndroidSurfaceKHRDelegate>(instance, "vkCreateAndroidSurfaceKHR");
                CreateMirSurfaceKHR = VkHelpers.GetInstanceDelegate<CreateMirSurfaceKHRDelegate>(instance, "vkCreateMirSurfaceKHR");
                CreateWaylandSurfaceKHR = VkHelpers.GetInstanceDelegate<CreateWaylandSurfaceKHRDelegate>(instance, "vkCreateWaylandSurfaceKHR");
                CreateWin32SurfaceKHR = VkHelpers.GetInstanceDelegate<CreateWin32SurfaceKHRDelegate>(instance, "vkCreateWin32SurfaceKHR");
                CreateXcbSurfaceKHR = VkHelpers.GetInstanceDelegate<CreateXcbSurfaceKHRDelegate>(instance, "vkCreateXcbSurfaceKHR");
                CreateXlibSurfaceKHR = VkHelpers.GetInstanceDelegate<CreateXlibSurfaceKHRDelegate>(instance, "vkCreateXlibSurfaceKHR");
                DestroySurfaceKHR = VkHelpers.GetInstanceDelegate<DestroySurfaceKHRDelegate>(instance, "vkDestroySurfaceKHR");
            }
        }

        public void Dispose()
        {
            var size = Allocator.SizeOfMarshalIndirect();
            VkHelpers.RunWithUnamangedData(size, DisposeInternal);
        }

        private unsafe void DisposeInternal(IntPtr data)
        {
            var unmanaged = (byte*)data;
            var pAllocator = Allocator.MarshalIndirect(ref unmanaged);
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