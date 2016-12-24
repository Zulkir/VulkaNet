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
    public interface IVkImageMemoryBarrier
    {
        IVkStructWrapper Next { get; }
        VkAccessFlags SrcAccessMask { get; }
        VkAccessFlags DstAccessMask { get; }
        VkImageLayout OldLayout { get; }
        VkImageLayout NewLayout { get; }
        int SrcQueueFamilyIndex { get; }
        int DstQueueFamilyIndex { get; }
        IVkImage Image { get; }
        VkImageSubresourceRange SubresourceRange { get; }
    }

    public unsafe class VkImageMemoryBarrier : IVkImageMemoryBarrier
    {
        public IVkStructWrapper Next { get; set; }
        public VkAccessFlags SrcAccessMask { get; set; }
        public VkAccessFlags DstAccessMask { get; set; }
        public VkImageLayout OldLayout { get; set; }
        public VkImageLayout NewLayout { get; set; }
        public int SrcQueueFamilyIndex { get; set; }
        public int DstQueueFamilyIndex { get; set; }
        public IVkImage Image { get; set; }
        public VkImageSubresourceRange SubresourceRange { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkAccessFlags srcAccessMask;
            public VkAccessFlags dstAccessMask;
            public VkImageLayout oldLayout;
            public VkImageLayout newLayout;
            public int srcQueueFamilyIndex;
            public int dstQueueFamilyIndex;
            public VkImage.HandleType image;
            public VkImageSubresourceRange subresourceRange;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkImageMemoryBarrierExtensions
    {
        public static int SizeOfMarshalDirect(this IVkImageMemoryBarrier s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkImageMemoryBarrier.Raw MarshalDirect(this IVkImageMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkImageMemoryBarrier.Raw result;
            result.sType = VkStructureType.ImageMemoryBarrier;
            result.pNext = pNext;
            result.srcAccessMask = s.SrcAccessMask;
            result.dstAccessMask = s.DstAccessMask;
            result.oldLayout = s.OldLayout;
            result.newLayout = s.NewLayout;
            result.srcQueueFamilyIndex = s.SrcQueueFamilyIndex;
            result.dstQueueFamilyIndex = s.DstQueueFamilyIndex;
            result.image = s.Image?.Handle ?? VkImage.HandleType.Null;
            result.subresourceRange = s.SubresourceRange;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkImageMemoryBarrier s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkImageMemoryBarrier.Raw.SizeInBytes;

        public static VkImageMemoryBarrier.Raw* MarshalIndirect(this IVkImageMemoryBarrier s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkImageMemoryBarrier.Raw*)0;
            var result = (VkImageMemoryBarrier.Raw*)unmanaged;
            unmanaged += VkImageMemoryBarrier.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkImageMemoryBarrier> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkImageMemoryBarrier.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkImageMemoryBarrier.Raw* MarshalDirect(this IReadOnlyList<IVkImageMemoryBarrier> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkImageMemoryBarrier.Raw*)0;
            var result = (VkImageMemoryBarrier.Raw*)unmanaged;
            unmanaged += sizeof(VkImageMemoryBarrier.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkImageMemoryBarrier> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkImageMemoryBarrier.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkImageMemoryBarrier.Raw** MarshalIndirect(this IReadOnlyList<IVkImageMemoryBarrier> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkImageMemoryBarrier.Raw**)0;
            var result = (VkImageMemoryBarrier.Raw**)unmanaged;
            unmanaged += sizeof(VkImageMemoryBarrier.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
