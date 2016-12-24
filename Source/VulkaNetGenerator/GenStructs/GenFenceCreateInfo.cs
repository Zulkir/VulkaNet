using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenFenceCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkFenceCreateFlags flags;
    }
}