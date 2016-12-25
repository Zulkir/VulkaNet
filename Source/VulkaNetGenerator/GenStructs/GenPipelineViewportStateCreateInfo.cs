using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineViewportStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineViewportStateCreateFlags flags;
        [CountFor("Viewports")] public int viewportCount;
        [IsArray] public VkViewport* pViewports;
        [CountFor("Scissors")] public int scissorCount;
        [IsArray] public VkRect2D* pScissors;
    }
}