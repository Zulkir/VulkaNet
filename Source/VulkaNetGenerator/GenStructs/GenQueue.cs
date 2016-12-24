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
    }
}