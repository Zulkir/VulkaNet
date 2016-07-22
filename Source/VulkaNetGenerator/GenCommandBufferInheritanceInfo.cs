namespace VulkaNetGenerator
{
    public unsafe struct GenCommandBufferInheritanceInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public HndRenderPass renderPass;
        public int subpass;
        public HndFramebuffer framebuffer;
        public VkBool32 occlusionQueryEnable;
        public VkQueryControlFlags queryFlags;
        public VkQueryPipelineStatisticFlags pipelineStatistics;
    }
}