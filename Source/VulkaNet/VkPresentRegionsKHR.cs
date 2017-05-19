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
    public unsafe class VkPresentRegionsKHR
    {
        public IVkStructWrapper Next { get; set; }
        public IReadOnlyList<VkPresentRegionKHR> Regions { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public int swapchainCount;
            public VkPresentRegionKHR.Raw* pRegions;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPresentRegionsKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkPresentRegionsKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Regions.SizeOfMarshalDirect();
        }

        public static VkPresentRegionsKHR.Raw MarshalDirect(this VkPresentRegionsKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pRegions = s.Regions.MarshalDirect(ref unmanaged);

            VkPresentRegionsKHR.Raw result;
            result.sType = VkStructureType.PresentRegionsKHR;
            result.pNext = pNext;
            result.swapchainCount = s.Regions?.Count ?? 0;
            result.pRegions = pRegions;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPresentRegionsKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPresentRegionsKHR.Raw.SizeInBytes;

        public static VkPresentRegionsKHR.Raw* MarshalIndirect(this VkPresentRegionsKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPresentRegionsKHR.Raw*)0;
            var result = (VkPresentRegionsKHR.Raw*)unmanaged;
            unmanaged += VkPresentRegionsKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPresentRegionsKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPresentRegionsKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPresentRegionsKHR.Raw* MarshalDirect(this IReadOnlyList<VkPresentRegionsKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPresentRegionsKHR.Raw*)0;
            var result = (VkPresentRegionsKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkPresentRegionsKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
