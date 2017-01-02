using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSparseImageOpaqueMemoryBindInfo
    {
        public GenImage image;
        [CountFor("Binds")] public int bindCount;
        [IsArray] public GenSparseMemoryBind* pBinds;
    }
}