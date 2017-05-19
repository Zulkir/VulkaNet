using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDisplayPresentInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        public VkRect2D srcRect;
        public VkRect2D dstRect;
        public VkBool32 persistent;
    }
}