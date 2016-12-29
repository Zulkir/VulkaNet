using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenCopyDescriptorSet
    {
        public VkStructureType sType;
        public void* pNext;
        public GenDescriptorSet srcSet;
        public int srcBinding;
        public int srcArrayElement;
        public GenDescriptorSet dstSet;
        public int dstBinding;
        public int dstArrayElement;
        public int descriptorCount;
    }
}