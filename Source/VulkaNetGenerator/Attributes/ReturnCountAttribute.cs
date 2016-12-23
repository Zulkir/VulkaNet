using System;

namespace VulkaNetGenerator.Attributes
{
    public class ReturnCountAttribute : Attribute
    {
        public string Property { get; }

        public ReturnCountAttribute(string property)
        {
            Property = property;
        }
    }
}