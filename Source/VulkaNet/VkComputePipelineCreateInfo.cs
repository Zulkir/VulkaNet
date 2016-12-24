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
    public interface IVkComputePipelineCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineCreateFlags Flags { get; }
        IVkPipelineShaderStageCreateInfo Stage { get; }
        IVkPipelineLayout Layout { get; }
        IVkPipeline BasePipelineHandle { get; }
        int BasePipelineIndex { get; }
    }

    public unsafe class VkComputePipelineCreateInfo : IVkComputePipelineCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineCreateFlags Flags { get; set; }
        public IVkPipelineShaderStageCreateInfo Stage { get; set; }
        public IVkPipelineLayout Layout { get; set; }
        public IVkPipeline BasePipelineHandle { get; set; }
        public int BasePipelineIndex { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineCreateFlags flags;
            public VkPipelineShaderStageCreateInfo.Raw stage;
            public VkPipelineLayout.HandleType layout;
            public VkPipeline.HandleType basePipelineHandle;
            public int basePipelineIndex;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkComputePipelineCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkComputePipelineCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Stage.SizeOfMarshalDirect();
        }

        public static VkComputePipelineCreateInfo.Raw MarshalDirect(this IVkComputePipelineCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var stage = s.Stage.MarshalDirect(ref unmanaged);

            VkComputePipelineCreateInfo.Raw result;
            result.sType = VkStructureType.ComputePipelineCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.stage = stage;
            result.layout = s.Layout?.Handle ?? VkPipelineLayout.HandleType.Null;
            result.basePipelineHandle = s.BasePipelineHandle?.Handle ?? VkPipeline.HandleType.Null;
            result.basePipelineIndex = s.BasePipelineIndex;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkComputePipelineCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkComputePipelineCreateInfo.Raw.SizeInBytes;

        public static VkComputePipelineCreateInfo.Raw* MarshalIndirect(this IVkComputePipelineCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkComputePipelineCreateInfo.Raw*)0;
            var result = (VkComputePipelineCreateInfo.Raw*)unmanaged;
            unmanaged += VkComputePipelineCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkComputePipelineCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkComputePipelineCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkComputePipelineCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkComputePipelineCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkComputePipelineCreateInfo.Raw*)0;
            var result = (VkComputePipelineCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkComputePipelineCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkComputePipelineCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkComputePipelineCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkComputePipelineCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkComputePipelineCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkComputePipelineCreateInfo.Raw**)0;
            var result = (VkComputePipelineCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkComputePipelineCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
