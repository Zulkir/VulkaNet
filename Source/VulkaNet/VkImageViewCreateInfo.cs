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
    public unsafe class VkImageViewCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkImageViewCreateFlags Flags { get; set; }
        public IVkImage Image { get; set; }
        public VkImageViewType ViewType { get; set; }
        public VkFormat Format { get; set; }
        public VkComponentMapping Components { get; set; }
        public VkImageSubresourceRange SubresourceRange { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkImageViewCreateFlags flags;
            public VkImage.HandleType image;
            public VkImageViewType viewType;
            public VkFormat format;
            public VkComponentMapping components;
            public VkImageSubresourceRange subresourceRange;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkImageViewCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkImageViewCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkImageViewCreateInfo.Raw MarshalDirect(this VkImageViewCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkImageViewCreateInfo.Raw result;
            result.sType = VkStructureType.ImageViewCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.image = s.Image?.Handle ?? VkImage.HandleType.Null;
            result.viewType = s.ViewType;
            result.format = s.Format;
            result.components = s.Components;
            result.subresourceRange = s.SubresourceRange;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkImageViewCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkImageViewCreateInfo.Raw.SizeInBytes;

        public static VkImageViewCreateInfo.Raw* MarshalIndirect(this VkImageViewCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkImageViewCreateInfo.Raw*)0;
            var result = (VkImageViewCreateInfo.Raw*)unmanaged;
            unmanaged += VkImageViewCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkImageViewCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkImageViewCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkImageViewCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkImageViewCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkImageViewCreateInfo.Raw*)0;
            var result = (VkImageViewCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkImageViewCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
