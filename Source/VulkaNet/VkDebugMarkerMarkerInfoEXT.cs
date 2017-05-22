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
    public unsafe class VkDebugMarkerMarkerInfoEXT
    {
        public IVkStructWrapper Next { get; set; }
        public string MarkerName { get; set; }
        public VkColor4 Color { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public byte* pMarkerName;
            public VkColor4 color;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDebugMarkerMarkerInfoEXTExtensions
    {
        public static int SizeOfMarshalDirect(this VkDebugMarkerMarkerInfoEXT s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.MarkerName.SizeOfMarshalIndirect();
        }

        public static VkDebugMarkerMarkerInfoEXT.Raw MarshalDirect(this VkDebugMarkerMarkerInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pMarkerName = s.MarkerName.MarshalIndirect(ref unmanaged);

            VkDebugMarkerMarkerInfoEXT.Raw result;
            result.sType = VkStructureType.DebugMarkerMarkerInfoEXT;
            result.pNext = pNext;
            result.pMarkerName = pMarkerName;
            result.color = s.Color;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDebugMarkerMarkerInfoEXT s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDebugMarkerMarkerInfoEXT.Raw.SizeInBytes;

        public static VkDebugMarkerMarkerInfoEXT.Raw* MarshalIndirect(this VkDebugMarkerMarkerInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDebugMarkerMarkerInfoEXT.Raw*)0;
            var result = (VkDebugMarkerMarkerInfoEXT.Raw*)unmanaged;
            unmanaged += VkDebugMarkerMarkerInfoEXT.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDebugMarkerMarkerInfoEXT> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDebugMarkerMarkerInfoEXT.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDebugMarkerMarkerInfoEXT.Raw* MarshalDirect(this IReadOnlyList<VkDebugMarkerMarkerInfoEXT> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDebugMarkerMarkerInfoEXT.Raw*)0;
            var result = (VkDebugMarkerMarkerInfoEXT.Raw*)unmanaged;
            unmanaged += sizeof(VkDebugMarkerMarkerInfoEXT.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
