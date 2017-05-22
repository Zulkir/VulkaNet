using System;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDebugReportCallbackCreateInfoEXT
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDebugReportFlagsEXT flags;
        public IntPtr pfnCallback;
        public void* pUserData;
    }
}