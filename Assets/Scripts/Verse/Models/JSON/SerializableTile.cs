using System.Collections.Generic;
using UnityEngine;
using Verse.API.Interfaces;

namespace Verse.API.Models.JSON {
    public class SerializableTileUnified {
        public string Definition;
        public Vector2Int Position;
        public TileEntity TileEntity;

        public TileUnified ToTile(Room room, TileLayer layer) {
            return new TileUnified(TileDefMap.GetTileDef(Definition), Position, room, layer, TileEntity);
        }
    }

    public class SerializableTile {
        public string Definition;
        public TilePosition Position;

        public SerializableTile(string definition, TilePosition position) {
            Definition = definition;
            Position = position;
        }

        public SerializableTile() { }

        public Tile ToTile(RoomOld roomOld) {
            return new TileActual(TileDefMapOld.GetTileDef(Definition), Position, roomOld);
        }

        static public implicit operator SerializableTile(Tile value) {
            return new SerializableTile(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableTileObject : SerializableTile {
        public SerializableTileObject(string definition, TilePosition position) : base(definition, position) { }

        public SerializableTileObject() { }

        public TileObject ToTileObject(RoomOld roomOld) {
            return new TileObjectActual(TileDefMapOld.GetTileObjectDef(Definition), Position, roomOld);
        }

        static public implicit operator SerializableTileObject(TileObject value) {
            return new SerializableTileObject(value.Definition.FullName, value.Position);
        }
    }

    public class SerializableTileObjectEntity : SerializableTileObject {
        public List<IThingData> Datasets;

        public SerializableTileObjectEntity(string definition, TilePosition position, List<IThingData> datasets) : base(
            definition, position) {
            Datasets = datasets;
        }

        public SerializableTileObjectEntity() { }

        public TileObjectEntity ToScriptableTileObject(RoomOld roomOld) {
            return new TileObjectEntityActual(TileDefMapOld.GetScriptableTileObjectDef(Definition), Position, roomOld,
                Datasets);
        }

        static public implicit operator SerializableTileObjectEntity(TileObjectEntity value) {
            return new SerializableTileObjectEntity(value.Definition.FullName, value.Position, value.Datasets);
        }
    }
}