using System;

namespace VulkaNet
{
    public class VkNotSuccessException : Exception
    {
        public VkNotSuccessException(VkResult result) : base(result.ToString()) { }
    }
}