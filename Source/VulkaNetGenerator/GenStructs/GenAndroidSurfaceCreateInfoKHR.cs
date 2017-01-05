using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenAndroidSurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkAndroidSurfaceCreateFlagsKHR flags;
        public IntPtr window;
    }
}