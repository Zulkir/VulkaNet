using System.Linq;
using System.Reflection;

namespace VulkaNetGenerator.Reflection
{
    public class RawField : RawBase
    {
        public RawField(FieldInfo fieldInfo) 
            : base(fieldInfo.FieldType, fieldInfo.Name, fieldInfo.CustomAttributes.ToArray())
        {
            
        }
    }
}