using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineColorBlendStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineColorBlendStateCreateFlags flags;
        public VkBool32 logicOpEnable;
        public VkLogicOp logicOp;
        [CountFor("Attachments")] public int attachmentCount;
        [IsArray] public VkPipelineColorBlendAttachmentState* pAttachments;
        public VkColor4 blendConstants;
    }
}