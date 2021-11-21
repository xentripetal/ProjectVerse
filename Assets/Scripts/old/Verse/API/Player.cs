using Verse.API.Models;

namespace Verse.API {
    public abstract class Player {
        public static Player Main;
        public Position CurrentInputAxis;

        public float Height;

        public bool IsMoving;
        public bool IsRunning;

        public float RunSpeed;
        public SpriteInfo SpriteInfo;
        public float WalkSpeed;

        public virtual Position Position { get; protected set; }
        public virtual Position PositionDelta { get; set; }
        public string CurrentRoom { get; protected set; }

        public abstract void TeleportPlayer(Position position);
        public abstract void ChangeRoom(string room, Position position);
    }
}