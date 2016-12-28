using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPhysicalDeviceMemoryProperties
    {
        public int memoryTypeCount;
        [FixedArray("32")] public VkMemoryType* memoryTypes;
        public int memoryHeapCount;
        [FixedArray("16")] public VkMemoryHeap* memoryHeaps;
    }
}