using Verse.API.Models;
using Verse.Systems.Visual;

namespace Verse.API {
    internal class PlayerActual : Player {
        public delegate void PlayerTeleported(Position position);

        public PlayerActual() {
            Height = 1.1f;
            RunSpeed = 4f;
            WalkSpeed = 2.2f;
            Main = this;
        }

        public override Position Position { get; protected set; }

        public override Position PositionDelta { get; set; }

        public event PlayerTeleported OnPlayerTeleported;

        public void SetPosition(Position position) {
            Position = position;
        }

        public override void TeleportPlayer(Position position) {
            if (OnPlayerTeleported != null) OnPlayerTeleported(position);
        }

        public override void ChangeRoom(string room, Position pos) {
            RoomController.Instance.ChangeRoom(room);
            CurrentRoom = room;
            TeleportPlayer(pos);
        }
    }
}