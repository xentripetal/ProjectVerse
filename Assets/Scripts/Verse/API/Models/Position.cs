using Unity.Mathematics;
using UnityEngine;

namespace Verse.API.Models {
    public struct Position {
        public float x;
        public float y;

        public Position(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public static Position Zero() {
            return new Position(0,0);
        }

        public override string ToString() {
            return "Position(" + x + ", " + y + ")";
        }

        static public implicit operator float2(Position value) {
            return new float2(value.x, value.y);
        }
        
        static public implicit operator Position(float2 value) {
            return new Position(value.x, value.y);
        }
        
        static public implicit operator Vector2(Position value) {
            return new Vector2(value.x, value.y);
        }
        
        static public implicit operator Position(Vector2 value) {
            return new Position(value.x, value.y);
        }
        
        static public explicit operator Vector3(Position value) {
            return new Vector3(value.x, value.y, 0);
        }
        
        static public explicit operator Position(Vector3 value) {
            return new Position(value.x, value.y);
        }
    }
}