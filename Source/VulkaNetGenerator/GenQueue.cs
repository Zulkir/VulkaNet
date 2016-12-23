using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    [Handled]
    public unsafe struct GenQueue : IGenHandledObject, IGenDeviceChild
    {
        public VkResult QueueSubmitDelegate(
            [FromProperty("this")] GenQueue queue,
            [CountFor("submits")] int submitCount,
            [IsArray] GenSubmitInfo* pSubmits,
            GenFence fence)
            => default(VkResult);
    }
}