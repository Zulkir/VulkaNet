using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenWin32SurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkWin32SurfaceCreateFlagsKHR flags;
        public IntPtr hinstance;
        public IntPtr hwnd;//突破　崩す　破る
    }
}