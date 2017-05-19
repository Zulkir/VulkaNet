using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPresentRegionKHR
    {
        [CountFor("Rectangles")] public int rectangleCount;
        [IsArray] public VkRectLayerKHR* pRectangles;
    }
}