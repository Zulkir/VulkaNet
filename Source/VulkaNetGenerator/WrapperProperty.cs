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

namespace VulkaNetGenerator
{
    public class WrapperProperty
    {
        public RawField RawField { get; }
        public RawField CountField { get; }
        public string TypeStr { get; }
        public string Name { get; }
        public bool MarshalledAsUnmanaged { get; }
        public bool NeedsCast { get; }
        public string CreatorFunc { get; }
        public bool CreatorFuncTakesPtr { get; }
        public string SizeMethod { get; }
        public string MarshalMethod { get; }

        public WrapperProperty(RawField rawField, RawField countField)
        {
            RawField = rawField;
            CountField = countField;

            TypeStr = DeriveTypeStr(rawField);
            Name = DeriveName(rawField);
            MarshalledAsUnmanaged = IsMarshalledAsUnamanaged(rawField);
            NeedsCast = DeriveNeedsCast(rawField);
            CreatorFunc = DeriveCreatorFunc(rawField, TypeStr);
            CreatorFuncTakesPtr = DeriveCreatorFuncTakesPtr(rawField);
            string sizeMethod, marshalMethod;
            DeriveMarshalMethods(rawField, out sizeMethod, out marshalMethod);
            SizeMethod = sizeMethod;
            MarshalMethod = marshalMethod;
        }

        private static string DeriveTypeStr(RawField rawField)
        {
            if (rawField.ExplicitWrapperType != null)
                return rawField.ExplicitWrapperType;
            if (rawField.Name == "pNext")
                return "IVkStructWrapper";
            if (rawField.IsArray)
            {
                var elemTypeStr = DeriveTypeInternal(rawField.TypeStr.Substring(0, rawField.TypeStr.Length - 1));
                return $"IReadOnlyList<{elemTypeStr}>";
            }
            return DeriveTypeInternal(rawField.TypeStr);
        }

        private static string DeriveTypeInternal(string rawTypeStr)
        {
            if (rawTypeStr.EndsWith(".Raw*"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".Raw*".Length);
            if (rawTypeStr.EndsWith(".Raw"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".Raw".Length);
            //if (rawTypeStr.EndsWith("*"))
            //    return $"IReadOnlyList<{DeriveTypeInternal(rawTypeStr.Substring(0, rawTypeStr.Length - 1))}>";
            if (rawTypeStr.EndsWith(".HandleType*"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".HandleType*".Length);
            if (rawTypeStr.EndsWith(".HandleType"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".HandleType".Length);
            if (rawTypeStr == "VkBool32")
                return "bool";
            return rawTypeStr;
        }

        private static string DeriveName(RawField rawField)
        {
            if (IsDoublePointerName(rawField.Name))
                return rawField.Name.Substring(2);
            if (IsSinglePointerName(rawField.Name))
                return rawField.Name.Substring(1);
            return "" + char.ToUpper(rawField.Name[0]) + rawField.Name.Substring(1);
        }

        private static string DeriveCreatorFunc(RawField rawField, string wrapperTypeStr)
        {
            if (rawField.TypeStr == wrapperTypeStr)
                return null;
            if (wrapperTypeStr == "string")
                return "VkHelpers.ToString";
            if (wrapperTypeStr.StartsWith("IVk"))
                return "new " + wrapperTypeStr.Substring(1);
            if (wrapperTypeStr.StartsWith("Vk"))
                return "new " + wrapperTypeStr;
            return null;
        }

        private static bool DeriveCreatorFuncTakesPtr(RawField rawField) => 
            !rawField.TypeStr.EndsWith("*") && rawField.FixedArraySize == null;

        private static bool DeriveNeedsCast(RawField rawField)
        {
            if (rawField.TypeStr == "VkBool32")
                return true;
            return false;
        }

        private void DeriveMarshalMethods(RawField rawField, out string sizeMethod, out string marshalMethod)
        {
            if (rawField.IsArray)
            {
                if (rawField.TypeStr.EndsWith("**"))
                {
                    sizeMethod = "SizeOfMarshalIndirect";
                    marshalMethod = "MarshalIndirect";
                }
                else
                {
                    sizeMethod = "SizeOfMarshalDirect";
                    marshalMethod = "MarshalDirect";
                }
            }
            else
            {
                if (rawField.TypeStr.EndsWith("*"))
                {
                    sizeMethod = "SizeOfMarshalIndirect";
                    marshalMethod = "MarshalIndirect";
                }
                else
                {
                    sizeMethod = "SizeOfMarshalDirect";
                    marshalMethod = "MarshalDirect";
                }
            }
        }

        private static bool IsDoublePointerName(string name) =>
            name[0] == 'p' && name[1] == 'p' && char.IsUpper(name[2]);

        private static bool IsSinglePointerName(string name) =>
            name[0] == 'p' && char.IsUpper(name[1]);

        private static bool IsMarshalledAsUnamanaged(RawField rawField) => 
            rawField.IsUnmanagedPtr;
    }
}