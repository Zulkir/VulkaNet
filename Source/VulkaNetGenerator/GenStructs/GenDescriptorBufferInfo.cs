using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDescriptorBufferInfo
    {
        public GenSampler sampler;
        public GenImageView imageView;
        public VkImageLayout imageLayout;
    }
}