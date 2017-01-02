using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSparseImageMemoryBindInfo
    {
        public GenImage image;
        [CountFor("Binds")] public int bindCount;
        [IsArray] public GenSparseImageMemoryBind* pBinds;
    }
}