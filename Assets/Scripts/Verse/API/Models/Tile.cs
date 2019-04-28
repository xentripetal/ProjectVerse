using UnityEngine;

namespace Verse.API.Models {
    public abstract class Tile {
        public abstract TileDef Definition { get; protected set; }
        public abstract Vector2Int Position { get; set; }
        public abstract Room Room { get; protected set; }
        public abstract TileLayer Layer { get; protected set; }
        public abstract TileEntity Entity { get; protected set; }
        public abstract bool IsRegistered { get; protected set; }

        //Todo seperate TileActual call from API assembly
        /// <summary>
        ///     Creates an unregisted tile.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="position"></param>
        /// <param name="room"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static Tile Create(TileDef definition, Vector2Int position, Room room, TileLayer layer) {
            return new TileActual(definition, position, room, layer);
        }

        /// <summary>
        ///     Unregisters the tile from the world
        /// </summary>
        /// <returns></returns>
        public abstract bool Unregister();

        /// <summary>
        ///     Registers the current tile into the Room.
        /// </summary>
        public abstract bool Register();
    }
}