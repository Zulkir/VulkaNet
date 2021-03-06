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

using System.Collections.Generic;

namespace VulkaNet
{
    public unsafe struct VkAttachmentDescription
    {
        public VkAttachmentDescriptionFlags Flags { get; set; }
        public VkFormat Format { get; set; }
        public VkSampleCount Samples { get; set; }
        public VkAttachmentLoadOp LoadOp { get; set; }
        public VkAttachmentStoreOp StoreOp { get; set; }
        public VkAttachmentLoadOp StencilLoadOp { get; set; }
        public VkAttachmentStoreOp StencilStoreOp { get; set; }
        public VkImageLayout InitialLayout { get; set; }
        public VkImageLayout FinalLayout { get; set; }
    }

    public static unsafe class VkAttachmentDescriptionExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkAttachmentDescription> list) =>
            list.SizeOfMarshalDirect(sizeof(VkAttachmentDescription), x => 0);

        public static VkAttachmentDescription* MarshalDirect(this IReadOnlyList<VkAttachmentDescription> list, ref byte* unmanaged) =>
            (VkAttachmentDescription*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkAttachmentDescription*)dst = elem; }, sizeof(VkAttachmentDescription));
    }
}
