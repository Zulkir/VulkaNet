using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNetGenerator.Reflection
{
    public class WrapperMethod
    {
        public string Name { get; }
        public IReadOnlyList<WrapperParameter> Parameters { get; }
        public string ReturnTypeStr { get; }
        public RawFunction Raw { get; }

        public WrapperMethod(RawFunction rawFunction)
        {
            Raw = rawFunction;
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
                    case "Void": return wrapper.TypeStr;
                    default: throw new NotImplementedException($"Unexpected return type '{rawFunction.ReturnGenType.Name}'.");
                }
            }
            else
            {
                switch (rawFunction.ReturnGenType.Name)
                {
                    case "VkResult": return "VkResult";
                    case "Void": return "void";
                    default: throw new NotImplementedException($"Unexpected return type '{rawFunction.ReturnGenType.Name}'.");
                }
            }
        }
    }
}