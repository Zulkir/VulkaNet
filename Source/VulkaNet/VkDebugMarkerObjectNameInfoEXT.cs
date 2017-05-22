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
    public unsafe class VkDebugMarkerObjectNameInfoEXT
    {
        public IVkStructWrapper Next { get; set; }
        public VkDebugReportObjectTypeEXT ObjectType { get; set; }
        public ulong VkObject { get; set; }
        public string ObjectName { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDebugReportObjectTypeEXT objectType;
            public ulong vkObject;
            public byte* pObjectName;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDebugMarkerObjectNameInfoEXTExtensions
    {
        public static int SizeOfMarshalDirect(this VkDebugMarkerObjectNameInfoEXT s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.ObjectName.SizeOfMarshalIndirect();
        }

        public static VkDebugMarkerObjectNameInfoEXT.Raw MarshalDirect(this VkDebugMarkerObjectNameInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pObjectName = s.ObjectName.MarshalIndirect(ref unmanaged);

            VkDebugMarkerObjectNameInfoEXT.Raw result;
            result.sType = VkStructureType.DebugMarkerObjectNameInfoEXT;
            result.pNext = pNext;
            result.objectType = s.ObjectType;
            result.vkObject = s.VkObject;
            result.pObjectName = pObjectName;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDebugMarkerObjectNameInfoEXT s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDebugMarkerObjectNameInfoEXT.Raw.SizeInBytes;

        public static VkDebugMarkerObjectNameInfoEXT.Raw* MarshalIndirect(this VkDebugMarkerObjectNameInfoEXT s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDebugMarkerObjectNameInfoEXT.Raw*)0;
            var result = (VkDebugMarkerObjectNameInfoEXT.Raw*)unmanaged;
            unmanaged += VkDebugMarkerObjectNameInfoEXT.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDebugMarkerObjectNameInfoEXT> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDebugMarkerObjectNameInfoEXT.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDebugMarkerObjectNameInfoEXT.Raw* MarshalDirect(this IReadOnlyList<VkDebugMarkerObjectNameInfoEXT> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDebugMarkerObjectNameInfoEXT.Raw*)0;
            var result = (VkDebugMarkerObjectNameInfoEXT.Raw*)unmanaged;
            unmanaged += sizeof(VkDebugMarkerObjectNameInfoEXT.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
