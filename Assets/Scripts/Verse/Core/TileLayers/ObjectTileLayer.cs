using UnityEngine;

namespace Verse.Core.TileLayers {
    public sealed class ObjectTileLayer : TileLayer {
        public static readonly float ZPositionMultiplier = -.01f;

        public readonly float ZPositionOffset = -10f;

        public ObjectTileLayer() {
            Name = "Object";
            SortingOrder = 5;
        }

        public override string Name { get; protected set; }
        public override int SortingOrder { get; protected set; }

        public override Vector3 TilePositionToVisualPosition(Vector2Int pos) {
            return new Vector3(pos.x, pos.y, pos.y * ZPositionMultiplier + ZPositionOffset);
        }
    }
}