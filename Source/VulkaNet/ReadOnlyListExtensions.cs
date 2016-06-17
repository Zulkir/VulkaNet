using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public static class ReadOnlyListExtensions
    {
        public static int SafeMarshalSize(this IReadOnlyList<string> list) 
            => SafeMarshalReferenceSize(list, StringExtensions.SafeMarshalSize);
        public static int SafeMarshalSize(this IReadOnlyList<IVkStructWrapper> list)
            => SafeMarshalReferenceSize(list, x => x.SafeMarshalSize());
        public static int SafeMarshalReferenceSize<T>(this IReadOnlyList<T> list, Func<T, int> marshalElemSize) 
            => list != null ? list.Sum(marshalElemSize) + IntPtr.Size * list.Count : 0;

        public static int SafeMarshalSize(this IReadOnlyList<float> list)
            => SafeMarshalStructSize(list, sizeof(float));
        public static int SafeMarshalStructSize<T>(this IReadOnlyList<T> list, int elemSize)
            => list?.Count * elemSize ?? 0;

        public unsafe delegate IntPtr MarshalElemDelegate<T>(T elem, ref byte* unmanaged);

        private static unsafe IntPtr SafeMarshalStringTo(string elem, ref byte* unmanaged) 
            => (IntPtr)elem.SafeMarshalTo(ref unmanaged);
        public static unsafe byte** SafeMarshalTo(this IReadOnlyList<string> list, ref byte* unamanged) 
            => (byte**)SafeMarshalReferencesTo(list, ref unamanged, SafeMarshalStringTo);

        private static unsafe IntPtr SafeMarshalStructWrapperTo(IVkStructWrapper elem, ref byte* unmanaged)
            => (IntPtr)elem.SafeMarshalTo(ref unmanaged);
        public static unsafe void** SafeMarshalTo(this IReadOnlyList<IVkStructWrapper> list, ref byte* unamanged)
            => (void**)SafeMarshalReferencesTo(list, ref unamanged, SafeMarshalStructWrapperTo);

        public static unsafe IntPtr* SafeMarshalReferencesTo<T>(this IReadOnlyList<T> list, ref byte* unamanged, MarshalElemDelegate<T> marshalElem)
            where T : class
        {
            if (list == null)
                return (IntPtr*)0;
            var ptrArray = new IntPtr[list.Count];
            for (int i = 0; i < list.Count; i++)
                ptrArray[i] = marshalElem(list[i], ref unamanged);
            var result = (IntPtr*)unamanged;
            for (int i = 0; i < ptrArray.Length; i++)
                result[i] = ptrArray[i];
            unamanged += IntPtr.Size * ptrArray.Length;
            return result;
        }

        private static unsafe IntPtr MarshalFloatTo(float elem, ref byte* unmanaged)
        {
            var result = (float*)unmanaged;
            *result = elem;
            unmanaged += sizeof(float);
            return (IntPtr)result;
        }
        public static unsafe float* SafeMarshalTo(this IReadOnlyList<float> list, ref byte* unamanged)
            => (float*)SafeMarshalStructsTo(list, ref unamanged, MarshalFloatTo);

        public static unsafe IntPtr SafeMarshalStructsTo<T>(this IReadOnlyList<T> list, ref byte* unamanged, MarshalElemDelegate<T> marshalElem)
            where T : struct
        {
            if (list == null)
                return (IntPtr)0;
            var result = (IntPtr)unamanged;
            foreach (var elem in list)
                marshalElem(elem, ref unamanged);
            return result;
        }
    }
}