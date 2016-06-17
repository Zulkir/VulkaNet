using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkDeviceCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkDeviceCreateFlags Flags { get; }
        IReadOnlyList<IVkDeviceQueueCreateInfo> QueueCreateInfos { get; }
        IReadOnlyList<string> EnabledExtensionNames { get; }
        IVkPhysicalDeviceFeatures EnabledFeatures { get; }
    }

    public unsafe class VkDeviceCreateInfo : IVkDeviceCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkDeviceCreateFlags Flags { get; set; }
        public IReadOnlyList<IVkDeviceQueueCreateInfo> QueueCreateInfos { get; set; }
        public IReadOnlyList<string> EnabledExtensionNames { get; set; }
        public IVkPhysicalDeviceFeatures EnabledFeatures { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDeviceCreateFlags flags;
            public int queueCreateInfoCount;
            public VkDeviceQueueCreateInfo.Raw** pQueueCreateInfos;
            public int enabledLayerCount;
            public byte** ppEnabledLayerNames;
            public int enabledExtensionCount;
            public byte** ppEnabledExtensionNames;
            public VkPhysicalDeviceFeatures.Raw* pEnabledFeatures;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static class VkDeviceCreateInfoExtensions
    {
        public static int SafeMarshalSize(this IVkDeviceCreateInfo s)
            => s != null
                ? VkDeviceCreateInfo.Raw.SizeInBytes +
                  s.Next.SafeMarshalSize() +
                  s.QueueCreateInfos.SafeMarshalSize() +
                  s.EnabledExtensionNames.SafeMarshalSize() +
                  s.EnabledFeatures.SafeMarshalSize()
                : 0;

        public static unsafe VkDeviceCreateInfo.Raw* SafeMarshalTo(this IVkDeviceCreateInfo s, ref byte* unmanaged)
        {
            var pNext = s.Next.SafeMarshalTo(ref unmanaged);
            var pQueueCreateInfos = (VkDeviceQueueCreateInfo.Raw**)s.QueueCreateInfos.SafeMarshalTo(ref unmanaged);
            var ppEnabledExtensionNames = s.EnabledExtensionNames.SafeMarshalTo(ref unmanaged);
            var pEnabledFeatures = s.EnabledFeatures.SafeMarshalTo(ref unmanaged);
            var result = (VkDeviceCreateInfo.Raw*)unmanaged;
            unmanaged += VkDeviceCreateInfo.Raw.SizeInBytes;
            result->sType = VkStructureType.DeviceCreateInfo;
            result->pNext = pNext;
            result->flags = s.Flags;
            result->queueCreateInfoCount = s.QueueCreateInfos?.Count ?? 0;
            result->pQueueCreateInfos = pQueueCreateInfos;
            result->enabledLayerCount = 0;
            result->ppEnabledLayerNames = (byte**)0;
            result->enabledExtensionCount = s.EnabledExtensionNames?.Count ?? 0;
            result->ppEnabledExtensionNames = ppEnabledExtensionNames;
            result->pEnabledFeatures = pEnabledFeatures;
            return result;
        }
    }
}