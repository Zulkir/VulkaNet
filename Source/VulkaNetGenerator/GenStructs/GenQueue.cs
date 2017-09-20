using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenQueue : IGenHandledObject
    {
        [MethodName("Submit")]
        public VkResult QueueSubmit(
            [FromProperty("this")] GenQueue queue,
            [CountFor("submits")] int submitCount,
            [IsArray] GenSubmitInfo* pSubmits,
            GenFence fence)
            => default(VkResult);

        [MethodName("WaitIdle")]
        public VkResult QueueWaitIdle(
            [FromProperty("this")] GenQueue queue)
            => default(VkResult);

        [MethodName("BindSparse")]
        public VkResult QueueBindSparse(
            [FromProperty("this")] GenQueue queue,
            [CountFor("bindInfo")] int bindInfoCount,
            [IsArray] GenBindSparseInfo* pBindInfo,
            GenFence fence)
            => default(VkResult);

        [MethodName("PresentKHR")]
        public VkResult QueuePresentKHR(
            [FromProperty("this")] GenQueue queue,
            GenPresentInfoKHR* pPresentInfo)
            => default(VkResult);
    }
}