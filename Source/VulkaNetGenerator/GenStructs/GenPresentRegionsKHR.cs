using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPresentRegionsKHR
    {
        public VkStructureType sType;
        public void* pNext;
        [CountFor("Regions")] public int swapchainCount;
        [IsArray] public GenPresentRegionKHR* pRegions;
    }
}