using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSubpassDescription
    {
        public VkSubpassDescriptionFlags flags;
        public VkPipelineBindPoint pipelineBindPoint;
        [CountFor("InputAttachments")] public int inputAttachmentCount;
        [IsArray] public VkAttachmentReference* pInputAttachments;
        [CountFor("ColorAttachments")] public int colorAttachmentCount;
        [IsArray] public VkAttachmentReference* pColorAttachments;
        [IsArray] public VkAttachmentReference* pResolveAttachments;
        public VkAttachmentReference* pDepthStencilAttachment;
        [CountFor("PreserveAttachments")] public int preserveAttachmentCount;
        [IsArray] public int* pPreserveAttachments;
    }
}