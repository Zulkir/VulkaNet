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
    public unsafe class VkDisplaySurfaceCreateInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public VkDisplaySurfaceCreateFlagsKHR Flags { get; set; }
        public IVkDisplayModeKHR DisplayMode { get; set; }
        public int PlaneIndex { get; set; }
        public int PlaneStackIndex { get; set; }
        public VkSurfaceTransformFlagBitsKHR Transform { get; set; }
        public float GlobalAlpha { get; set; }
        public VkDisplayPlaneAlphaFlagBitsKHR AlphaMode { get; set; }
        public VkExtent2D ImageExtent { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkDisplaySurfaceCreateFlagsKHR flags;
            public VkDisplayModeKHR.HandleType displayMode;
            public int planeIndex;
            public int planeStackIndex;
            public VkSurfaceTransformFlagBitsKHR transform;
            public float globalAlpha;
            public VkDisplayPlaneAlphaFlagBitsKHR alphaMode;
            public VkExtent2D imageExtent;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDisplaySurfaceCreateInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkDisplaySurfaceCreateInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkDisplaySurfaceCreateInfoKHR.Raw MarshalDirect(this VkDisplaySurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkDisplaySurfaceCreateInfoKHR.Raw result;
            result.sType = VkStructureType.DisplaySurfaceCreateInfoKHR;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.displayMode = s.DisplayMode?.Handle ?? VkDisplayModeKHR.HandleType.Null;
            result.planeIndex = s.PlaneIndex;
            result.planeStackIndex = s.PlaneStackIndex;
            result.transform = s.Transform;
            result.globalAlpha = s.GlobalAlpha;
            result.alphaMode = s.AlphaMode;
            result.imageExtent = s.ImageExtent;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkDisplaySurfaceCreateInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDisplaySurfaceCreateInfoKHR.Raw.SizeInBytes;

        public static VkDisplaySurfaceCreateInfoKHR.Raw* MarshalIndirect(this VkDisplaySurfaceCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDisplaySurfaceCreateInfoKHR.Raw*)0;
            var result = (VkDisplaySurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += VkDisplaySurfaceCreateInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkDisplaySurfaceCreateInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDisplaySurfaceCreateInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDisplaySurfaceCreateInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkDisplaySurfaceCreateInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDisplaySurfaceCreateInfoKHR.Raw*)0;
            var result = (VkDisplaySurfaceCreateInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkDisplaySurfaceCreateInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
