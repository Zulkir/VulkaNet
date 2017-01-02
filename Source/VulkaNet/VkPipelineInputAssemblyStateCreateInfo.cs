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
    public unsafe class VkPipelineInputAssemblyStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineInputAssemblyStateCreateFlags Flags { get; set; }
        public VkPrimitiveTopology Topology { get; set; }
        public bool PrimitiveRestartEnable { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineInputAssemblyStateCreateFlags flags;
            public VkPrimitiveTopology topology;
            public VkBool32 primitiveRestartEnable;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineInputAssemblyStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineInputAssemblyStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkPipelineInputAssemblyStateCreateInfo.Raw MarshalDirect(this VkPipelineInputAssemblyStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkPipelineInputAssemblyStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineInputAssemblyStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.topology = s.Topology;
            result.primitiveRestartEnable = new VkBool32(s.PrimitiveRestartEnable);
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineInputAssemblyStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineInputAssemblyStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineInputAssemblyStateCreateInfo.Raw* MarshalIndirect(this VkPipelineInputAssemblyStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineInputAssemblyStateCreateInfo.Raw*)0;
            var result = (VkPipelineInputAssemblyStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineInputAssemblyStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineInputAssemblyStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineInputAssemblyStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineInputAssemblyStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineInputAssemblyStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineInputAssemblyStateCreateInfo.Raw*)0;
            var result = (VkPipelineInputAssemblyStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineInputAssemblyStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
