using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenRenderPassBeginInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public GenRenderPass renderPass;
        public GenFramebuffer framebuffer;
        public VkRect2D renderArea;
        [CountFor("ClearValues")] public int clearValueCount;
        [IsArray] public VkClearValue* pClearValues;
    }
}