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
    public unsafe struct VkSubpassDependency
    {
        public int SrcSubpass { get; set; }
        public int DstSubpass { get; set; }
        public VkPipelineStageFlags SrcStageMask { get; set; }
        public VkPipelineStageFlags DstStageMask { get; set; }
        public VkAccessFlags SrcAccessMask { get; set; }
        public VkAccessFlags DstAccessMask { get; set; }
        public VkDependencyFlags DependencyFlags { get; set; }
    }

    public static unsafe class VkSubpassDependencyExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSubpassDependency> list) =>
            list.SizeOfMarshalDirect(sizeof(VkSubpassDependency), x => 0);

        public static VkSubpassDependency* MarshalDirect(this IReadOnlyList<VkSubpassDependency> list, ref byte* unmanaged) =>
            (VkSubpassDependency*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkSubpassDependency*)dst = elem; }, sizeof(VkSubpassDependency));
    }
}
