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
using System.Runtime.InteropServices;

namespace VulkaNet
{
    public interface IVkAllocationCallbacks
    {
        IntPtr UserData { get; }
        VkAllocationCallbacks.AllocationFunction Allocation { get; }
        VkAllocationCallbacks.ReallocationFunction Reallocation { get; }
        VkAllocationCallbacks.FreeFunction Free { get; }
        VkAllocationCallbacks.InternalAllocationNotification InternalAllocation { get; }
        VkAllocationCallbacks.InternalFreeNotification InternalFree { get; }
    }

    public class VkAllocationCallbacks : IVkAllocationCallbacks
    {
        public IntPtr UserData { get; set; }
        public AllocationFunction Allocation { get; set; }
        public ReallocationFunction Reallocation { get; set; }
        public FreeFunction Free { get; set; }
        public InternalAllocationNotification InternalAllocation { get; set; }
        public InternalFreeNotification InternalFree { get; set; }
        
        public delegate void AllocationFunction(IntPtr pUserData, IntPtr size, IntPtr alignment, VkSystemAllocationScope allocationScope);
        public delegate IntPtr ReallocationFunction(IntPtr pUserData, IntPtr pOriginal, IntPtr size, IntPtr alignment, VkSystemAllocationScope allocationScope);
        public delegate void FreeFunction(IntPtr pUserData, IntPtr pMemory);
        public delegate void InternalAllocationNotification(IntPtr pUserData, IntPtr size, VkInternalAllocationType allocationType, VkSystemAllocationScope allocationScope);
        public delegate void InternalFreeNotification(IntPtr pUserData, IntPtr size, VkInternalAllocationType allocationType, VkSystemAllocationScope allocationScope);

        [StructLayout(LayoutKind.Sequential)]
        public struct Raw
        {
            public IntPtr pUserData;
            public IntPtr pfnAllocation;
            public IntPtr pfnReallocation;
            public IntPtr pfnFree;
            public IntPtr pfnInternalAllocation;
            public IntPtr pfnInternalFree;

            public static int SizeInBytes { get; } = Marshal.SizeOf<Raw>();
        }
    }

    public static class VkAllocationCallbacksExtensions
    {
        public static int SizeOfMarshalIndirect(this IVkAllocationCallbacks s)
            => s != null
                ? VkAllocationCallbacks.Raw.SizeInBytes
                : 0;

        public static unsafe VkAllocationCallbacks.Raw* MarshalIndirect(this IVkAllocationCallbacks s, ref byte* unmanaged)
        {
            if (s == null)
                return (VkAllocationCallbacks.Raw*)0;

            var result = (VkAllocationCallbacks.Raw*)unmanaged;
            unmanaged += VkAllocationCallbacks.Raw.SizeInBytes;
            result->pUserData = s.UserData;
            result->pfnAllocation = Marshal.GetFunctionPointerForDelegate(s.Allocation);
            result->pfnReallocation = Marshal.GetFunctionPointerForDelegate(s.Reallocation);
            result->pfnFree = Marshal.GetFunctionPointerForDelegate(s.Free);
            result->pfnInternalAllocation = Marshal.GetFunctionPointerForDelegate(s.InternalAllocation);
            result->pfnInternalFree = Marshal.GetFunctionPointerForDelegate(s.InternalFree);
            return result;
        }
    }
}