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
    public unsafe class VkSubpassDependency
    {
        public int SrcSubpass { get; set; }
        public int DstSubpass { get; set; }
        public VkPipelineStageFlags SrcStageMask { get; set; }
        public VkPipelineStageFlags DstStageMask { get; set; }
        public VkAccessFlags SrcAccessMask { get; set; }
        public VkAccessFlags DstAccessMask { get; set; }
        public VkDependencyFlags DependencyFlags { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public int srcSubpass;
            public int dstSubpass;
            public VkPipelineStageFlags srcStageMask;
            public VkPipelineStageFlags dstStageMask;
            public VkAccessFlags srcAccessMask;
            public VkAccessFlags dstAccessMask;
            public VkDependencyFlags dependencyFlags;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSubpassDependencyExtensions
    {
        public static int SizeOfMarshalDirect(this VkSubpassDependency s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return 0;
        }

        public static VkSubpassDependency.Raw MarshalDirect(this VkSubpassDependency s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");


            VkSubpassDependency.Raw result;
            result.srcSubpass = s.SrcSubpass;
            result.dstSubpass = s.DstSubpass;
            result.srcStageMask = s.SrcStageMask;
            result.dstStageMask = s.DstStageMask;
            result.srcAccessMask = s.SrcAccessMask;
            result.dstAccessMask = s.DstAccessMask;
            result.dependencyFlags = s.DependencyFlags;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSubpassDependency s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSubpassDependency.Raw.SizeInBytes;

        public static VkSubpassDependency.Raw* MarshalIndirect(this VkSubpassDependency s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSubpassDependency.Raw*)0;
            var result = (VkSubpassDependency.Raw*)unmanaged;
            unmanaged += VkSubpassDependency.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSubpassDependency> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSubpassDependency.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSubpassDependency.Raw* MarshalDirect(this IReadOnlyList<VkSubpassDependency> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubpassDependency.Raw*)0;
            var result = (VkSubpassDependency.Raw*)unmanaged;
            unmanaged += sizeof(VkSubpassDependency.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkSubpassDependency> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkSubpassDependency.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkSubpassDependency.Raw** MarshalIndirect(this IReadOnlyList<VkSubpassDependency> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubpassDependency.Raw**)0;
            var result = (VkSubpassDependency.Raw**)unmanaged;
            unmanaged += sizeof(VkSubpassDependency.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
