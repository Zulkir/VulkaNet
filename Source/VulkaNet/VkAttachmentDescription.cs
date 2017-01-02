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
    public unsafe class VkAttachmentDescription
    {
        public VkAttachmentDescriptionFlags Flags { get; set; }
        public VkFormat Format { get; set; }
        public VkSampleCountFlagBits Samples { get; set; }
        public VkAttachmentLoadOp LoadOp { get; set; }
        public VkAttachmentStoreOp StoreOp { get; set; }
        public VkAttachmentLoadOp StencilLoadOp { get; set; }
        public VkAttachmentStoreOp StencilStoreOp { get; set; }
        public VkImageLayout InitialLayout { get; set; }
        public VkImageLayout FinalLayout { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkAttachmentDescriptionFlags flags;
            public VkFormat format;
            public VkSampleCountFlagBits samples;
            public VkAttachmentLoadOp loadOp;
            public VkAttachmentStoreOp storeOp;
            public VkAttachmentLoadOp stencilLoadOp;
            public VkAttachmentStoreOp stencilStoreOp;
            public VkImageLayout initialLayout;
            public VkImageLayout finalLayout;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkAttachmentDescriptionExtensions
    {
        public static int SizeOfMarshalDirect(this VkAttachmentDescription s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return 0;
        }

        public static VkAttachmentDescription.Raw MarshalDirect(this VkAttachmentDescription s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");


            VkAttachmentDescription.Raw result;
            result.flags = s.Flags;
            result.format = s.Format;
            result.samples = s.Samples;
            result.loadOp = s.LoadOp;
            result.storeOp = s.StoreOp;
            result.stencilLoadOp = s.StencilLoadOp;
            result.stencilStoreOp = s.StencilStoreOp;
            result.initialLayout = s.InitialLayout;
            result.finalLayout = s.FinalLayout;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkAttachmentDescription s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkAttachmentDescription.Raw.SizeInBytes;

        public static VkAttachmentDescription.Raw* MarshalIndirect(this VkAttachmentDescription s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkAttachmentDescription.Raw*)0;
            var result = (VkAttachmentDescription.Raw*)unmanaged;
            unmanaged += VkAttachmentDescription.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkAttachmentDescription> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkAttachmentDescription.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkAttachmentDescription.Raw* MarshalDirect(this IReadOnlyList<VkAttachmentDescription> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkAttachmentDescription.Raw*)0;
            var result = (VkAttachmentDescription.Raw*)unmanaged;
            unmanaged += sizeof(VkAttachmentDescription.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<VkAttachmentDescription> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkAttachmentDescription.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkAttachmentDescription.Raw** MarshalIndirect(this IReadOnlyList<VkAttachmentDescription> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkAttachmentDescription.Raw**)0;
            var result = (VkAttachmentDescription.Raw**)unmanaged;
            unmanaged += sizeof(VkAttachmentDescription.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
