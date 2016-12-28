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
    public interface IVkDescriptorSetLayoutBinding
    {
        int Binding { get; }
        VkDescriptorType DescriptorType { get; }
        int DescriptorCount { get; }
        VkShaderStageFlagBits StageFlags { get; }
        IReadOnlyList<IVkSampler> ImmutableSamplers { get; }
    }

    public unsafe class VkDescriptorSetLayoutBinding : IVkDescriptorSetLayoutBinding
    {
        public int Binding { get; set; }
        public VkDescriptorType DescriptorType { get; set; }
        public int DescriptorCount { get; set; }
        public VkShaderStageFlagBits StageFlags { get; set; }
        public IReadOnlyList<IVkSampler> ImmutableSamplers { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public int binding;
            public VkDescriptorType descriptorType;
            public int descriptorCount;
            public VkShaderStageFlagBits stageFlags;
            public VkSampler.HandleType* pImmutableSamplers;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkDescriptorSetLayoutBindingExtensions
    {
        public static int SizeOfMarshalDirect(this IVkDescriptorSetLayoutBinding s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.ImmutableSamplers.SizeOfMarshalDirect();
        }

        public static VkDescriptorSetLayoutBinding.Raw MarshalDirect(this IVkDescriptorSetLayoutBinding s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pImmutableSamplers = s.ImmutableSamplers.MarshalDirect(ref unmanaged);

            VkDescriptorSetLayoutBinding.Raw result;
            result.binding = s.Binding;
            result.descriptorType = s.DescriptorType;
            result.descriptorCount = s.DescriptorCount;
            result.stageFlags = s.StageFlags;
            result.pImmutableSamplers = pImmutableSamplers;
            return result;
        }

        public static int SizeOfMarshalIndirect(this IVkDescriptorSetLayoutBinding s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkDescriptorSetLayoutBinding.Raw.SizeInBytes;

        public static VkDescriptorSetLayoutBinding.Raw* MarshalIndirect(this IVkDescriptorSetLayoutBinding s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkDescriptorSetLayoutBinding.Raw*)0;
            var result = (VkDescriptorSetLayoutBinding.Raw*)unmanaged;
            unmanaged += VkDescriptorSetLayoutBinding.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<IVkDescriptorSetLayoutBinding> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkDescriptorSetLayoutBinding.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkDescriptorSetLayoutBinding.Raw* MarshalDirect(this IReadOnlyList<IVkDescriptorSetLayoutBinding> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorSetLayoutBinding.Raw*)0;
            var result = (VkDescriptorSetLayoutBinding.Raw*)unmanaged;
            unmanaged += sizeof(VkDescriptorSetLayoutBinding.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalIndirect(this IReadOnlyList<IVkDescriptorSetLayoutBinding> list) =>
            list == null || list.Count == 0
                ? 0
                : sizeof(VkDescriptorSetLayoutBinding.Raw*) * list.Count + list.Sum(x => x.SizeOfMarshalIndirect());

        public static VkDescriptorSetLayoutBinding.Raw** MarshalIndirect(this IReadOnlyList<IVkDescriptorSetLayoutBinding> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkDescriptorSetLayoutBinding.Raw**)0;
            var result = (VkDescriptorSetLayoutBinding.Raw**)unmanaged;
            unmanaged += sizeof(VkDescriptorSetLayoutBinding.Raw*) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalIndirect(ref unmanaged);
            return result;
        }
    }
}
