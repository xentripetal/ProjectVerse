using System;
using System.Collections.Generic;
using System.Linq;
using Verse.API.Models.JSON;

namespace Verse.API.Models {
    public class Room {
        public string Name { get; protected set; }
        public RoomColliders Colliders { get; protected set; }
        public TileLayer[] Layers { get; protected set; }
        public TileProvider Provider { get; protected set; }

        public Room(SerializableRoom sRoom) {
            Name = sRoom.Name;
            Colliders = sRoom.Colliders;

            var layerDict = new Dictionary<TileLayer, TileUnified[]>();
            var layers = new List<TileLayer>();
            foreach (var sLayer in sRoom.Layers) {
                var layer = (TileLayer) Activator.CreateInstance(sLayer.LayerType);
                layers.Add(layer);
                layerDict.Add(layer, sLayer.Tiles.Select(tile => tile.ToTile(this, layer)).ToArray());
            }

            Layers = layers.ToArray();
            Provider = new TileProvider(layerDict);
        }
    }
}