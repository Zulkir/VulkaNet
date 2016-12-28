using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorSetLayoutBinding
    {
        public int binding;
        public VkDescriptorType descriptorType;
        public int descriptorCount;
        public VkShaderStageFlagBits stageFlags;
        [IsArray] public GenSampler* pImmutableSamplers;
    }
}