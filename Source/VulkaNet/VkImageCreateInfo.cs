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
    public interface IVkImageCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkImageCreateFlags Flags { get; }
        VkImageType ImageType { get; }
        VkFormat Format { get; }
        VkExtent3D Extent { get; }
        int MipLevels { get; }
        int ArrayLayers { get; }
        VkSampleCountFlagBits Samples { get; }
        VkImageTiling Tiling { get; }
        VkImageUsageFlags Usage { get; }
        VkSharingMode SharingMode { get; }
        IReadOnlyList<int> QueueFamilyIndices { get; }
        VkImageLayout InitialLayout { get; }
    }

    public unsafe class VkImageCreateInfo : IVkImageCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkImageCreateFlags Flags { get; set; }
        public VkImageType ImageType { get; set; }
        public VkFormat Format { get; set; }
        public VkExtent3D Extent { get; set; }
        public int MipLevels { get; set; }
        public int ArrayLayers { get; set; }
        public VkSampleCountFlagBits Samples { get; set; }
        public VkImageTiling Tiling { get; set; }
        public VkImageUsageFlags Usage { get; set; }
        public VkSharingMode SharingMode { get; set; }
        public IReadOnlyList<int> QueueFamilyIndices { get; set; }
        public VkImageLayout InitialLayout { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkImageCreateFlags flags;
            public VkImageType imageType;
            public VkFormat format;
            public VkExtent3D extent;
            public int mipLevels;
            public int arrayLayers;
            public VkSampleCountFlagBits samples;
            public VkImageTiling tiling;
            public VkImageUsageFlags usage;
            public VkSharingMode sharingMode;
            public int queueFamilyIndexCount;
            public int* pQueueFamilyIndices;
            public VkImageLayout initialLayout;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkImageCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkImageCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.QueueFamilyIndices.SizeOfMarshalDirect();
        }

        public static VkImageCreateInfo.Raw MarshalDirect(this IVkImageCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pQueueFamilyIndices = s.QueueFamilyIndices.MarshalDirect(ref unmanaged);

            VkImageCreateInfo.Raw result;
            result.sType = VkStructureType.ImageCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.imageType = s.ImageType;
            result.format = s.Format;
            result.extent = s.Extent;
            result.mipLevels = s.MipLevels;
            result.arrayLayers = s.ArrayLayers;
            result.samples = s.Samples;
            result.tiling = s.Tiling;
            result.usage = s.Usage;
            result.sharingMode = s.SharingMode;
            result.queueFamilyIndexCount = s.QueueFamilyIndices?.Count ?? 0;
            result.pQueueFamilyIndices = pQueueFamilyIndices;
            result.initialLayout = s.InitialLayout;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkImageCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkImageCreateInfo.Raw.SizeInBytes;

        public static VkImageCreateInfo.Raw* MarshalIndirect(this IVkImageCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkImageCreateInfo.Raw*)0;
            var result = (VkImageCreateInfo.Raw*)unmanaged;
            unmanaged += VkImageCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkImageCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkImageCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkImageCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkImageCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkImageCreateInfo.Raw*)0;
            var result = (VkImageCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkImageCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkImageCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkImageCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkImageCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkImageCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkImageCreateInfo.Raw**)0;
            var result = (VkImageCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkImageCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
