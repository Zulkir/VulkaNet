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
    public unsafe class VkDebugMarkerObjectTagInfoEXT
    {
        public IVkStructWrapper Next { get; set; }
        public VkDebugReportObjectTypeEXT ObjectType { get; set; }
        public ulong VkObject { get; set; }
        public ulong TagName { get; set; }
        public IntPtr TagSize { get; set; }
        public IntPtr Tag { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDebugReportObjectTypeEXT objectType;
            public ulong vkObject;
            public ulong tagName;
            public IntPtr tagSize;
            public IntPtr pTag;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDebugMarkerObjectTagInfoEXTExtensions
    {
        public static int SizeOfMarshalDirect(this VkDebugMarkerObjectTagInfoEXT s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkDebugMarkerObjectTagInfoEXT.Raw MarshalDirect(this VkDebugMarkerObjectTagInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkDebugMarkerObjectTagInfoEXT.Raw result;
            result.sType = VkStructureType.DebugMarkerObjectTagInfoEXT;
            result.pNext = pNext;
            result.objectType = s.ObjectType;
            result.vkObject = s.VkObject;
            result.tagName = s.TagName;
            result.tagSize = s.TagSize;
            result.pTag = s.Tag;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDebugMarkerObjectTagInfoEXT s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDebugMarkerObjectTagInfoEXT.Raw.SizeInBytes;

        public static VkDebugMarkerObjectTagInfoEXT.Raw* MarshalIndirect(this VkDebugMarkerObjectTagInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDebugMarkerObjectTagInfoEXT.Raw*)0;
            var result = (VkDebugMarkerObjectTagInfoEXT.Raw*)unmanaged;
            unmanaged += VkDebugMarkerObjectTagInfoEXT.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDebugMarkerObjectTagInfoEXT> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDebugMarkerObjectTagInfoEXT.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDebugMarkerObjectTagInfoEXT.Raw* MarshalDirect(this IReadOnlyList<VkDebugMarkerObjectTagInfoEXT> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDebugMarkerObjectTagInfoEXT.Raw*)0;
            var result = (VkDebugMarkerObjectTagInfoEXT.Raw*)unmanaged;
            unmanaged += sizeof(VkDebugMarkerObjectTagInfoEXT.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
