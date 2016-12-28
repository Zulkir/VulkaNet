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
    public interface IVkBufferCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkBufferCreateFlags Flags { get; }
        ulong Size { get; }
        VkBufferUsageFlags Usage { get; }
        VkSharingMode SharingMode { get; }
        IReadOnlyList<int> QueueFamilyIndices { get; }
    }

    public unsafe class VkBufferCreateInfo : IVkBufferCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkBufferCreateFlags Flags { get; set; }
        public ulong Size { get; set; }
        public VkBufferUsageFlags Usage { get; set; }
        public VkSharingMode SharingMode { get; set; }
        public IReadOnlyList<int> QueueFamilyIndices { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkBufferCreateFlags flags;
            public ulong size;
            public VkBufferUsageFlags usage;
            public VkSharingMode sharingMode;
            public int queueFamilyIndexCount;
            public int* pQueueFamilyIndices;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkBufferCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkBufferCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.QueueFamilyIndices.SizeOfMarshalDirect();
        }

        public static VkBufferCreateInfo.Raw MarshalDirect(this IVkBufferCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pQueueFamilyIndices = s.QueueFamilyIndices.MarshalDirect(ref unmanaged);

            VkBufferCreateInfo.Raw result;
            result.sType = VkStructureType.BufferCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.size = s.Size;
            result.usage = s.Usage;
            result.sharingMode = s.SharingMode;
            result.queueFamilyIndexCount = s.QueueFamilyIndices?.Count ?? 0;
            result.pQueueFamilyIndices = pQueueFamilyIndices;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkBufferCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkBufferCreateInfo.Raw.SizeInBytes;

        public static VkBufferCreateInfo.Raw* MarshalIndirect(this IVkBufferCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkBufferCreateInfo.Raw*)0;
            var result = (VkBufferCreateInfo.Raw*)unmanaged;
            unmanaged += VkBufferCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkBufferCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkBufferCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkBufferCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkBufferCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferCreateInfo.Raw*)0;
            var result = (VkBufferCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkBufferCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkBufferCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkBufferCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkBufferCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkBufferCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkBufferCreateInfo.Raw**)0;
            var result = (VkBufferCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkBufferCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
