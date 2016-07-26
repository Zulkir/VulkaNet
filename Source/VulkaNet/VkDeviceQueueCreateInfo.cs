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
    public interface IVkDeviceQueueCreateInfo
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
    }

    public static unsafe class VkDeviceQueueCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkDeviceQueueCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.QueuePriorities.SizeOfMarshalDirect();
        }

        public static VkDeviceQueueCreateInfo.Raw MarshalDirect(this IVkDeviceQueueCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pQueuePriorities = s.QueuePriorities.MarshalDirect(ref unmanaged);

            VkDeviceQueueCreateInfo.Raw result;
            result.sType = VkStructureType.DeviceQueueCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.queueFamilyIndex = s.QueueFamilyIndex;
            result.queueCount = s.QueuePriorities?.Count ?? 0;
            result.pQueuePriorities = pQueuePriorities;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkDeviceQueueCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDeviceQueueCreateInfo.Raw.SizeInBytes;

        public static VkDeviceQueueCreateInfo.Raw* MarshalIndirect(this IVkDeviceQueueCreateInfo s, ref byte* unmanaged)
        {
            var result = (VkDeviceQueueCreateInfo.Raw*)unmanaged;
            unmanaged += VkDeviceQueueCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDeviceQueueCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDeviceQueueCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDeviceQueueCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkDeviceQueueCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDeviceQueueCreateInfo.Raw*)0;
            var result = (VkDeviceQueueCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkDeviceQueueCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkDeviceQueueCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkDeviceQueueCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkDeviceQueueCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkDeviceQueueCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDeviceQueueCreateInfo.Raw**)0;
            var result = (VkDeviceQueueCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkDeviceQueueCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
