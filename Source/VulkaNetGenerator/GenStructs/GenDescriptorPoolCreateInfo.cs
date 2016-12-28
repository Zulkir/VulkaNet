using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorPoolCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDescriptorPoolCreateFlags flags;
        public int maxSets;
        [CountFor("PoolSizes")] public int poolSizeCount;
        [IsArray] public VkDescriptorPoolSize* pPoolSizes;
    }
}