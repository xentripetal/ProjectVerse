using System;

namespace Verse.API.Models.JSON {
    public class SerializableRoom {
        public RoomColliders Colliders;
        public SerializableLayers[] Layers;
        public string Name;
    }

    public class SerializableLayers {
        public Type LayerType;
        public SerializableTileUnified[] Tiles;
    }
}