using System;
using Unity.Mathematics;
using Verse.API;
using Verse.API.Interfaces;

namespace Verse.ThingScripts {
    public class DoorTriggerData : IThingData {
        public string room;
        public float x;
        public float y;

        public DoorTriggerData(string room, float x, float y) {
            this.room = room;
            this.x = x;
            this.y = y;
        }
    }

    public class DoorTrigger : ITrigger {
        public Type DataModel => typeof(DoorTriggerData);

        public void OnPlayerEnter(IPlayerReadOnly player, IThingData data) {
            var dat = (DoorTriggerData) data;
            player.RequestRoomChange(dat.room, new float2(dat.x, dat.y));
        }
    }
}