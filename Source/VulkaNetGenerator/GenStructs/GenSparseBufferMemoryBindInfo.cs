using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenSparseBufferMemoryBindInfo
    {
        public GenBuffer buffer;
        [CountFor("Binds")] public int bindCount;
        [IsArray] public GenSparseMemoryBind* pBinds;
    }
}