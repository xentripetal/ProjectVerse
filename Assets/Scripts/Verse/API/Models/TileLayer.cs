using UnityEngine;

public abstract class TileLayer {
    public abstract string Name { get; protected set; }
    public abstract int SortingOrder { get; protected set; }

    public abstract Vector3 TilePositionToVisualPosition(Vector2Int pos);
}