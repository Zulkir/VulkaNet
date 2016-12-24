using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenFramebufferCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkFramebufferCreateFlags flags;
        public GenRenderPass renderPass;
        [CountFor("Attachments")] public int attachmentCount;
        [IsArray] public GenImageView* pAttachments;
        public int width;
        public int height;
        public int layers;
    }
}