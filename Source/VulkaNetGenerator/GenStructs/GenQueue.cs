using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
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