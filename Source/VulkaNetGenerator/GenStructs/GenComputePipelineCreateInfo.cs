using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenComputePipelineCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineCreateFlags flags;
        public GenPipelineShaderStageCreateInfo stage;
        public GenPipelineLayout layout;
        public GenPipeline basePipelineHandle;
        public int basePipelineIndex;
    }
}