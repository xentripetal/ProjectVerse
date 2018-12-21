using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.API.Models;

namespace Verse.Utilities {
    public static class Utils {

        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            return InternalDropLast(source);
        }

        private static IEnumerable<T> InternalDropLast<T>(IEnumerable<T> source) {
            T buffer = default(T);
            bool buffered = false;

            foreach (T x in source) {
                if (buffered)
                    yield return buffer;

                buffer = x;
                buffered = true;
            }
        }
    }
}