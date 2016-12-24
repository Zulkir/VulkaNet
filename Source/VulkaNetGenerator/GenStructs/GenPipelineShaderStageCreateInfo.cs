using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineShaderStageCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineShaderStageCreateFlags flags;
        public VkShaderStageFlagBits stage;
        public GenShaderModule module;
        public StrByte* pName;
        public GenSpecializationInfo* pSpecializationInfo;
    }
}