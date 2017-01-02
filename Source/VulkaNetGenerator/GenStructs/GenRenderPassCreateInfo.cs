using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenRenderPassCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkRenderPassCreateFlags flags;
        [CountFor("Attachments")] public int attachmentCount;
        [IsArray] public VkAttachmentDescription* pAttachments;
        [CountFor("Subpasses")] public int subpassCount;
        [IsArray] public GenSubpassDescription* pSubpasses;
        [CountFor("Dependencies")] public int dependencyCount;
        [IsArray] public VkSubpassDependency* pDependencies;
    }
}