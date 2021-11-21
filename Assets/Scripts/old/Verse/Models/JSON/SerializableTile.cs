using UnityEngine;

namespace Verse.API.Models.JSON {
    public class SerializableTileUnified {
        public string Definition;
        public Vector2Int Position;
        public TileEntity TileEntity;

        public Tile ToTile(Room room, TileLayer layer) {
            return new TileActual(TileDefMap.GetTileDef(Definition), Position, room, layer, TileEntity);
        }
    }
}