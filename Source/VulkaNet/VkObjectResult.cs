namespace VulkaNet
{
    public struct VkObjectResult<T>
    {
        private readonly VkResult result;
        private readonly T obj;

        public VkResult Result { get { return result; } }
        public T ObjectUnchecked { get { return obj; } }

        public T Object
        {
            get
            {
                if (result != VkResult.Success)
                    throw new VkNotSuccessException(result);
                return obj;
            }
        }

        public VkObjectResult(VkResult result, T obj)
        {
            this.result = result;
            this.obj = obj;
        }
    }
}