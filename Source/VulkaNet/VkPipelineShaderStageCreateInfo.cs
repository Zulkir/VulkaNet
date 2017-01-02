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
    public unsafe class VkPipelineShaderStageCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineShaderStageCreateFlags Flags { get; set; }
        public VkShaderStageFlagBits Stage { get; set; }
        public IVkShaderModule Module { get; set; }
        public string Name { get; set; }
        public VkSpecializationInfo SpecializationInfo { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineShaderStageCreateFlags flags;
            public VkShaderStageFlagBits stage;
            public VkShaderModule.HandleType module;
            public byte* pName;
            public VkSpecializationInfo.Raw* pSpecializationInfo;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineShaderStageCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineShaderStageCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Name.SizeOfMarshalIndirect() +
                s.SpecializationInfo.SizeOfMarshalIndirect();
        }

        public static VkPipelineShaderStageCreateInfo.Raw MarshalDirect(this VkPipelineShaderStageCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pName = s.Name.MarshalIndirect(ref unmanaged);
            var pSpecializationInfo = s.SpecializationInfo.MarshalIndirect(ref unmanaged);

            VkPipelineShaderStageCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineShaderStageCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.stage = s.Stage;
            result.module = s.Module?.Handle ?? VkShaderModule.HandleType.Null;
            result.pName = pName;
            result.pSpecializationInfo = pSpecializationInfo;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineShaderStageCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineShaderStageCreateInfo.Raw.SizeInBytes;

        public static VkPipelineShaderStageCreateInfo.Raw* MarshalIndirect(this VkPipelineShaderStageCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineShaderStageCreateInfo.Raw*)0;
            var result = (VkPipelineShaderStageCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineShaderStageCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineShaderStageCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineShaderStageCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineShaderStageCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineShaderStageCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineShaderStageCreateInfo.Raw*)0;
            var result = (VkPipelineShaderStageCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineShaderStageCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

    }
}
