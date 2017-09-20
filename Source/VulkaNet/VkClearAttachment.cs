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
using System.Runtime.InteropServices;

namespace VulkaNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VkClearAttachment
    {
        public VkImageAspectFlags AspectMask;
        public int ColorAttachment;
        public VkClearValue ClearValue;
    }

    public static unsafe class VkClearAttachmentExstensions
    {
        public static int SizeOfMarshalIndirect(this VkClearAttachment s) =>
            sizeof(VkClearAttachment);

        public static int SizeOfMarshalIndirect(this VkClearAttachment? s) =>
            s?.SizeOfMarshalIndirect() ?? 0;

        public static VkClearAttachment* MarshalIndirect(this VkClearAttachment s, ref byte* unmanaged)
        {
            var result = (VkClearAttachment*)unmanaged;
            unmanaged += sizeof(VkClearAttachment);
            *result = s;
            return result;
        }

        public static VkClearAttachment* MarshalIndirect(this VkClearAttachment? s, ref byte* unmanaged) =>
            s.HasValue ? s.Value.MarshalIndirect(ref unmanaged) : null;

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkClearAttachment> list) =>
            list.SizeOfMarshalDirect(sizeof(VkClearAttachment), x => 0);

        public static VkClearAttachment* MarshalDirect(this IReadOnlyList<VkClearAttachment> list, ref byte* unmanaged) =>
            (VkClearAttachment*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkClearAttachment*)dst = elem; }, sizeof(VkClearAttachment));
    }
}