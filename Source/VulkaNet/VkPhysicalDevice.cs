using System;
using VulkaNet.InternalHelpers;

namespace VulkaNet
{
    public interface IVkPhysicalDevice : IVkHandledObject
    {
        IVkPhysicalDeviceProperties Properties { get; }
    }

    public unsafe class VkPhysicalDevice : IVkPhysicalDevice
    {
        public IntPtr Handle { get; }
        public DirectFunctions Direct { get; }
        public IVkPhysicalDeviceProperties Properties { get; }

        public VkPhysicalDevice(IVkInstance instance, IntPtr handle)
        {
            Handle = handle;
            Direct = new DirectFunctions(instance);
            Properties = GetPhysicalDeviceProperties();
        }

        public class DirectFunctions
        {
            public GetPhysicalDevicePropertiesDelegate GetPhysicalDeviceProperties { get; }
            public delegate VkResult GetPhysicalDevicePropertiesDelegate(
                IntPtr physicalDevice,
                VkPhysicalDeviceProperties.Raw* pProperties);

            public DirectFunctions(IVkInstance instance)
            {
                GetPhysicalDeviceProperties =
                    VkHelpers.GetDelegate<GetPhysicalDevicePropertiesDelegate>(instance, "vkGetPhysicalDeviceProperties");
            }
        }

        private VkPhysicalDeviceProperties GetPhysicalDeviceProperties()
        {
            VkPhysicalDeviceProperties.Raw raw;
            Direct.GetPhysicalDeviceProperties(Handle, &raw);
            return new VkPhysicalDeviceProperties(&raw);
        }
    }
}