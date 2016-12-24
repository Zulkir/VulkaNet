namespace VulkaNetGenerator.Reflection
{
    public class WrapperBase<TRaw>
        where TRaw : RawBase
    {
        public TRaw Raw { get; }
        public string TypeStr { get; }
        public string Name { get; protected set; }
        public bool MarshalledAsUnmanaged { get; }
        public bool NeedsCast { get; }
        public string CreatorFunc { get; }
        public bool CreatorFuncTakesPtr { get; }
        public string SizeMethod { get; }
        public string MarshalMethod { get; }

        protected WrapperBase(TRaw raw)
        {
            Raw = raw;
            TypeStr = DeriveTypeStr(raw);
            Name = DeriveName(raw);
            MarshalledAsUnmanaged = IsMarshalledAsUnamanaged(raw);
            NeedsCast = DeriveNeedsCast(raw);
            CreatorFunc = DeriveCreatorFunc(raw, TypeStr);
            CreatorFuncTakesPtr = DeriveCreatorFuncTakesPtr(raw);
            string sizeMethod, marshalMethod;
            DeriveMarshalMethods(raw, out sizeMethod, out marshalMethod);
            SizeMethod = sizeMethod;
            MarshalMethod = marshalMethod;
        }

        private static string DeriveTypeStr(TRaw raw)
        {
            if (raw.ExplicitWrapperType != null)
                return raw.ExplicitWrapperType;
            if (raw.Name == "pNext")
                return "IVkStructWrapper";
            if (raw.IsArray)
            {
                var elemTypeStr = DeriveTypeInternal(raw.TypeStr.Substring(0, raw.TypeStr.Length - 1));
                return $"IReadOnlyList<{elemTypeStr}>";
            }
            return DeriveTypeInternal(raw.TypeStr);
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
            if (rawTypeStr.EndsWith("*"))
                return rawTypeStr.Substring(0, rawTypeStr.Length - 1);
            return rawTypeStr;
        }

        private static string DeriveName(TRaw raw)
        {
            if (IsDoublePointerName(raw.Name))
                return raw.Name.Substring(2);
            if (IsSinglePointerName(raw.Name))
                return raw.Name.Substring(1);
            return "" + char.ToUpper(raw.Name[0]) + raw.Name.Substring(1);
        }

        private static string DeriveCreatorFunc(TRaw raw, string wrapperTypeStr)
        {
            if (raw.TypeStr == wrapperTypeStr)
                return null;
            if (wrapperTypeStr == "string")
                return "VkHelpers.ToString";
            if (wrapperTypeStr.StartsWith("IVk"))
                return "new " + wrapperTypeStr.Substring(1);
            if (wrapperTypeStr.StartsWith("Vk"))
                return "new " + wrapperTypeStr;
            return null;
        }

        private static bool DeriveCreatorFuncTakesPtr(TRaw rawField) => 
            !rawField.TypeStr.EndsWith("*") && rawField.FixedArraySize == null;

        private static bool DeriveNeedsCast(TRaw rawField)
        {
            if (rawField.TypeStr == "VkBool32")
                return true;
            return false;
        }

        private void DeriveMarshalMethods(TRaw raw, out string sizeMethod, out string marshalMethod)
        {
            if (raw.IsArray)
            {
                if (raw.TypeStr.EndsWith("**"))
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
                if (raw.TypeStr.EndsWith("*"))
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

        private static bool IsMarshalledAsUnamanaged(TRaw raw) => 
            raw.IsUnmanagedPtr;
    }
}