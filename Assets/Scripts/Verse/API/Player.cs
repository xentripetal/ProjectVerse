using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Verse.API.Models;

namespace Verse.API {
    public class Player {
        public PlayerPosition Position { get; private set; }
        public string CurrentRoom { get; private set; }

        public float Height { get; private set; }
        public SpriteInfo SpriteInfo { get; private set; }

        public float RunSpeed { get; private set; }
        public float WalkSpeed { get; private set; }

        public delegate void RequestedPlayerMove(PlayerPosition posDifference);

        /// <summary>
        /// Called when Player.Move is called, This is used by the physics system to validate the
        /// requested position and then pass it back to MoveWithoutPhysics
        /// </summary>
        public event RequestedPlayerMove OnRequestedPlayerMove;

        public delegate void PlayerMoved(PlayerPosition newPosition, PlayerPosition oldPosition);

        /// <summary>
        /// Called when MoveWithoutPhysics is called. The physics system will also call it after a Move if the given
        /// move position is valid.
        /// </summary>
        public event PlayerMoved OnPlayerMoved;

        public delegate void BeforeRoomChange(string newRoom, string oldRoom);
        public delegate void DuringRoomChange(string newRoom, string oldRoom);
        public delegate void PostRoomChange(string newRoom, string oldRoom);

        public event BeforeRoomChange OnBeforeRoomChange;
        public event DuringRoomChange OnRoomChange;
        public event PostRoomChange OnPostRoomChange;

        public static Player Instance;

        public Player() {
            Height = 1.1f;
            WalkSpeed = 2.2f;
            RunSpeed = 4f;
            Position = PlayerPosition.Zero;
            Instance = this;
        }
        
        public void ChangeRoom(string room, PlayerPosition pos) {
            var prevRoom = CurrentRoom;
            if (OnBeforeRoomChange != null) {
                OnBeforeRoomChange(room, prevRoom);
            }
            CurrentRoom = room;
            MoveToWithoutPhysics(pos);
            if (OnRoomChange != null) {
                OnRoomChange(room, prevRoom);
            }

            if (OnPostRoomChange != null) {
                OnPostRoomChange(room, prevRoom);
            }
        }

        public void MoveToWithoutPhysics(PlayerPosition position) {
            var previousPosition = Position;
            Position = position;
            if (OnPlayerMoved != null) {
                OnPlayerMoved(position, previousPosition);
            }
        }

        public void Move(PlayerPosition posDifference) {
            if (OnRequestedPlayerMove != null) {
                OnRequestedPlayerMove(posDifference);
            }
        }

    }
}