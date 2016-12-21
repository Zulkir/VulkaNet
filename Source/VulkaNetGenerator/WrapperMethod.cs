using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNetGenerator
{
    public class WrapperMethod
    {
        public string Name { get; }
        public IReadOnlyList<WrapperParameter> Parameters { get; }
        public string ReturnTypeStr { get; }

        public WrapperMethod(RawFunction rawFunction)
        {
            Name = rawFunction.WrapperName;
            Parameters = rawFunction.Parameters.Where(x => !x.IgnoreInWrapper).Select(x => new WrapperParameter(x)).ToArray();
            ReturnTypeStr = DeriveReturnTypeStr(rawFunction);
        }

        private static string DeriveReturnTypeStr(RawFunction rawFunction)
        {
            var returnParam = rawFunction.Parameters.FirstOrDefault(x => x.IsReturnParam);
            if (returnParam != null)
            {
                var wrapper = new WrapperParameter(returnParam);
                switch (rawFunction.ReturnGenType.Name)
                {
                    case "VkResult": return $"VkObjectResult<{wrapper.TypeStr}>";
                    case "void": return wrapper.TypeStr;
                    default: throw new NotImplementedException($"Unexpected return type '{rawFunction.ReturnGenType.Name}'.");
                }
            }
            else
            {
                switch (rawFunction.ReturnGenType.Name)
                {
                    case "VkResult": return "VkResult";
                    case "void": return "void";
                    default: throw new NotImplementedException($"Unexpected return type '{rawFunction.ReturnGenType.Name}'.");
                }
            }
        }
    }
}