﻿using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    public class RawParameter : RawBase
    {
        public bool IsReturnParam { get; }
        public bool IsSelf { get; }
        public string FromProperty { get; }

        public RawParameter(ParameterInfo parameterInfo)
            : base(parameterInfo.ParameterType, parameterInfo.Name, parameterInfo.CustomAttributes.ToArray())
        {
            IsReturnParam = HasAttribute<ReturnAttribute>(attributes);
            IsSelf = HasAttribute<SelfAttribute>(attributes);
            if (IsSelf/* || TypeStr == $"Vk{Name}.Handle"*/)
                IgnoreInWrapper = true;
            if (IsReturnParam)
                IgnoreInWrapper = true;
            //if (TypeStr == "VkDevice.Handle")
            //    IgnoreInWrapper = true;
            //if (TypeStr == "VkAllocationCallbacks.Raw*")
            //    IgnoreInWrapper = true;
            FromProperty = GetAttrValue<FromPropertyAttribute>(attributes);
        }
    }
}