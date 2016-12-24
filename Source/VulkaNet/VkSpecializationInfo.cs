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
    public interface IVkSpecializationInfo
    {
        IReadOnlyList<VkSpecializationMapEntry> MapEntries { get; }
        int DataSize { get; }
        IntPtr Data { get; }
    }

    public unsafe class VkSpecializationInfo : IVkSpecializationInfo
    {
        public IReadOnlyList<VkSpecializationMapEntry> MapEntries { get; set; }
        public int DataSize { get; set; }
        public IntPtr Data { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public int mapEntryCount;
            public VkSpecializationMapEntry* pMapEntries;
            public IntPtr dataSize;
            public IntPtr pData;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSpecializationInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkSpecializationInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.MapEntries.SizeOfMarshalDirect();
        }

        public static VkSpecializationInfo.Raw MarshalDirect(this IVkSpecializationInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pMapEntries = s.MapEntries.MarshalDirect(ref unmanaged);

            VkSpecializationInfo.Raw result;
            result.mapEntryCount = s.MapEntries?.Count ?? 0;
            result.pMapEntries = pMapEntries;
            result.dataSize = (IntPtr)s.DataSize;
            result.pData = s.Data;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkSpecializationInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSpecializationInfo.Raw.SizeInBytes;

        public static VkSpecializationInfo.Raw* MarshalIndirect(this IVkSpecializationInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSpecializationInfo.Raw*)0;
            var result = (VkSpecializationInfo.Raw*)unmanaged;
            unmanaged += VkSpecializationInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkSpecializationInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSpecializationInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSpecializationInfo.Raw* MarshalDirect(this IReadOnlyList<IVkSpecializationInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSpecializationInfo.Raw*)0;
            var result = (VkSpecializationInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkSpecializationInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkSpecializationInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkSpecializationInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkSpecializationInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkSpecializationInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSpecializationInfo.Raw**)0;
            var result = (VkSpecializationInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkSpecializationInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
