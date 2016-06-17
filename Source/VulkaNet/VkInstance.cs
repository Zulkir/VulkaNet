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