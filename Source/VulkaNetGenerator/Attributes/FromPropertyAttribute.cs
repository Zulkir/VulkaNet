using System;

namespace VulkaNetGenerator.Attributes
{
    public class FromPropertyAttribute : Attribute
    {
        public string Property { get; }

        public FromPropertyAttribute(string property)
        {
            Property = property;
        }
    }
}