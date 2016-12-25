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
    public interface IVkPipelineColorBlendStateCreateInfo
    {
        IVkStructWrapper Next { get; }
        VkPipelineColorBlendStateCreateFlags Flags { get; }
        bool LogicOpEnable { get; }
        VkLogicOp LogicOp { get; }
        IReadOnlyList<VkPipelineColorBlendAttachmentState> Attachments { get; }
        VkColor4 BlendConstants { get; }
    }

    public unsafe class VkPipelineColorBlendStateCreateInfo : IVkPipelineColorBlendStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineColorBlendStateCreateFlags Flags { get; set; }
        public bool LogicOpEnable { get; set; }
        public VkLogicOp LogicOp { get; set; }
        public IReadOnlyList<VkPipelineColorBlendAttachmentState> Attachments { get; set; }
        public VkColor4 BlendConstants { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineColorBlendStateCreateFlags flags;
            public VkBool32 logicOpEnable;
            public VkLogicOp logicOp;
            public int attachmentCount;
            public VkPipelineColorBlendAttachmentState* pAttachments;
            public VkColor4 blendConstants;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineColorBlendStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this IVkPipelineColorBlendStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.Attachments.SizeOfMarshalDirect();
        }

        public static VkPipelineColorBlendStateCreateInfo.Raw MarshalDirect(this IVkPipelineColorBlendStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pAttachments = s.Attachments.MarshalDirect(ref unmanaged);

            VkPipelineColorBlendStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineColorBlendStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.logicOpEnable = new VkBool32(s.LogicOpEnable);
            result.logicOp = s.LogicOp;
            result.attachmentCount = s.Attachments?.Count ?? 0;
            result.pAttachments = pAttachments;
            result.blendConstants = s.BlendConstants;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkPipelineColorBlendStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineColorBlendStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineColorBlendStateCreateInfo.Raw* MarshalIndirect(this IVkPipelineColorBlendStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineColorBlendStateCreateInfo.Raw*)0;
            var result = (VkPipelineColorBlendStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineColorBlendStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkPipelineColorBlendStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineColorBlendStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineColorBlendStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<IVkPipelineColorBlendStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineColorBlendStateCreateInfo.Raw*)0;
            var result = (VkPipelineColorBlendStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineColorBlendStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkPipelineColorBlendStateCreateInfo> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkPipelineColorBlendStateCreateInfo.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkPipelineColorBlendStateCreateInfo.Raw** MarshalIndirect(this IReadOnlyList<IVkPipelineColorBlendStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineColorBlendStateCreateInfo.Raw**)0;
            var result = (VkPipelineColorBlendStateCreateInfo.Raw**)unmanaged;
            unmanaged += sizeof(VkPipelineColorBlendStateCreateInfo.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
