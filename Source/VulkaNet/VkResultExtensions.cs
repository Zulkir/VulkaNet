namespace VulkaNet
{
    public static class VkResultExtensions
    {
        public static void CheckSuccess(this VkResult result)
        {
            if (result != VkResult.Success)
                throw new VkNotSuccessException(result);
        }
    }
}