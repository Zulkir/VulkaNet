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
    public unsafe class VkPipelineMultisampleStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineMultisampleStateCreateFlags Flags { get; set; }
        public VkSampleCountFlagBits RasterizationSamples { get; set; }
        public bool SampleShadingEnable { get; set; }
        public float MinSampleShading { get; set; }
        public IReadOnlyList<int> SampleMask { get; set; }
        public bool AlphaToCoverageEnable { get; set; }
        public bool AlphaToOneEnable { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineMultisampleStateCreateFlags flags;
            public VkSampleCountFlagBits rasterizationSamples;
            public VkBool32 sampleShadingEnable;
            public float minSampleShading;
            public int* pSampleMask;
            public VkBool32 alphaToCoverageEnable;
            public VkBool32 alphaToOneEnable;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineMultisampleStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineMultisampleStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.SampleMask.SizeOfMarshalDirect();
        }

        public static VkPipelineMultisampleStateCreateInfo.Raw MarshalDirect(this VkPipelineMultisampleStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pSampleMask = s.SampleMask.MarshalDirect(ref unmanaged);

            VkPipelineMultisampleStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineMultisampleStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.rasterizationSamples = s.RasterizationSamples;
            result.sampleShadingEnable = new VkBool32(s.SampleShadingEnable);
            result.minSampleShading = s.MinSampleShading;
            result.pSampleMask = pSampleMask;
            result.alphaToCoverageEnable = new VkBool32(s.AlphaToCoverageEnable);
            result.alphaToOneEnable = new VkBool32(s.AlphaToOneEnable);
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineMultisampleStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineMultisampleStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineMultisampleStateCreateInfo.Raw* MarshalIndirect(this VkPipelineMultisampleStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineMultisampleStateCreateInfo.Raw*)0;
            var result = (VkPipelineMultisampleStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineMultisampleStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineMultisampleStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineMultisampleStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineMultisampleStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineMultisampleStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineMultisampleStateCreateInfo.Raw*)0;
            var result = (VkPipelineMultisampleStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineMultisampleStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
