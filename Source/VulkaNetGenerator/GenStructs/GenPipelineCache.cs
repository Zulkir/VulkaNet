using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.GenStructs
{
    public unsafe struct GenPipelineCache : IGenNonDispatchableHandledObject
    {
        [MethodName("Dispose")]
        public void DestroyPipelineCache(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenPipelineCache pipelineCache,
            [FromProperty("Allocator")] GenAllocationCallbacks* pAllocator)
        { }

        [MethodName("Merge")]
        public VkResult MergePipelineCaches(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenPipelineCache dstCache,
            [CountFor("srcCaches")] int srcCacheCount,
            [IsArray] GenPipelineCache* pSrcCaches)
            => default(VkResult);

        [MethodName("GetData")]
        public VkResult GetPipelineCacheData(
            [FromProperty("Device")] GenDevice device,
            [FromProperty("this")] GenPipelineCache pipelineCache,
            [ReturnSize] Sizet* pDataSize,
            [Return] void* pData)
            => default(VkResult);
    }
}