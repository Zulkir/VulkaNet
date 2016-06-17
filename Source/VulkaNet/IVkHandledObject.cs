using System;

namespace VulkaNet
{
    public interface IVkHandledObject
    {
        IntPtr Handle { get; }
    }
}