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
    public interface IVkQueryPoolCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkQueryPoolCreateFlags Flags { get; }
        VkQueryType QueryType { get; }
        int QueryCount { get; }
        VkQueryPipelineStatisticFlags PipelineStatistics { get; }
    }

    public unsafe class VkQueryPoolCreateInfo : IVkQueryPoolCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkQueryPoolCreateFlags Flags { get; set; }
        public VkQueryType QueryType { get; set; }
        public int QueryCount { get; set; }
        public VkQueryPipelineStatisticFlags PipelineStatistics { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkQueryPoolCreateFlags flags;
            public VkQueryType queryType;
            public int queryCount;
            public VkQueryPipelineStatisticFlags pipelineStatistics;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkQueryPoolCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkQueryPoolCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkQueryPoolCreateInfo.Raw MarshalDirect(this IVkQueryPoolCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkQueryPoolCreateInfo.Raw result;
            result.sType = VkStructureType.QueryPoolCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.queryType = s.QueryType;
            result.queryCount = s.QueryCount;
            result.pipelineStatistics = s.PipelineStatistics;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkQueryPoolCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkQueryPoolCreateInfo.Raw.SizeInBytes;

        public static VkQueryPoolCreateInfo.Raw* MarshalIndirect(this IVkQueryPoolCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkQueryPoolCreateInfo.Raw*)0;
            var result = (VkQueryPoolCreateInfo.Raw*)unmanaged;
            unmanaged += VkQueryPoolCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkQueryPoolCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkQueryPoolCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkQueryPoolCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkQueryPoolCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkQueryPoolCreateInfo.Raw*)0;
            var result = (VkQueryPoolCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkQueryPoolCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkQueryPoolCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkQueryPoolCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkQueryPoolCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkQueryPoolCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkQueryPoolCreateInfo.Raw**)0;
            var result = (VkQueryPoolCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkQueryPoolCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
