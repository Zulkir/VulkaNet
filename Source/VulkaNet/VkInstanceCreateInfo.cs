using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public class VkInstanceCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkInstanceCreateFlags Flags { get; set; }
        public VkApplicationInfo ApplicationInfo { get; set; }
        public IReadOnlyList<string> EnabledLayerNames { get; set; }
        public IReadOnlyList<string> EnabledExtensionNames { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkInstanceCreateFlags flags;
            public VkApplicationInfo.Raw* pApplicationInfo;
            public int enabledLayerCount;
            public byte** ppEnabledLayerNames;
            public int enabledExtensionCount;
            public byte** ppEnabledExtensionNames;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static class VkInstanceCreateInfoExtensions
    {
        public static int SafeMarshalSize(this VkInstanceCreateInfo s)
            => s != null
                ? s.Next.SafeMarshalSize() +
                  s.ApplicationInfo.SafeMarshalSize() +
                  s.EnabledLayerNames.SafeMarshalSize() +
                  s.EnabledExtensionNames.SafeMarshalSize() +
                  VkInstanceCreateInfo.Raw.SizeInBytes
                : 0;

        public static unsafe VkInstanceCreateInfo.Raw* SafeMarshalTo(this VkInstanceCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkInstanceCreateInfo.Raw*)0;

            var pNext = s.Next.SafeMarshalTo(ref unmanaged);
            var pAppInfo = s.ApplicationInfo.SafeMarshalTo(ref unmanaged);
            var pLayerNames = s.EnabledLayerNames.SafeMarshalTo(ref unmanaged);
            var pExtensionNames = s.EnabledExtensionNames.SafeMarshalTo(ref unmanaged);

            var result = (VkInstanceCreateInfo.Raw*)unmanaged;
            unmanaged += VkInstanceCreateInfo.Raw.SizeInBytes;
            result->sType = VkStructureType.InstanceCreateInfo;
            result->pNext = pNext;
            result->flags = s.Flags;
            result->pApplicationInfo = pAppInfo;
            result->enabledLayerCount = s.EnabledLayerNames?.Count ?? 0;
            result->ppEnabledLayerNames = pLayerNames;
            result->enabledExtensionCount = s.EnabledExtensionNames?.Count ?? 0;
            result->ppEnabledExtensionNames = pExtensionNames;
            return result;
        }
    }
}