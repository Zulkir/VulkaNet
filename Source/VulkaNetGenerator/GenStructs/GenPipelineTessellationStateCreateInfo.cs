using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineTessellationStateCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkPipelineTessellationStateCreateFlags flags;
        public int patchControlPoints;
    }
}