using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDisplaySurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDisplaySurfaceCreateFlagsKHR flags;
        public GenDisplayModeKHR displayMode;
        public int planeIndex;
        public int planeStackIndex;
        public VkSurfaceTransformFlagBitsKHR transform;
        public float globalAlpha;
        public VkDisplayPlaneAlphaFlagBitsKHR alphaMode;
        public VkExtent2D imageExtent;
    }
}