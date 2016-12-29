using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenQueryPoolCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkQueryPoolCreateFlags flags;
        public VkQueryType queryType;
        public int queryCount;
        public VkQueryPipelineStatisticFlags pipelineStatistics;
    }
}