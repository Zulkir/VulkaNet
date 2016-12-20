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
    public interface IVkApplicationInfo
    {
        IVkStructWrapper Next { get; }
        string ApplicationName { get; }
        uint ApplicationVersion { get; }
        string EngineName { get; }
        uint EngineVersion { get; }
        VkApiVersion ApiVersion { get; }
    }

    public unsafe class VkApplicationInfo : IVkApplicationInfo
    {
        public IVkStructWrapper Next { get; set; }
        public string ApplicationName { get; set; }
        public uint ApplicationVersion { get; set; }
        public string EngineName { get; set; }
        public uint EngineVersion { get; set; }
        public VkApiVersion ApiVersion { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public byte* pApplicationName;
            public uint applicationVersion;
            public byte* pEngineName;
            public uint engineVersion;
            public VkApiVersion apiVersion;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkApplicationInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkApplicationInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.ApplicationName.SizeOfMarshalIndirect() +
                s.EngineName.SizeOfMarshalIndirect();
        }

        public static VkApplicationInfo.Raw MarshalDirect(this IVkApplicationInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pApplicationName = s.ApplicationName.MarshalIndirect(ref unmanaged);
            var pEngineName = s.EngineName.MarshalIndirect(ref unmanaged);

            VkApplicationInfo.Raw result;
            result.sType = VkStructureType.ApplicationInfo;
            result.pNext = pNext;
            result.pApplicationName = pApplicationName;
            result.applicationVersion = s.ApplicationVersion;
            result.pEngineName = pEngineName;
            result.engineVersion = s.EngineVersion;
            result.apiVersion = s.ApiVersion;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkApplicationInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkApplicationInfo.Raw.SizeInBytes;

        public static VkApplicationInfo.Raw* MarshalIndirect(this IVkApplicationInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkApplicationInfo.Raw*)0;
            var result = (VkApplicationInfo.Raw*)unmanaged;
            unmanaged += VkApplicationInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkApplicationInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkApplicationInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkApplicationInfo.Raw* MarshalDirect(this IReadOnlyList<IVkApplicationInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkApplicationInfo.Raw*)0;
            var result = (VkApplicationInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkApplicationInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkApplicationInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkApplicationInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkApplicationInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkApplicationInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkApplicationInfo.Raw**)0;
            var result = (VkApplicationInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkApplicationInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
