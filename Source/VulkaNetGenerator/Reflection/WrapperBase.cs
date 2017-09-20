using VulkaNetGenerator.Dummies;

namespace VulkaNetGenerator.Reflection
{
    public class WrapperBase<TRaw>
        where TRaw : RawBase
    {
        public TRaw Raw { get; }
        public string TypeStr { get; protected set; }
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
            
            NeedsCast = DeriveNeedsCast(raw.TypeStr, TypeStr);
            CreatorFunc = DeriveCreatorFunc(raw, TypeStr);
            CreatorFuncTakesPtr = DeriveCreatorFuncTakesPtr(raw);
            string sizeMethod, marshalMethod;
            DeriveMarshalMethods(raw, out sizeMethod, out marshalMethod);
            SizeMethod = sizeMethod;
            MarshalMethod = marshalMethod;
            MarshalledAsUnmanaged = IsMarshalledAsUnamanaged(raw);
        }

        private static string DeriveTypeStr(TRaw raw)
        {
            if (raw.ExplicitWrapperType != null)
                return raw.ExplicitWrapperType;
            if (raw.Name == "pNext")
                return "IVkStructWrapper";
            if (raw.IsByteArray)
                return "byte[]";
            if (raw.IsArray)
            {
                var elemTypeStr = DeriveTypeInternal(raw.TypeStr.Substring(0, raw.TypeStr.Length - 1), false, raw.IsNullable);
                return $"IReadOnlyList<{elemTypeStr}>";
            }
            return DeriveTypeInternal(raw.TypeStr, raw.IsActuallyStruct, raw.IsNullable);
        }

        private static string DeriveTypeInternal(string rawTypeStr, bool isActuallyStruct, bool isNullable)
        {
            if (rawTypeStr.EndsWith(".Raw*"))
                if (isActuallyStruct)
                    return rawTypeStr.Substring(0, rawTypeStr.Length - ".Raw*".Length) + "?";
                else
                    return rawTypeStr.Substring(0, rawTypeStr.Length - ".Raw*".Length);
            if (rawTypeStr.EndsWith(".Raw"))
                return rawTypeStr.Substring(0, rawTypeStr.Length - ".Raw".Length);
            if (rawTypeStr.EndsWith(".HandleType*"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".HandleType*".Length);
            if (rawTypeStr.EndsWith(".HandleType"))
                return "I" + rawTypeStr.Substring(0, rawTypeStr.Length - ".HandleType".Length);
            if (rawTypeStr == "VkBool32")
                return "bool";
            if (rawTypeStr.EndsWith("*"))
                if (isNullable)
                    return rawTypeStr.Substring(0, rawTypeStr.Length - 1) + "?";
                else
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
            !rawField.TypeStr.EndsWith("*") && rawField.FixedArraySize == null && !typeof(IGenInstanceChild).IsAssignableFrom(rawField.GenType);

        private static bool DeriveNeedsCast(string rawTypeStr, string thisTypeStr)
        {
            if (rawTypeStr == "VkBool32")
                return true;
            if (rawTypeStr == "IntPtr" && thisTypeStr == "int")
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
            raw.ShouldMarshal;
    }
}