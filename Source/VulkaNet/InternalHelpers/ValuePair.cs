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

namespace VulkaNet.InternalHelpers
{
    public struct ValuePair<TFirst, TSecond> : IEquatable<ValuePair<TFirst, TSecond>>
        where TFirst : struct, IEquatable<TFirst>
        where TSecond : struct, IEquatable<TSecond>
    {
        public readonly TFirst First;
        public readonly TSecond Second;

        public ValuePair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        #region Equality, Hash, String
        public override int GetHashCode() => 
            First.GetHashCode() ^
            (Second.GetHashCode() << 1);

        public override string ToString() => 
            $"{{{First}, {Second}}}";

        public static bool Equals(ValuePair<TFirst, TSecond> s1, ValuePair<TFirst, TSecond> s2) => 
            s1.First.Equals(s2.First) &&
            s1.Second.Equals(s2.Second);

        public static bool operator ==(ValuePair<TFirst, TSecond> s1, ValuePair<TFirst, TSecond> s2) => Equals(s1, s2);
        public static bool operator !=(ValuePair<TFirst, TSecond> s1, ValuePair<TFirst, TSecond> s2) => !Equals(s1, s2);
        public bool Equals(ValuePair<TFirst, TSecond> other) => Equals(this, other);
        public override bool Equals(object obj) => obj is ValuePair<TFirst, TSecond> && Equals((ValuePair<TFirst, TSecond>)obj);
        #endregion
    }
}