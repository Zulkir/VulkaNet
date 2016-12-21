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

using System;
using System.Collections.Generic;
using System.Linq;

namespace VulkaNet
{
    public static unsafe class ReadOnlyListExtensions
    {
        // Denotation:
        // MarshalDirect(list) returns a pointer to an array of values stored in 'unmanaged'.
        // MarshalIndirect(list) returns a pointer to an array of pointers stored in 'unmanaged'.
        // For objects, their handlers are considered values and therefore, MarshalDirect(list) is used.

        // Generic

        public static int SizeOfMarshalDirect<T>(this IReadOnlyList<T> list, int rawSize, Func<T, int> sizeOfMarshalElemDirect) =>
            list != null ? list.Sum(sizeOfMarshalElemDirect) + rawSize * list.Count : 0;

        public static int SizeOfMarshalIndirect<T>(this IReadOnlyList<T> list, Func<T, int> sizeOfMarshalElemIndirect) => 
            list != null ? list.Sum(sizeOfMarshalElemIndirect) + IntPtr.Size * list.Count : 0;

        public delegate void MarshalElemDirectDelegate<T>(T elem, ref byte* unmanaged, void* dst);

        public static void* MarshalDirect<T>(this IReadOnlyList<T> list, ref byte* unamanged, MarshalElemDirectDelegate<T> marshalElemDirect, int elemSize)
        {
            if (list == null || list.Count == 0)
                return (void*)0;
            var result = unamanged;
            unamanged += elemSize * list.Count;
            for (int i = 0; i < list.Count; i++)
                marshalElemDirect(list[i], ref unamanged, result + i * elemSize);
            return result;
        }

        public delegate void StoreElemDelegate<T>(T elem, void* dst);

        public static void* MarshalDirect<T>(this IReadOnlyList<T> list, ref byte* unamanged, StoreElemDelegate<T> storeElem, int elemSize) => 
            MarshalDirect(list, ref unamanged, (T e, ref byte* u, void* d) => storeElem(e, d), elemSize);

        public delegate void* MarshalElemIndirectDelegate<T>(T elem, ref byte* unmanaged);

        private static void** MarshalIndirect<T>(this IReadOnlyList<T> list, ref byte* unamanged, MarshalElemIndirectDelegate<T> marshalElemIndirect)
            where T : class
        {
            if (list == null || list.Count == 0)
                return (void**)0;
            var ptrArray = new void*[list.Count];
            for (int i = 0; i < list.Count; i++)
                ptrArray[i] = marshalElemIndirect(list[i], ref unamanged);
            var result = (void**)unamanged;
            unamanged += IntPtr.Size * ptrArray.Length;
            for (int i = 0; i < ptrArray.Length; i++)
                result[i] = ptrArray[i];
            return result;
        }

        // Float

        public static int SizeOfMarshalDirect(this IReadOnlyList<float> list) =>
            SizeOfMarshalDirect(list, sizeof(float), x => 0);

        public static float* MarshalDirect(this IReadOnlyList<float> list, ref byte* unmanaged) =>
            (float*)MarshalDirect(list, ref unmanaged, (e, d) => { *(float*)d = e; }, sizeof(float));

        // String

        public static int SizeOfMarshalIndirect(this IReadOnlyList<string> list) =>
            SizeOfMarshalIndirect(list, x => x.SizeOfMarshalIndirect());

        public static byte** MarshalIndirect(this IReadOnlyList<string> list, ref byte* unmanaged) =>
            (byte**)MarshalIndirect(list, ref unmanaged, (string e, ref byte* u) => (void*)e.MarshalIndirect(ref u));

        // Dispatched Handle

        public static int SizeOfMarshalDirectDispatchable<T>(this IReadOnlyList<T> list) where T : IVkHandledObject =>
            SizeOfMarshalDirect(list, sizeof(IntPtr), x => 0);

        public static IntPtr* MarshalDirectDispatchable<T>(this IReadOnlyList<T> list, ref byte* unmanaged) where T : IVkHandledObject =>
            (IntPtr*)MarshalDirect(list, ref unmanaged, (e, d) => { *(IntPtr*)d = e.RawHandle; }, sizeof(IntPtr));

        // Non-Dispatched Handle

        public static int SizeOfMarshalDirectNonDispatchable<T>(this IReadOnlyList<T> list) where T : IVkNonDisptatchableHandledObject =>
            SizeOfMarshalDirect(list, sizeof(ulong), x => 0);

        public static ulong* MarshalDirectNonDispatchable<T>(this IReadOnlyList<T> list, ref byte* unmanaged) where T : IVkNonDisptatchableHandledObject =>
            (ulong*)MarshalDirect(list, ref unmanaged, (e, d) => { *(ulong*)d = e.RawHandle; }, sizeof(IntPtr));
    }
}