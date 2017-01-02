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
    public unsafe class VkFramebufferCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkFramebufferCreateFlags Flags { get; set; }
        public IVkRenderPass RenderPass { get; set; }
        public IReadOnlyList<IVkImageView> Attachments { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Layers { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkFramebufferCreateFlags flags;
            public VkRenderPass.HandleType renderPass;
            public int attachmentCount;
            public VkImageView.HandleType* pAttachments;
            public int width;
            public int height;
            public int layers;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkFramebufferCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkFramebufferCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Attachments.SizeOfMarshalDirect();
        }

        public static VkFramebufferCreateInfo.Raw MarshalDirect(this VkFramebufferCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pAttachments = s.Attachments.MarshalDirect(ref unmanaged);

            VkFramebufferCreateInfo.Raw result;
            result.sType = VkStructureType.FramebufferCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.renderPass = s.RenderPass?.Handle ?? VkRenderPass.HandleType.Null;
            result.attachmentCount = s.Attachments?.Count ?? 0;
            result.pAttachments = pAttachments;
            result.width = s.Width;
            result.height = s.Height;
            result.layers = s.Layers;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkFramebufferCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkFramebufferCreateInfo.Raw.SizeInBytes;

        public static VkFramebufferCreateInfo.Raw* MarshalIndirect(this VkFramebufferCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkFramebufferCreateInfo.Raw*)0;
            var result = (VkFramebufferCreateInfo.Raw*)unmanaged;
            unmanaged += VkFramebufferCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkFramebufferCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkFramebufferCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkFramebufferCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkFramebufferCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkFramebufferCreateInfo.Raw*)0;
            var result = (VkFramebufferCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkFramebufferCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
