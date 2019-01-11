using UnityEngine;
using Verse.API;
using Verse.API.Interfaces.Events;
using Verse.API.Models;

namespace Verse.Core.Systems {
    public static class PlayerController {
        [OnFrameUpdate]
        public static void OnFrameUpdate() {
            var player = Player.Main;
            var input = GetInputAxis();

            if (input == Position.Zero) {
                player.IsMoving = false;
                player.IsRunning = false;
                return;
            }

            player.IsMoving = true;
            player.CurrentInputAxis = input;

            var speed = GetPlayerSpeed(player);
            player.PositionDelta += CalculatePlayerDelta(input, speed);
        }

        private static Position CalculatePlayerDelta(Position input, float speed) {
            return input.normalized * Time.deltaTime * speed;
        }

        private static float GetPlayerSpeed(Player player) {
            if (MappedInput.GetKey("Speed Modifier")) {
                player.IsRunning = false;
                return player.WalkSpeed;
            }

            player.IsRunning = true;
            return player.RunSpeed;
        }

        private static Position GetInputAxis() {
            Position pos = new Position();
            if (MappedInput.GetKey("Up"))
                pos += new Position(0, 1);
            if (MappedInput.GetKey("Down"))
                pos += new Position(0, -1);
            if (MappedInput.GetKey("Left"))
                pos += new Position(-1, 0);
            if (MappedInput.GetKey("Right"))
                pos += new Position(1, 0);
            return pos;
        }
    }
}