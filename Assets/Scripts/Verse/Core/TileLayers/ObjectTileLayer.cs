using UnityEngine;

namespace Verse.Core.TileLayers {
    public sealed class ObjectTileLayer : TileLayer {
        public override string Name { get; protected set; }
        public override int SortingOrder { get; protected set; }

        public readonly float ZPositionOffset = -10f;
        public static readonly float ZPositionMultiplier = -.01f;

        public override Vector3 TilePositionToVisualPosition(Vector2Int pos) {
            return new Vector3(pos.x, pos.y, pos.y * ZPositionMultiplier + ZPositionOffset);
        }

        public ObjectTileLayer() {
            Name = "Object";
            SortingOrder = 5;
        }
    }
}