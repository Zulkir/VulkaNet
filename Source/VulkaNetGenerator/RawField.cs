#region License
/*
Copyright (c) 2016 VulkaNet Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

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