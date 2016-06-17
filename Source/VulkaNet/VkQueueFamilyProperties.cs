using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkQueueFamilyProperties
    {
        VkQueueFlags QueueFlags { get; }
        int QueueCount { get; }
        int TimestampValidBits { get; }
        VkExtent3D MinImageTransferGranularity { get; }
    }

    public class VkQueueFamilyProperties : IVkQueueFamilyProperties
    {
        public VkQueueFlags QueueFlags { get; }
        public int QueueCount { get; }
        public int TimestampValidBits { get; }
        public VkExtent3D MinImageTransferGranularity { get; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkQueueFlags queueFlags;
            public int queueCount;
            public int timestampValidBits;
            public VkExtent3D minImageTransferGranularity;
        }

        public unsafe VkQueueFamilyProperties(Raw* raw)
        {
            QueueFlags = raw->queueFlags;
            QueueCount = raw->queueCount;
            TimestampValidBits = raw->timestampValidBits;
            MinImageTransferGranularity = raw->minImageTransferGranularity;
        }
    }
}