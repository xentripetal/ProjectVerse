using System;

namespace Verse.API.Models.JSON {
    public class SerializableRoom {
        public string Name;
        public RoomColliders Colliders;
        public SerializableLayers[] Layers;
    }

    public class SerializableLayers {
        public Type LayerType;
        public SerializableTileUnified[] Tiles;
    }
}