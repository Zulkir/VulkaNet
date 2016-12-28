using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineLayoutCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineLayoutCreateFlags flags;
        [CountFor("SetLayouts")] public int setLayoutCount;
        [IsArray] public GenDescriptorSetLayout* pSetLayouts;
        [CountFor("PushConstantRanges")] public int pushConstantRangeCount;
        [IsArray] public VkPushConstantRange* pPushConstantRanges;
    }
}