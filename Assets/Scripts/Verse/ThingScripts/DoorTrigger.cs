using System;
using Unity.Mathematics;
using Verse.API;
using Verse.API.Models;
using Verse.API.Scripting;

public class DoorTriggerData : IThingData {
    public string room;
    public float x;
    public float y;

    public DoorTriggerData() {
    }
}

public class DoorTrigger : ITrigger {
    public Type DataModel => typeof(DoorTriggerData);

    public void OnPlayerEnter(Player player, IThingData data) {
        var dat = (DoorTriggerData) data;
        player.ChangeRoom(dat.room, new PlayerPosition(dat.x, dat.y));
    }
}
