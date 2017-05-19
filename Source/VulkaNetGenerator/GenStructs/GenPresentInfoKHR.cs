using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPresentInfoKHR
    {
        public VkStructureType sType;
        public void* pNext;
        [CountFor("WaitSemaphores")]
        public int waitSemaphoreCount;
        [IsArray]
        public GenSemaphore* pWaitSemaphores;
        [CountFor("Swapchains")]
        public int swapchainCount;
        [IsArray]
        public GenSwapchainKHR* pSwapchains;
        [IsArray]
        public int* pImageIndices;
        [FromProperty("(VkResult*)0")]
        public VkResult* pResults;
    }
}