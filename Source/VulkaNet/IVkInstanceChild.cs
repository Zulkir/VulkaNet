namespace VulkaNet
{
    public interface IVkInstanceChild : IVkHandledObject
    {
        IVkInstance Instance { get; }
    }
}