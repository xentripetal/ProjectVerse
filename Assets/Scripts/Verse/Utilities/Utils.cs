using System;
using System.Collections.Generic;

namespace Verse.Utilities {
    public static class Utils {
        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            return InternalDropLast(source);
        }

        private static IEnumerable<T> InternalDropLast<T>(IEnumerable<T> source) {
            var buffer = default(T);
            var buffered = false;

            foreach (var x in source) {
                if (buffered)
                    yield return buffer;

                buffer = x;
                buffered = true;
            }
        }
    }
}