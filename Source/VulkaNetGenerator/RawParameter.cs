using System.Linq;
using System.Reflection;
using VulkaNetGenerator.Attributes;

namespace VulkaNetGenerator
{
    public class RawParameter : RawBase
    {
        public bool IsReturnParam { get; }

        public RawParameter(ParameterInfo parameterInfo)
            : base(parameterInfo.ParameterType, parameterInfo.Name, parameterInfo.CustomAttributes.ToArray())
        {
            IsReturnParam = HasAttribute<ReturnAttribute>(attributes);
        }
    }
}