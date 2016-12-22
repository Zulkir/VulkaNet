namespace VulkaNetGenerator
{
    public class WrapperParameter : WrapperBase<RawParameter>
    {
        public WrapperParameter(RawParameter raw) : base(raw)
        {
            Name = char.ToLower(Name[0]) + Name.Substring(1);
        }
    }
}