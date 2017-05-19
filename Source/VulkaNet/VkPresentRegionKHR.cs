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
using System.Linq;
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public unsafe struct VkPresentRegionKHR
    {
        public IReadOnlyList<VkRectLayerKHR> Rectangles { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public int rectangleCount;
            public VkRectLayerKHR* pRectangles;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPresentRegionKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkPresentRegionKHR s)
        {
            return
                s.Rectangles.SizeOfMarshalDirect();
        }

        public static VkPresentRegionKHR.Raw MarshalDirect(this VkPresentRegionKHR s, ref byte* unmanaged)
        {
            var pRectangles = s.Rectangles.MarshalDirect(ref unmanaged);

            VkPresentRegionKHR.Raw result;
            result.rectangleCount = s.Rectangles?.Count ?? 0;
            result.pRectangles = pRectangles;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPresentRegionKHR s) =>
            s.SizeOfMarshalDirect() + VkPresentRegionKHR.Raw.SizeInBytes;

        public static VkPresentRegionKHR.Raw* MarshalIndirect(this VkPresentRegionKHR s, ref byte* unmanaged)
        {
            var result = (VkPresentRegionKHR.Raw*)unmanaged;
            unmanaged += VkPresentRegionKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPresentRegionKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPresentRegionKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPresentRegionKHR.Raw* MarshalDirect(this IReadOnlyList<VkPresentRegionKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPresentRegionKHR.Raw*)0;
            var result = (VkPresentRegionKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkPresentRegionKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
