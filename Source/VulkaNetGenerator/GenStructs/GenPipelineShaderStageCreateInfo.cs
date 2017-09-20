using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineShaderStageCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineShaderStageCreateFlags flags;
        public VkShaderStage stage;
        public GenShaderModule module;
        public StrByte* pName;
        [Nullable] public GenSpecializationInfo* pSpecializationInfo;
    }
}