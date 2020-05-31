using System;
using Newtonsoft.Json;

namespace Verse.API.Models {
    public struct TilePosition {
        public bool Equals(TilePosition other) {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj) {
            return obj is TilePosition other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (x * 397) ^ y;
            }
        }

        public readonly int x;
        public readonly int y;

        public TilePosition(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return "TilePosition(" + x + ", " + y + ")";
        }

        [JsonIgnore] public int SqrMagnitude => x * x + y * y;

        [JsonIgnore] public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

        public static float Distance(TilePosition from, TilePosition to) {
            return (from - to).Magnitude;
        }

        public static TilePosition Zero = new TilePosition(0, 0);
        public static TilePosition MaxValue = new TilePosition(int.MaxValue, int.MaxValue);
        public static TilePosition MinValue = new TilePosition(int.MinValue, int.MinValue);


        public static TilePosition operator +(TilePosition a, TilePosition b) {
            return new TilePosition(a.x + b.x, a.y + b.y);
        }

        public static TilePosition operator -(TilePosition a, TilePosition b) {
            return new TilePosition(a.x - b.x, a.y - b.y);
        }

        public static TilePosition operator *(TilePosition a, TilePosition b) {
            return new TilePosition(a.x * b.x, a.y * b.y);
        }

        public static TilePosition operator /(TilePosition a, TilePosition b) {
            return new TilePosition(a.x / b.x, a.y / b.y);
        }

        public static bool operator ==(TilePosition a, TilePosition b) {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(TilePosition a, TilePosition b) {
            return !(a == b);
        }
    }
}