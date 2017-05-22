using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenDebugMarkerMarkerInfoEXT
    {
        public VkStructureType sType;
        public void* pNext;
        public StrByte* pMarkerName;
        public VkColor4 color;
    }
}