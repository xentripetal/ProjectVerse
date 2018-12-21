using System;
namespace Verse.API.Models {
    public struct PlayerPosition {
        public readonly float x;
        public readonly float y;

        public PlayerPosition(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return "PlayerPosition(" + x + ", " + y + ")";
        }

        public float SqrMagnitude => (float) (x * (double) x + y * (double) y);

        public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

        public static float Distance(PlayerPosition from, PlayerPosition to) {
            return (from - to).Magnitude;
        }

        public Position NearestPosition => new Position((int) Math.Round(x), (int) Math.Round(y));
        public Position CurrentPosition => new Position((int) x, (int) y);
        
        public static PlayerPosition Zero = new PlayerPosition(0, 0);
        public static PlayerPosition Max = new PlayerPosition(float.MaxValue, float.MaxValue);
        public static PlayerPosition Min = new PlayerPosition(float.MinValue, float.MinValue);


        public static PlayerPosition operator +(PlayerPosition a, PlayerPosition b) {
            return new PlayerPosition(a.x + b.x, a.y + b.y);
        }
        
        public static PlayerPosition operator -(PlayerPosition a, PlayerPosition b) {
            return new PlayerPosition(a.x - b.x, a.y - b.y);
        }
        
        public static PlayerPosition operator *(PlayerPosition a, PlayerPosition b) {
            return new PlayerPosition(a.x * b.x, a.y * b.y);
        }

        public static PlayerPosition operator /(PlayerPosition a, PlayerPosition b) {
            return new PlayerPosition(a.x / b.x, a.y / b.y);
        }

        public static PlayerPosition operator *(float a, PlayerPosition b) {
            return new PlayerPosition(a * b.x, a * b.y);
        }

        public static PlayerPosition operator *(PlayerPosition a, float b) {
            return new PlayerPosition(a.x * b, a.x * b);
        }

        public static PlayerPosition operator /(float a, PlayerPosition b) {
            return new PlayerPosition(a / b.x, a / b.y);
        }

        public static PlayerPosition operator /(PlayerPosition a, float b) {
            return new PlayerPosition(a.x / b, a.y / b);
        }

        public static bool operator ==(PlayerPosition a, PlayerPosition b) {
            return (a-b).SqrMagnitude < 9.99999943962493E-11;
        }

        public static bool operator !=(PlayerPosition a, PlayerPosition b) {
            return !(a == b);
        }
        

    }
}