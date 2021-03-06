﻿#region License
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
    public struct VkRect2D
    {
        public VkOffset2D Offset;
        public VkExtent2D Extent;

        public VkRect2D(VkOffset2D offset, VkExtent2D extent)
        {
            Offset = offset;
            Extent = extent;
        }
    }

    public static unsafe class VkRect2DExtensions
    {
        public static int SizeOfMarshalDirect(this IReadOnlyList<VkRect2D> list) =>
            list.SizeOfMarshalDirect(sizeof(VkRect2D), x => 0);

        public static VkRect2D* MarshalDirect(this IReadOnlyList<VkRect2D> list, ref byte* unmanaged) =>
            (VkRect2D*)list.MarshalDirect(ref unmanaged, (elem, dst) => { *(VkRect2D*)dst = elem; }, sizeof(VkRect2D));
    }
}
