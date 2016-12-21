using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    public class RawFunction
    {
        public string Name { get; }
        public IReadOnlyList<RawParameter> Parameters { get; }
        public Type ReturnGenType { get; }
        public string WrapperName { get; }

        public RawFunction(MethodInfo methodInfo)
        {
            Name = methodInfo.Name;
            Parameters = methodInfo.GetParameters().Select(x => new RawParameter(x)).ToArray();
            ReturnGenType = methodInfo.ReturnType;
            WrapperName = GetAttrValue<MethodNameAttribute>(methodInfo.CustomAttributes.ToArray());
        }

        protected static bool HasAttribute<T>(CustomAttributeData[] attributes) =>
            attributes.Any(x => x.AttributeType == typeof(T));

        protected static string GetAttrValue<T>(CustomAttributeData[] attributes) =>
            attributes.FirstOrDefault(x => x.AttributeType == typeof(T))?.ConstructorArguments[0].Value as string;
    }
}