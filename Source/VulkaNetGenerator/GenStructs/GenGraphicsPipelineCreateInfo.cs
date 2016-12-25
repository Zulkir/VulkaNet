using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenGraphicsPipelineCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineCreateFlags flags;
        [CountFor("Stages")] public int stageCount;
        [IsArray] public GenPipelineShaderStageCreateInfo* pStages;
        public GenPipelineVertexInputStateCreateInfo* pVertexInputState;
        public GenPipelineInputAssemblyStateCreateInfo* pInputAssemblyState;
        public GenPipelineTessellationStateCreateInfo* pTessellationState;
        public GenPipelineViewportStateCreateInfo* pViewportState;
        public GenPipelineRasterizationStateCreateInfo* pRasterizationState;
        public GenPipelineMultisampleStateCreateInfo* pMultisampleState;
        public GenPipelineDepthStencilStateCreateInfo* pDepthStencilState;
        public GenPipelineColorBlendStateCreateInfo* pColorBlendState;
        public GenPipelineDynamicStateCreateInfo* pDynamicState;
        public GenPipelineLayout layout;
        public GenRenderPass renderPass;
        public int subpass;
        public GenPipeline basePipelineHandle;
        public int basePipelineIndex;
    }
}