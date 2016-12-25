using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineMultisampleStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineMultisampleStateCreateFlags flags;
        public VkSampleCountFlagBits rasterizationSamples;
        public VkBool32 sampleShadingEnable;
        public float minSampleShading;
        [IsArray] public int* pSampleMask;
        public VkBool32 alphaToCoverageEnable;
        public VkBool32 alphaToOneEnable;
    }
}