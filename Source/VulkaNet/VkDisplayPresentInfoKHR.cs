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
    public unsafe class VkDisplayPresentInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public VkRect2D SrcRect { get; set; }
        public VkRect2D DstRect { get; set; }
        public bool Persistent { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkRect2D srcRect;
            public VkRect2D dstRect;
            public VkBool32 persistent;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDisplayPresentInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkDisplayPresentInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkDisplayPresentInfoKHR.Raw MarshalDirect(this VkDisplayPresentInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkDisplayPresentInfoKHR.Raw result;
            result.sType = VkStructureType.DisplayPresentInfoKHR;
            result.pNext = pNext;
            result.srcRect = s.SrcRect;
            result.dstRect = s.DstRect;
            result.persistent = new VkBool32(s.Persistent);
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDisplayPresentInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDisplayPresentInfoKHR.Raw.SizeInBytes;

        public static VkDisplayPresentInfoKHR.Raw* MarshalIndirect(this VkDisplayPresentInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDisplayPresentInfoKHR.Raw*)0;
            var result = (VkDisplayPresentInfoKHR.Raw*)unmanaged;
            unmanaged += VkDisplayPresentInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDisplayPresentInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDisplayPresentInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDisplayPresentInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkDisplayPresentInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDisplayPresentInfoKHR.Raw*)0;
            var result = (VkDisplayPresentInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkDisplayPresentInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
