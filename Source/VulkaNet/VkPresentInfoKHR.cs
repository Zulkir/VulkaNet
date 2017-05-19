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
    public unsafe class VkPresentInfoKHR
    {
        public IVkStructWrapper Next { get; set; }
        public IReadOnlyList<IVkSemaphore> WaitSemaphores { get; set; }
        public IReadOnlyList<IVkSwapchainKHR> Swapchains { get; set; }
        public IReadOnlyList<int> ImageIndices { get; set; }
        public VkResult* Results { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public VkStructureType sType;
            public void* pNext;
            public int waitSemaphoreCount;
            public VkSemaphore.HandleType* pWaitSemaphores;
            public int swapchainCount;
            public VkSwapchainKHR.HandleType* pSwapchains;
            public int* pImageIndices;
            public VkResult* pResults;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static unsafe class VkPresentInfoKHRExtensions
    {
        public static int SizeOfMarshalDirect(this VkPresentInfoKHR s)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            return
                s.Next.SizeOfMarshalIndirect() +
                s.WaitSemaphores.SizeOfMarshalDirect() +
                s.Swapchains.SizeOfMarshalDirect() +
                s.ImageIndices.SizeOfMarshalDirect();
        }

        public static VkPresentInfoKHR.Raw MarshalDirect(this VkPresentInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                throw new InvalidOperationException("Trying to directly marshal a null.");

            var pNext = s.Next.MarshalIndirect(ref unmanaged);
            var pWaitSemaphores = s.WaitSemaphores.MarshalDirect(ref unmanaged);
            var pSwapchains = s.Swapchains.MarshalDirect(ref unmanaged);
            var pImageIndices = s.ImageIndices.MarshalDirect(ref unmanaged);

            VkPresentInfoKHR.Raw result;
            result.sType = VkStructureType.PresentInfoKHR;
            result.pNext = pNext;
            result.waitSemaphoreCount = s.WaitSemaphores?.Count ?? 0;
            result.pWaitSemaphores = pWaitSemaphores;
            result.swapchainCount = s.Swapchains?.Count ?? 0;
            result.pSwapchains = pSwapchains;
            result.pImageIndices = pImageIndices;
            result.pResults = s.Results;
            return result;
        }

        public static int SizeOfMarshalIndirect(this VkPresentInfoKHR s) =>
            s == null ? 0 : s.SizeOfMarshalDirect() + VkPresentInfoKHR.Raw.SizeInBytes;

        public static VkPresentInfoKHR.Raw* MarshalIndirect(this VkPresentInfoKHR s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkPresentInfoKHR.Raw*)0;
            var result = (VkPresentInfoKHR.Raw*)unmanaged;
            unmanaged += VkPresentInfoKHR.Raw.SizeInBytes;
            *result = s.MarshalDirect(ref unmanaged);
            return result;
        }

        public static int SizeOfMarshalDirect(this IReadOnlyList<VkPresentInfoKHR> list) => 
            list == null || list.Count == 0 
                ? 0
                : sizeof(VkPresentInfoKHR.Raw) * list.Count + list.Sum(x => x.SizeOfMarshalDirect());

        public static VkPresentInfoKHR.Raw* MarshalDirect(this IReadOnlyList<VkPresentInfoKHR> list, ref byte* unmanaged)
        {
            if (list == null || list.Count == 0)
                return (VkPresentInfoKHR.Raw*)0;
            var result = (VkPresentInfoKHR.Raw*)unmanaged;
            unmanaged += sizeof(VkPresentInfoKHR.Raw) * list.Count;
            for (int i = 0; i < list.Count; i++)
                result[i] = list[i].MarshalDirect(ref unmanaged);
            return result;
        }
    }
}
