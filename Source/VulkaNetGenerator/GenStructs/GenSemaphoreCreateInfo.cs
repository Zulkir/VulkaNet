using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSemaphoreCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkSemaphoreCreateFlags flags;
    }
}