using Unity.Mathematics;
using UnityEngine;
using Verse.Systems.Visual;

namespace Verse.API {
    public class Player : IPlayerReadOnly {
        public float2 Position { get; set; }
        public bool IsMoving { get; set; }
        public bool IsRunning { get; set; }
        public string CurrentRoom { get; set; }
        public static float Height = 1.1f;

        private RoomController _roomController;
        private PlayerController _playerController;

        public Player() {
            Position = float2.zero;
            _roomController = RoomController.Instance;
            _playerController = PlayerController.Instance;
        }

        public void RequestRoomChange(string room, float2 pos) {
            Debug.Log("Room change requested to " + room + " at position " + pos);
            _roomController.ChangeRoom(room);
            MovePlayer(pos);
        }

        public void MovePlayer(float2 pos) {
            _playerController.DirectMovePlayer(pos);
        }
    }
}