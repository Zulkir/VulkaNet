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
        public static int SafeMarshalSize(this IVkAllocationCallbacks s)
            => s != null
                ? VkAllocationCallbacks.Raw.SizeInBytes
                : 0;

        public static unsafe VkAllocationCallbacks.Raw* SafeMarshalTo(this IVkAllocationCallbacks s, ref byte* unmanaged)
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