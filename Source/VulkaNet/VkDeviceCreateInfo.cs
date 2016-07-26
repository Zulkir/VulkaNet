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
            var ppEnabledExtensionNames = s.EnabledExtensionNames.direct(ref unmanaged);
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