using System;
using System.Collections.Generic;
using System.Linq;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkPhysicalDevice : IVkHandledObject
    {
        IVkPhysicalDeviceProperties Properties { get; }
        IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }
    }

    public unsafe class VkPhysicalDevice : IVkPhysicalDevice
    {
        public IntPtr Handle { get; }
        public DirectFunctions Direct { get; }
        public IVkPhysicalDeviceProperties Properties { get; }
        public IReadOnlyList<IVkQueueFamilyProperties> QueueFamilyProperties { get; }

        public VkPhysicalDevice(IVkInstance instance, IntPtr handle)
        {
            Handle = handle;
            Direct = new DirectFunctions(instance);
            Properties = GetPhysicalDeviceProperties();
            QueueFamilyProperties = GetPhysicalDeviceQueueFamilyProperties();
        }

        public class DirectFunctions
        {
            public GetPhysicalDevicePropertiesDelegate GetPhysicalDeviceProperties { get; }
            public delegate VkResult GetPhysicalDevicePropertiesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceProperties.Raw* pProperties);

            public GetPhysicalDeviceQueueFamilyPropertiesDelegate GetPhysicalDeviceQueueFamilyProperties { get; }
            public delegate void GetPhysicalDeviceQueueFamilyPropertiesDelegate(
                IntPtr physicalDevice,
                int* pQueueFamilyPropertyCount,
                VkQueueFamilyProperties.Raw* pQueueFamilyProperties);

            public DirectFunctions(IVkInstance instance)
            {
                GetPhysicalDeviceProperties =
                    VkHelpers.GetDelegate<GetPhysicalDevicePropertiesDelegate>(instance, "vkGetPhysicalDeviceProperties");
                GetPhysicalDeviceQueueFamilyProperties =
                    VkHelpers.GetDelegate<GetPhysicalDeviceQueueFamilyPropertiesDelegate>(instance, "vkGetPhysicalDeviceQueueFamilyProperties");
            }
        }

        private VkPhysicalDeviceProperties GetPhysicalDeviceProperties()
        {
            VkPhysicalDeviceProperties.Raw raw;
            Direct.GetPhysicalDeviceProperties(Handle, &raw);
            return new VkPhysicalDeviceProperties(&raw);
        }

        private IReadOnlyList<IVkQueueFamilyProperties> GetPhysicalDeviceQueueFamilyProperties()
        {
            int count;
            Direct.GetPhysicalDeviceQueueFamilyProperties(Handle, &count, (VkQueueFamilyProperties.Raw*)0);
            var rawArray = new VkQueueFamilyProperties.Raw[count];
            fixed (VkQueueFamilyProperties.Raw* pRawArray = rawArray)
            {
                Direct.GetPhysicalDeviceQueueFamilyProperties(Handle, &count, pRawArray);
                return rawArray.Select(x => new VkQueueFamilyProperties(&x)).ToArray();
            }
        }
    }
}