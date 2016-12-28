using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorSetAllocateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public GenDescriptorPool descriptorPool;
        [CountFor("SetLayouts")] public int descriptorSetCount;
        [IsArray] public GenDescriptorSetLayout* pSetLayouts;
    }
}