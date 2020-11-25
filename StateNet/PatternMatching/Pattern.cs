using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.PatternMatching
{
    public class Pattern : IEnumerable<int?>, IEquatable<Pattern>
    {
        public static readonly Pattern Empty = new Pattern();
        public readonly string?[] Elements;

        public readonly int?[] HashedElements;
        public readonly int Length;

        public Pattern(params string[] elements)
        {
            if (elements == null)
            {
                elements = new string[0];
            }

            Elements = elements;
            HashedElements = elements.Select(x => x?.GetDeterministicHashCode()).ToArray();
            Length = elements.Length;
        }

        public IEnumerator<int?> GetEnumerator() => HashedElements.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region IEquatable

        public override int GetHashCode()
        {
            return HashedElements
                .Select(item => item.GetHashCode())
                .Aggregate((total, nextCode) => total ^ nextCode);
        }

        public override bool Equals(object obj) => obj is Pattern pattern && Equals(pattern);

        public bool Equals(Pattern other) => this == other;

        public static bool operator ==(Pattern lhs, Pattern rhs)
        {
            if (lhs?.Length != rhs?.Length)
            {
                return false;
            }

            return lhs?.Length == null || lhs.HashedElements.SequenceEqual(rhs?.HashedElements);
        }

        public static bool operator !=(Pattern lhs, Pattern rhs) => !(lhs == rhs);

        #endregion
    }
}