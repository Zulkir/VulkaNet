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
    public unsafe class VkSwapchainCreateInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public VkSwapchainCreateFlagsKHR Flags { get; set; }
        public IVkSurfaceKHR Surface { get; set; }
        public int MinImageCount { get; set; }
        public VkFormat ImageFormat { get; set; }
        public VkColorSpaceKHR ImageColorSpace { get; set; }
        public VkExtent2D ImageExtent { get; set; }
        public int ImageArrayLayers { get; set; }
        public VkImageUsageFlags ImageUsage { get; set; }
        public VkSharingMode ImageSharingMode { get; set; }
        public IReadOnlyList<int> QueueFamilyIndices { get; set; }
        public VkSurfaceTransformFlagBitsKHR PreTransform { get; set; }
        public VkCompositeAlphaFlagsKHR CompositeAlpha { get; set; }
        public VkPresentModeKHR PresentMode { get; set; }
        public bool Clipped { get; set; }
        public IVkSwapchainKHR OldSwapchain { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public VkSwapchainCreateFlagsKHR flags;
            public VkSurfaceKHR.HandleType surface;
            public int minImageCount;
            public VkFormat imageFormat;
            public VkColorSpaceKHR imageColorSpace;
            public VkExtent2D imageExtent;
            public int imageArrayLayers;
            public VkImageUsageFlags imageUsage;
            public VkSharingMode imageSharingMode;
            public int queueFamilyIndexCount;
            public int* pQueueFamilyIndices;
            public VkSurfaceTransformFlagBitsKHR preTransform;
            public VkCompositeAlphaFlagsKHR compositeAlpha;
            public VkPresentModeKHR presentMode;
            public VkBool32 clipped;
            public VkSwapchainKHR.HandleType oldSwapchain;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkSwapchainCreateInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkSwapchainCreateInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.QueueFamilyIndices.SizeOfMarshalDirect();
        }

        public static VkSwapchainCreateInfoKHR.Raw MarshalDirect(this VkSwapchainCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pQueueFamilyIndices = s.QueueFamilyIndices.MarshalDirect(ref unmanaged);

            VkSwapchainCreateInfoKHR.Raw result;
            result.sType = VkStructureType.SwapchainCreateInfoKHR;
            result.pNext = pNext;
            result.flags = s.Flags;
            result.surface = s.Surface?.Handle ?? VkSurfaceKHR.HandleType.Null;
            result.minImageCount = s.MinImageCount;
            result.imageFormat = s.ImageFormat;
            result.imageColorSpace = s.ImageColorSpace;
            result.imageExtent = s.ImageExtent;
            result.imageArrayLayers = s.ImageArrayLayers;
            result.imageUsage = s.ImageUsage;
            result.imageSharingMode = s.ImageSharingMode;
            result.queueFamilyIndexCount = s.QueueFamilyIndices?.Count ?? 0;
            result.pQueueFamilyIndices = pQueueFamilyIndices;
            result.preTransform = s.PreTransform;
            result.compositeAlpha = s.CompositeAlpha;
            result.presentMode = s.PresentMode;
            result.clipped = new VkBool32(s.Clipped);
            result.oldSwapchain = s.OldSwapchain?.Handle ?? VkSwapchainKHR.HandleType.Null;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkSwapchainCreateInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkSwapchainCreateInfoKHR.Raw.SizeInBytes;

        public static VkSwapchainCreateInfoKHR.Raw* MarshalIndirect(this VkSwapchainCreateInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkSwapchainCreateInfoKHR.Raw*)0;
            var result = (VkSwapchainCreateInfoKHR.Raw*)unmanaged;
            unmanaged += VkSwapchainCreateInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkSwapchainCreateInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkSwapchainCreateInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkSwapchainCreateInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkSwapchainCreateInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkSwapchainCreateInfoKHR.Raw*)0;
            var result = (VkSwapchainCreateInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkSwapchainCreateInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
