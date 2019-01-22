using UnityEngine;

namespace Verse.API.Models {
    public sealed class TileActual : Tile {
        public override TileDef Definition { get; protected set; }

        public override Vector2Int Position {
            get { return _position; }
            set {
                Debug.LogError("Tile Position change not yet implemented");
                _position = value;
            }
        }

        private Vector2Int _position;

        public override Room Room { get; protected set; }
        public override TileLayer Layer { get; protected set; }
        public override TileEntity Entity { get; protected set; }

        public override bool IsRegistered { get; protected set; }

        public override bool Unregister() {
            return false;
        }

        public override bool Register() {
            return false;
        }

        public TileActual(TileDef definition, Vector2Int position, Room room, TileLayer layer) {
            Definition = definition;
            Position = position;
            Room = room;
            Layer = layer;
            Entity = definition.TileEntityDefault.Clone();
            RegisterTile();
        }

        public TileActual(TileDef definition, Vector2Int position, Room room, TileLayer layer,
            TileEntity entity) {
            Definition = definition;
            Position = position;
            Room = room;
            Layer = layer;
            Entity = entity;
            RegisterTile();
        }

        private void RegisterTile() {
            Room.Tiles.Add(this);
        }
    }
}