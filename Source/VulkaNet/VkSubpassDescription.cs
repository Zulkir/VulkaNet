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
    public unsafe class VkSubpassDescription
    {
        public VkSubpassDescriptionFlags Flags { get; set; }
        public VkPipelineBindPoint PipelineBindPoint { get; set; }
        public IReadOnlyList<VkAttachmentReference> InputAttachments { get; set; }
        public IReadOnlyList<VkAttachmentReference> ColorAttachments { get; set; }
        public IReadOnlyList<VkAttachmentReference> ResolveAttachments { get; set; }
        public VkAttachmentReference DepthStencilAttachment { get; set; }
        public IReadOnlyList<int> PreserveAttachments { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkSubpassDescriptionFlags flags;
            public VkPipelineBindPoint pipelineBindPoint;
            public int inputAttachmentCount;
            public VkAttachmentReference* pInputAttachments;
            public int colorAttachmentCount;
            public VkAttachmentReference* pColorAttachments;
            public VkAttachmentReference* pResolveAttachments;
            public VkAttachmentReference* pDepthStencilAttachment;
            public int preserveAttachmentCount;
            public int* pPreserveAttachments;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSubpassDescriptionExtensions
    {
        public static int SizeOfMarshalDirect(this VkSubpassDescription s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.InputAttachments.SizeOfMarshalDirect() +
                s.ColorAttachments.SizeOfMarshalDirect() +
                s.ResolveAttachments.SizeOfMarshalDirect() +
                s.DepthStencilAttachment.SizeOfMarshalIndirect() +
                s.PreserveAttachments.SizeOfMarshalDirect();
        }

        public static VkSubpassDescription.Raw MarshalDirect(this VkSubpassDescription s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pInputAttachments = s.InputAttachments.MarshalDirect(ref unmanaged);
            var pColorAttachments = s.ColorAttachments.MarshalDirect(ref unmanaged);
            var pResolveAttachments = s.ResolveAttachments.MarshalDirect(ref unmanaged);
            var pDepthStencilAttachment = s.DepthStencilAttachment.MarshalIndirect(ref unmanaged);
            var pPreserveAttachments = s.PreserveAttachments.MarshalDirect(ref unmanaged);

            VkSubpassDescription.Raw result;
            result.flags = s.Flags;
            result.pipelineBindPoint = s.PipelineBindPoint;
            result.inputAttachmentCount = s.InputAttachments?.Count ?? 0;
            result.pInputAttachments = pInputAttachments;
            result.colorAttachmentCount = s.ColorAttachments?.Count ?? 0;
            result.pColorAttachments = pColorAttachments;
            result.pResolveAttachments = pResolveAttachments;
            result.pDepthStencilAttachment = pDepthStencilAttachment;
            result.preserveAttachmentCount = s.PreserveAttachments?.Count ?? 0;
            result.pPreserveAttachments = pPreserveAttachments;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSubpassDescription s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSubpassDescription.Raw.SizeInBytes;

        public static VkSubpassDescription.Raw* MarshalIndirect(this VkSubpassDescription s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSubpassDescription.Raw*)0;
            var result = (VkSubpassDescription.Raw*)unmanaged;
            unmanaged += VkSubpassDescription.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSubpassDescription> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSubpassDescription.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSubpassDescription.Raw* MarshalDirect(this IReadOnlyList<VkSubpassDescription> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubpassDescription.Raw*)0;
            var result = (VkSubpassDescription.Raw*)unmanaged;
            unmanaged += sizeof(VkSubpassDescription.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkSubpassDescription> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkSubpassDescription.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkSubpassDescription.Raw** MarshalIndirect(this IReadOnlyList<VkSubpassDescription> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSubpassDescription.Raw**)0;
            var result = (VkSubpassDescription.Raw**)unmanaged;
            unmanaged += sizeof(VkSubpassDescription.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
