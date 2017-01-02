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
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe class VkDeviceCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkDeviceCreateFlags Flags { get; set; }
        public IReadOnlyList<VkDeviceQueueCreateInfo> QueueCreateInfos { get; set; }
        public IReadOnlyList<string> EnabledLayerNames { get; set; }
        public IReadOnlyList<string> EnabledExtensionNames { get; set; }
        public VkPhysicalDeviceFeatures EnabledFeatures { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDeviceCreateFlags flags;
            public int queueCreateInfoCount;
            public VkDeviceQueueCreateInfo.Raw* pQueueCreateInfos;
            public int enabledLayerCount;
            public byte** ppEnabledLayerNames;
            public int enabledExtensionCount;
            public byte** ppEnabledExtensionNames;
            public VkPhysicalDeviceFeatures.Raw* pEnabledFeatures;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDeviceCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkDeviceCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.QueueCreateInfos.SizeOfMarshalDirect() +
                s.EnabledLayerNames.SizeOfMarshalIndirect() +
                s.EnabledExtensionNames.SizeOfMarshalIndirect() +
                s.EnabledFeatures.SizeOfMarshalIndirect();
        }

        public static VkDeviceCreateInfo.Raw MarshalDirect(this VkDeviceCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pQueueCreateInfos = s.QueueCreateInfos.MarshalDirect(ref unmanaged);
            var ppEnabledLayerNames = s.EnabledLayerNames.MarshalIndirect(ref unmanaged);
            var ppEnabledExtensionNames = s.EnabledExtensionNames.MarshalIndirect(ref unmanaged);
            var pEnabledFeatures = s.EnabledFeatures.MarshalIndirect(ref unmanaged);

            VkDeviceCreateInfo.Raw result;
            result.sType = VkStructureType.DeviceCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.queueCreateInfoCount = s.QueueCreateInfos?.Count ?? 0;
            result.pQueueCreateInfos = pQueueCreateInfos;
            result.enabledLayerCount = s.EnabledLayerNames?.Count ?? 0;
            result.ppEnabledLayerNames = ppEnabledLayerNames;
            result.enabledExtensionCount = s.EnabledExtensionNames?.Count ?? 0;
            result.ppEnabledExtensionNames = ppEnabledExtensionNames;
            result.pEnabledFeatures = pEnabledFeatures;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDeviceCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDeviceCreateInfo.Raw.SizeInBytes;

        public static VkDeviceCreateInfo.Raw* MarshalIndirect(this VkDeviceCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDeviceCreateInfo.Raw*)0;
            var result = (VkDeviceCreateInfo.Raw*)unmanaged;
            unmanaged += VkDeviceCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDeviceCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDeviceCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDeviceCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkDeviceCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDeviceCreateInfo.Raw*)0;
            var result = (VkDeviceCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDeviceCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkDeviceCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkDeviceCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkDeviceCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<VkDeviceCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDeviceCreateInfo.Raw**)0;
            var result = (VkDeviceCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkDeviceCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
