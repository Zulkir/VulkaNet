using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    public interface GenFence
    {
        [MethodName("GetStatus")]
        VkResult GetFenceStatus(
            GenDevice device, 
            [Self] GenFence fence);
    }
}