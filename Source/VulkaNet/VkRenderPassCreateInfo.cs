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
    public unsafe class VkRenderPassCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkRenderPassCreateFlags Flags { get; set; }
        public IReadOnlyList<VkAttachmentDescription> Attachments { get; set; }
        public IReadOnlyList<VkSubpassDescription> Subpasses { get; set; }
        public IReadOnlyList<VkSubpassDependency> Dependencies { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkRenderPassCreateFlags flags;
            public int attachmentCount;
            public VkAttachmentDescription.Raw* pAttachments;
            public int subpassCount;
            public VkSubpassDescription.Raw* pSubpasses;
            public int dependencyCount;
            public VkSubpassDependency.Raw* pDependencies;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkRenderPassCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkRenderPassCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Attachments.SizeOfMarshalDirect() +
                s.Subpasses.SizeOfMarshalDirect() +
                s.Dependencies.SizeOfMarshalDirect();
        }

        public static VkRenderPassCreateInfo.Raw MarshalDirect(this VkRenderPassCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pAttachments = s.Attachments.MarshalDirect(ref unmanaged);
            var pSubpasses = s.Subpasses.MarshalDirect(ref unmanaged);
            var pDependencies = s.Dependencies.MarshalDirect(ref unmanaged);

            VkRenderPassCreateInfo.Raw result;
            result.sType = VkStructureType.RenderPassCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.attachmentCount = s.Attachments?.Count ?? 0;
            result.pAttachments = pAttachments;
            result.subpassCount = s.Subpasses?.Count ?? 0;
            result.pSubpasses = pSubpasses;
            result.dependencyCount = s.Dependencies?.Count ?? 0;
            result.pDependencies = pDependencies;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkRenderPassCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkRenderPassCreateInfo.Raw.SizeInBytes;

        public static VkRenderPassCreateInfo.Raw* MarshalIndirect(this VkRenderPassCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkRenderPassCreateInfo.Raw*)0;
            var result = (VkRenderPassCreateInfo.Raw*)unmanaged;
            unmanaged += VkRenderPassCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkRenderPassCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkRenderPassCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkRenderPassCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkRenderPassCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkRenderPassCreateInfo.Raw*)0;
            var result = (VkRenderPassCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkRenderPassCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
