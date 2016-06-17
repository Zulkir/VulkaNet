using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkDeviceQueueCreateInfo : IVkStructWrapper
    {
        IVkStructWrapper Next { get; }
        VkDeviceQueueCreateFlags Flags { get; }
        int QueueFamilyIndex { get; }
        IReadOnlyList<float> QueuePriorities { get; }
    }

    public unsafe class VkDeviceQueueCreateInfo : IVkDeviceQueueCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkDeviceQueueCreateFlags Flags { get; set; }
        public int QueueFamilyIndex { get; set; }
        public IReadOnlyList<float> QueuePriorities { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDeviceQueueCreateFlags flags;
            public int queueFamilyIndex;
            public int queueCount;
            public float* pQueuePriorities;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }

        public int MarshalSize()
            => Next.SafeMarshalSize() +
               QueuePriorities.SafeMarshalSize() +
               Raw.SizeInBytes;

        public Raw* MarshalTo(ref byte* unmanaged)
        {
            var pNext = Next.SafeMarshalTo(ref unmanaged);
            var pQueuePriorities = QueuePriorities.SafeMarshalTo(ref unmanaged);

            var result = (Raw*)unmanaged;
            unmanaged += Raw.SizeInBytes;
            result->sType = VkStructureType.DeviceQueueCreateInfo;
            result->pNext = pNext;
            result->flags = Flags;
            result->queueFamilyIndex = QueueFamilyIndex;
            result->queueCount = QueuePriorities?.Count ?? 0;
            result->pQueuePriorities = pQueuePriorities;
            return result;
        }

        void* IVkStructWrapper.MarshalTo(ref byte* unmanaged)
            => MarshalTo(ref unmanaged);
    }

    public static class VkDeviceQueueCreateInfoExtensions
    {
        public static int SafeMarshalSize(this VkDeviceQueueCreateInfo s)
            => s?.MarshalSize() ?? 0;

        public static unsafe VkDeviceQueueCreateInfo.Raw* SafeMarshalTo(this VkDeviceQueueCreateInfo s, ref byte* unmanaged) 
            => s != null ? s.MarshalTo(ref unmanaged) : (VkDeviceQueueCreateInfo.Raw*)0;
    }
}