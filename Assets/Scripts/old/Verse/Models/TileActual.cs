using UnityEngine;

namespace Verse.API.Models {
    public sealed class TileActual : Tile {
        private Vector2Int _position;

        public TileActual(TileDef definition, Vector2Int position, Room room, TileLayer layer) {
            Definition = definition;
            _position = position;
            Room = room;
            Layer = layer;
            Entity = definition.TileEntityDefault.Clone();
        }

        public TileActual(TileDef definition, Vector2Int position, Room room, TileLayer layer,
            TileEntity entity) {
            Definition = definition;
            _position = position;
            Room = room;
            Layer = layer;
            Entity = entity;
        }

        public override TileDef Definition { get; protected set; }

        public override Vector2Int Position {
            get => _position;
            set {
                Debug.LogError("Tile Position change not yet implemented");
                _position = value;
            }
        }

        public override Room Room { get; protected set; }
        public override TileLayer Layer { get; protected set; }
        public override TileEntity Entity { get; protected set; }

        public override bool IsRegistered { get; protected set; }

        public override bool Unregister() {
            return false;
        }

        public override bool Register() {
            Room.Tiles.Add(this);
            return true;
        }
    }
}