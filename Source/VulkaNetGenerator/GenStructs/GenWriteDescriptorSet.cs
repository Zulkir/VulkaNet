using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenWriteDescriptorSet
    {
        public VkStructureType sType;
        public void* pNext;
        public GenDescriptorSet dstSet;
        public int dstBinding;
        public int dstArrayElement;
        // todo: make work
        [FromProperty("ImageInfo?.Count ?? BufferInfo?.Count ?? TexelBufferView?.Count ?? 0")]
        public int descriptorCount;
        public VkDescriptorType descriptorType;
        [IsArray] public GenDescriptorImageInfo* pImageInfo;
        [IsArray] public GenDescriptorBufferInfo* pBufferInfo;
        [IsArray] public GenBufferView* pTexelBufferView;
    }
}