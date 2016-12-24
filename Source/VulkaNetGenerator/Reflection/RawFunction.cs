using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator.Reflection
{
    public class RawFunction
    {
        public string Name { get; }
        public IReadOnlyList<RawParameter> Parameters { get; }
        public Type ReturnGenType { get; }
        public string ReturnTypeStr { get; }
        public string WrapperName { get; }

        public RawFunction(MethodInfo methodInfo)
        {
            Name = methodInfo.Name;
            Parameters = methodInfo.GetParameters().Select(x => new RawParameter(x)).ToArray();
            ReturnGenType = methodInfo.ReturnType;
            ReturnTypeStr = DeriveReturnTypeStr(ReturnGenType);
            WrapperName = GetAttrValue<MethodNameAttribute>(methodInfo.CustomAttributes.ToArray()) ?? Name;
        }

        private static bool HasAttribute<T>(CustomAttributeData[] attributes) =>
            attributes.Any(x => x.AttributeType == typeof(T));

        private static string GetAttrValue<T>(CustomAttributeData[] attributes) =>
            attributes.FirstOrDefault(x => x.AttributeType == typeof(T))?.ConstructorArguments[0].Value as string;

        private static string DeriveReturnTypeStr(Type returnGenType)
        {
            switch (returnGenType.Name)
            {
                case "Void": return "void";
                case "VkResult": return "VkResult";
                default: throw new NotSupportedException($"Unexpected return type '{returnGenType.Name}'.");
            }
        }
    }
}