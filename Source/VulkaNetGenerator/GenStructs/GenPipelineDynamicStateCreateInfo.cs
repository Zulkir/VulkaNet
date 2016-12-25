using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineDynamicStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineDynamicStateCreateFlags flags;
        [CountFor("DynamicStates")] public int dynamicStateCount;
        [IsArray] public VkDynamicState* pDynamicStates;
    }
}