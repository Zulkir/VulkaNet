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