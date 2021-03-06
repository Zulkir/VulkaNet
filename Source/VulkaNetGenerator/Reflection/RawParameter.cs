﻿using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;
using VulkaNetGenerator.GenStructs;

namespace VulkaNetGenerator.Reflection
{
    public class RawParameter : RawBase
    {
        public bool IsReturnParam { get; }
        public string ReturnCount { get; }
        public bool IsReturnSize { get; }

        public RawParameter(ParameterInfo parameterInfo)
            : base(parameterInfo.ParameterType, parameterInfo.Name, parameterInfo.CustomAttributes.ToArray())
        {
            IsReturnParam = HasAttribute<ReturnAttribute>(attributes);
            ReturnCount = GetAttrValue<ReturnCountAttribute>(attributes);
            IsReturnSize = HasAttribute<ReturnSizeAttribute>(attributes);
            if (IsReturnParam)
                IgnoreInWrapper = true;
            if (TypeStr.StartsWith("VkDevice.HandleType") && parameterInfo.Member.DeclaringType == typeof(GenDevice))
                TypeStr = TypeStr.Substring("VkDevice.".Length);
            if (parameterInfo.ParameterType.Name.StartsWith("Vk") && !IsArray)
                ShouldMarshal = false;
        }
    }
}