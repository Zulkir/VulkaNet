using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public struct GenSubpassDependency
    {
        public int srcSubpass;
        public int dstSubpass;
        public VkPipelineStageFlags srcStageMask;
        public VkPipelineStageFlags dstStageMask;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public VkDependencyFlags dependencyFlags;
    }
}