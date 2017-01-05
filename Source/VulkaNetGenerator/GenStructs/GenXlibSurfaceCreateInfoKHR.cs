using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenXlibSurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkXlibSurfaceCreateFlagsKHR flags;
        public IntPtr dpy;
        public IntPtr window;
    }
}