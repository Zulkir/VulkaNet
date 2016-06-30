namespace VulkaNetGenerator
{
    public class WrapperProperty
    {
        public RawField RawField { get; }
        public RawField CountField { get; }
        public string TypeStr { get; }
        public string Name { get; }
        public bool MarshalledAsUnmanaged { get; }

        public WrapperProperty(RawField rawField, RawField countField)
        {
            RawField = rawField;
            CountField = countField;

            TypeStr = DeriveTypeStr(rawField);
            Name = DeriveName(rawField);
            MarshalledAsUnmanaged = IsMarshalledAsUnamanaged(rawField);
        }

        private static string DeriveTypeStr(RawField rawField)
        {
            if (rawField.Name == "pNext")
                return "IVkStructWrapper";
            //if (rawField.Type.Name.StartsWith("Gen"))
            //    return "Vk" + rawField.Type.Name.Substring(3);
            if (rawField.TypeStr == "byte*")
                return "string";
            return rawField.TypeStr;
        }

        private static string DeriveName(RawField rawField)
        {
            if (IsDoublePointerName(rawField.Name))
                return rawField.Name.Substring(2);
            if (IsSinglePointerName(rawField.Name))
                return rawField.Name.Substring(1);
            return "" + char.ToUpper(rawField.Name[0]) + rawField.Name.Substring(1);
        }

        private static bool IsDoublePointerName(string name) =>
            name[0] == 'p' && name[1] == 'p' && char.IsUpper(name[2]);

        private static bool IsSinglePointerName(string name) =>
            name[0] == 'p' && char.IsUpper(name[1]);

        private static bool IsMarshalledAsUnamanaged(RawField rawField) => 
            rawField.IsUnmanagedPtr;
    }
}