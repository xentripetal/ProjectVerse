using System;

namespace Verse.API.Models {
    public struct Position {
        public readonly int x;
        public readonly int y;

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return "Position(" + x + ", " + y + ")";
        }
        
        public float SqrMagnitude => x * x + y * y;

        public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

        public static float Distance(Position from, Position to) {
            return (from - to).Magnitude;
        }

        public static Position Zero = new Position(0, 0);
        public static Position MaxValue = new Position(int.MaxValue, int.MaxValue);
        public static Position MinValue = new Position(int.MinValue, int.MinValue);


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
        
    }
}