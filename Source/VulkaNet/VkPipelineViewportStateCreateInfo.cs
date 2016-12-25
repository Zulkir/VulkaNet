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
    public interface IVkPipelineViewportStateCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineViewportStateCreateFlags Flags { get; }
        IReadOnlyList<VkViewport> Viewports { get; }
        IReadOnlyList<VkRect2D> Scissors { get; }
    }

    public unsafe class VkPipelineViewportStateCreateInfo : IVkPipelineViewportStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineViewportStateCreateFlags Flags { get; set; }
        public IReadOnlyList<VkViewport> Viewports { get; set; }
        public IReadOnlyList<VkRect2D> Scissors { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineViewportStateCreateFlags flags;
            public int viewportCount;
            public VkViewport* pViewports;
            public int scissorCount;
            public VkRect2D* pScissors;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineViewportStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkPipelineViewportStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Viewports.SizeOfMarshalDirect() +
                s.Scissors.SizeOfMarshalDirect();
        }

        public static VkPipelineViewportStateCreateInfo.Raw MarshalDirect(this IVkPipelineViewportStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pViewports = s.Viewports.MarshalDirect(ref unmanaged);
            var pScissors = s.Scissors.MarshalDirect(ref unmanaged);

            VkPipelineViewportStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineViewportStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.viewportCount = s.Viewports?.Count ?? 0;
            result.pViewports = pViewports;
            result.scissorCount = s.Scissors?.Count ?? 0;
            result.pScissors = pScissors;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkPipelineViewportStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineViewportStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineViewportStateCreateInfo.Raw* MarshalIndirect(this IVkPipelineViewportStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineViewportStateCreateInfo.Raw*)0;
            var result = (VkPipelineViewportStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineViewportStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkPipelineViewportStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineViewportStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineViewportStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkPipelineViewportStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineViewportStateCreateInfo.Raw*)0;
            var result = (VkPipelineViewportStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineViewportStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkPipelineViewportStateCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkPipelineViewportStateCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkPipelineViewportStateCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkPipelineViewportStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineViewportStateCreateInfo.Raw**)0;
            var result = (VkPipelineViewportStateCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkPipelineViewportStateCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
