using System;
using System.Linq;
using System.Reflection;

namespace VulkaNetGenerator
{
    public class RawField
    {
        //public Type Type { get; }
        public string TypeStr { get; }
        public string Name { get; }
        public bool IgnoreInWrapper { get; }
        public bool IsUnmanagedPtr { get; }
        public string IsCountFor { get; }

        public RawField(FieldInfo fieldInfo)
        {
            //Type = fieldInfo.FieldType;
            TypeStr = DeriveTypeStr(fieldInfo.FieldType);
            Name = fieldInfo.Name;
            
            IsUnmanagedPtr = fieldInfo.FieldType.IsPointer;
            var attr = fieldInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(CountForAttribute));
            IsCountFor = attr?.ConstructorArguments[0].Value as string;
            IgnoreInWrapper = Name == "sType" || IsCountFor != null;
        }

        private static string DeriveTypeStr(Type type)
        {
            if (type.Name.StartsWith("Gen"))
                if (type.IsPointer)
                    return $"Vk{type.Name.Substring(3, type.Name.Length - 4)}.Raw*";
                else
                    return $"Vk{type.Name.Substring(3)}";
            switch (type.Name)
            {
                case "Void*": return "void*";
                case "Byte": return "byte";
                case "Byte*": return "byte*";
                case "Byte**": return "byte**";
                case "Int32": return "int";
                case "Int32*": return "int*";
                case "Int32**": return "int**";
                case "UInt32": return "uint";
                case "UInt32*": return "uint*";
                case "UInt32**": return "uint**";
            }
            return type.Name;
        }
    }
}