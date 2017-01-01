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
    public struct VkImageBlit
    {
        public VkImageSubresourceLayers SrcSubresource;
        public VkOffset3D SrcOffsetsStart;
        public VkOffset3D SrcOffsetsEnd;
        public VkImageSubresourceLayers DstSubresource;
        public VkOffset3D DstOffsetsStart;
        public VkOffset3D DstOffsetsEnd;
    }

    public static unsafe class VkImageBlitExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkImageBlit> list) =>
            list.SizeOfMarshalDirect(sizeof(VkImageBlit), x => 0);

        public static VkImageBlit* MarshalDirect(this IReadOnlyList<VkImageBlit> list, ref byte* unmanaged) =>
            (VkImageBlit*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkImageBlit*)dst = elem; }, sizeof(VkImageBlit));
    }
}