using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineVertexInputStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineVertexInputStateCreateFlags flags;
        [CountFor("VertexBindingDescriptions")] public int vertexBindingDescriptionCount;
        [IsArray] public VkVertexInputBindingDescription* pVertexBindingDescriptions;
        [CountFor("VertexAttributeDescriptions")] public int vertexAttributeDescriptionCount;
        [IsArray] public VkVertexInputAttributeDescription* pVertexAttributeDescriptions;
    }
}