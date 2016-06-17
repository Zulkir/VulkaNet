using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public static class ReadOnlyListExtensions
    {
        public static int SafeMarshalSize(this IReadOnlyList<string> list) 
            => SafeMarshalSize(list, StringExtensions.SafeMarshalSize);
        public static int SafeMarshalSize<T>(this IReadOnlyList<T> list, Func<T, int> marshalElemSize) 
            => list != null ? list.Sum(marshalElemSize) + IntPtr.Size * list.Count : 0;
        
        public unsafe delegate IntPtr MarshalElemDelegate<T>(T elem, ref byte* unmanaged);

        private static unsafe IntPtr SafeMarshalStringTo(string elem, ref byte* unmanaged) 
            => (IntPtr)elem.SafeMarshalTo(ref unmanaged);
        public static unsafe byte** SafeMarshalTo(this IReadOnlyList<string> list, ref byte* unamanged) 
            => (byte**)SafeMarshalTo(list, ref unamanged, SafeMarshalStringTo);
        public static unsafe IntPtr* SafeMarshalTo<T>(this IReadOnlyList<T> list, ref byte* unamanged, MarshalElemDelegate<T> marshalElem)
        {
            if (list == null)
                return (IntPtr*)0;
            var ptrArray = new IntPtr[list.Count];
            for (int i = 0; i < ptrArray.Length; i++)
                ptrArray[i] = marshalElem(list[i], ref unamanged);
            var result = (IntPtr*)unamanged;
            for (int i = 0; i < ptrArray.Length; i++)
                result[i] = ptrArray[i];
            unamanged += IntPtr.Size * ptrArray.Length;
            return result;
        }
    }
}