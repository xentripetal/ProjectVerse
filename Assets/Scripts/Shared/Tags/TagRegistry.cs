using System;
using System.Collections.Generic;

namespace DarkRiftTests.Shared.Tags {
    public static class TagRegistry {
        private static readonly Dictionary<string, ushort> _tagMap = new Dictionary<string, ushort>();
        
        public static ushort RegisterTag(string name) {
            // 0-255 are reserved for core tags.
            if (_tagMap.Count >= ushort.MaxValue - 256) {
                throw new ArgumentException($"Attempted to add too many tags with tag {name}");
            }

            var tag = (ushort)( _tagMap.Count + 256);
            _tagMap.Add(name, tag);

            return tag;
        }

        public static bool TryGetTag(string key, out ushort tag) {
            return _tagMap.TryGetValue(key, out tag);
        }
    }
}