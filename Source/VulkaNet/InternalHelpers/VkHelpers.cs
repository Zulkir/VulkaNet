using System;
using System.Runtime.InteropServices;

namespace VulkaNet.InternalHelpers
{
    public static class VkHelpers
    {
        public static TDelegate GetDelegate<TDelegate>(IVkInstance instance, string name)
        {
            var funPtr = VkGlobal.GetInstanceProcAddr(instance, name);
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(funPtr);
        }

        public static unsafe string ToString(byte* cstr) => Marshal.PtrToStringAnsi((IntPtr)cstr);

        public static unsafe void RunWithUnamangedData(int size, Action<IntPtr> func)
        {
            if (size <= 256)
            {
                VkBlob256 blob;
                func((IntPtr)(&blob));
            }
            else
            {
                var data = new byte[size];
                fixed (byte* pData = data)
                    func((IntPtr)pData);
            }
        }

        public static unsafe TResult RunWithUnamangedData<TResult>(int size, Func<IntPtr, TResult> func)
        {
            if (size <= 256)
            {
                VkBlob256 blob;
                return func((IntPtr)(&blob));
            }
            else
            {
                var data = new byte[size];
                fixed (byte* pData = data)
                    return func((IntPtr)pData);
            }
        }
    }
}