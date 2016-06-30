namespace VulkaNetGenerator
{
    public unsafe struct GenApplicationInfo
    {
        public GenStructureType sType;
        public void* pNext;
        public byte* pApplicationName;
        public uint applicationVersion;
        public byte* pEngineName;
        public uint engineVersion;
        public GenApiVersion apiVersion;
    }
}