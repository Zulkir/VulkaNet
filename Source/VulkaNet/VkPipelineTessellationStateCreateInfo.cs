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
    public unsafe class VkPipelineTessellationStateCreateInfo
    {
        public IVkStructWrapper Next { get; set; }
        public VkPipelineTessellationStateCreateFlags Flags { get; set; }
        public int PatchControlPoints { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkPipelineTessellationStateCreateFlags flags;
            public int patchControlPoints;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPipelineTessellationStateCreateInfoExtensions
    {
        public static int SizeOfMarshalDirect(this VkPipelineTessellationStateCreateInfo s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect();
        }

        public static VkPipelineTessellationStateCreateInfo.Raw MarshalDirect(this VkPipelineTessellationStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);

            VkPipelineTessellationStateCreateInfo.Raw result;
            result.sType = VkStructureType.PipelineTessellationStateCreateInfo;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.patchControlPoints = s.PatchControlPoints;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPipelineTessellationStateCreateInfo s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPipelineTessellationStateCreateInfo.Raw.SizeInBytes;

        public static VkPipelineTessellationStateCreateInfo.Raw* MarshalIndirect(this VkPipelineTessellationStateCreateInfo s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPipelineTessellationStateCreateInfo.Raw*)0;
            var result = (VkPipelineTessellationStateCreateInfo.Raw*)unmanaged;
            unmanaged += VkPipelineTessellationStateCreateInfo.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPipelineTessellationStateCreateInfo> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPipelineTessellationStateCreateInfo.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPipelineTessellationStateCreateInfo.Raw* MarshalDirect(this IReadOnlyList<VkPipelineTessellationStateCreateInfo> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPipelineTessellationStateCreateInfo.Raw*)0;
            var result = (VkPipelineTessellationStateCreateInfo.Raw*)unmanaged;
            unmanaged += sizeof(VkPipelineTessellationStateCreateInfo.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
