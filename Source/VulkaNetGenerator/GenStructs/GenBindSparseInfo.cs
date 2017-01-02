using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenBindSparseInfo
    {
        public VkStructureType sType;
        public void* pNext;
        [CountFor("WaitSemaphores")] public int waitSemaphoreCount;
        [IsArray] public GenSemaphore* pWaitSemaphores;
        [CountFor("BufferBinds")] public int bufferBindCount;
        [IsArray] public GenSparseBufferMemoryBindInfo* pBufferBinds;
        [CountFor("ImageOpaqueBinds")] public int imageOpaqueBindCount;
        [IsArray] public GenSparseImageOpaqueMemoryBindInfo* pImageOpaqueBinds;
        [CountFor("ImageBinds")] public int imageBindCount;
        [IsArray] public GenSparseImageMemoryBindInfo* pImageBinds;
        [CountFor("SignalSemaphores")] public int signalSemaphoreCount;
        [IsArray] public GenSemaphore* pSignalSemaphores;
    }
}