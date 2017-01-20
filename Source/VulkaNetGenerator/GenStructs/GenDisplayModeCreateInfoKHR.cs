using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDisplayModeCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDisplayModeCreateFlagsKHR flags;
        public VkDisplayModeParametersKHR parameters;
    }
}