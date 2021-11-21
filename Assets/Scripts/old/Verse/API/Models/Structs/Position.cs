using System;
using Newtonsoft.Json;

namespace Verse.API.Models {
    public struct Position {
        public bool Equals(Position other) {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj) {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public readonly float x;
        public readonly float y;

        public Position(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return "PlayerPosition(" + x + ", " + y + ")";
        }

        [JsonIgnore] public float SqrMagnitude => (float) (x * (double) x + y * (double) y);

        [JsonIgnore] public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

        public static float Distance(Position from, Position to) {
            return (from - to).Magnitude;
        }

        [JsonIgnore]
        public Position normalized {
            get {
                var magnitude = Magnitude;
                if (magnitude > 9.99999974737875E-06)
                    return this / magnitude;
                return Zero;
            }
        }

        [JsonIgnore]
        public TilePosition NearestTilePosition => new TilePosition((int) Math.Round(x), (int) Math.Round(y));

        [JsonIgnore] public TilePosition CurrentTilePosition => new TilePosition((int) x, (int) y);

        public static Position Zero = new Position(0, 0);
        public static Position Max = new Position(float.MaxValue, float.MaxValue);
        public static Position Min = new Position(float.MinValue, float.MinValue);


        public static Position operator +(Position a, Position b) {
            return new Position(a.x + b.x, a.y + b.y);
        }

        public static Position operator -(Position a, Position b) {
            return new Position(a.x - b.x, a.y - b.y);
        }

        public static Position operator *(Position a, Position b) {
            return new Position(a.x * b.x, a.y * b.y);
        }

        public static Position operator /(Position a, Position b) {
            return new Position(a.x / b.x, a.y / b.y);
        }

        public static Position operator *(float a, Position b) {
            return new Position(a * b.x, a * b.y);
        }

        public static Position operator *(Position a, float b) {
            return new Position(a.x * b, a.y * b);
        }

        public static Position operator /(float a, Position b) {
            return new Position(a / b.x, a / b.y);
        }

        public static Position operator /(Position a, float b) {
            return new Position(a.x / b, a.y / b);
        }

        public static bool operator ==(Position a, Position b) {
            return (a - b).SqrMagnitude < 9.99999943962493E-11;
        }

        public static bool operator !=(Position a, Position b) {
            return !(a == b);
        }
    }
}