namespace VulkaNetGenerator.Reflection
{
    public class WrapperParameter : WrapperBase<RawParameter>
    {
        public bool IsFromProperty { get; }

        public WrapperParameter(RawParameter raw) : base(raw)
        {
            if (raw.FromProperty != null)
            {
                IsFromProperty = true;
                Name = raw.FromProperty;
            }
            else
            {
                Name = char.ToLower(Name[0]) + Name.Substring(1);
            }

            if (raw.IsReturnParam && raw.TypeStr == "void*")
            {
                TypeStr = "byte[]";
            }
        }
    }
}