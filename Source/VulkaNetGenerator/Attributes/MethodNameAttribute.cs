using System;

namespace VulkaNetGenerator.Attributes
{
    public class MethodNameAttribute : Attribute
    {
        public string WrapperName { get; }

        public MethodNameAttribute(string typeStr) { WrapperName = typeStr; }
    }
}