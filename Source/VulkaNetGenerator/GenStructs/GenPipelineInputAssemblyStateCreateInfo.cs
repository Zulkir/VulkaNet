using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineInputAssemblyStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineInputAssemblyStateCreateFlags flags;
        public VkPrimitiveTopology topology;
        public VkBool32 primitiveRestartEnable;
    }
}