using System;
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSpecializationInfo
    {
        [CountFor("MapEntries")] public int mapEntryCount;
        [IsArray] public VkSpecializationMapEntry* pMapEntries;
        public Sizet dataSize;
        public IntPtr pData;
    }
}