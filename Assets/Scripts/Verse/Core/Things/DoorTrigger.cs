using System;
using Verse.API;
using Verse.API.Interfaces;
using Verse.API.Interfaces.Events;
using Verse.API.Models;

public class DoorTriggerData : IThingData {
    public string room;
    public float x;
    public float y;

    public DoorTriggerData() { }
}

public class DoorTrigger : ITrigger {
    public Type DataModel => typeof(DoorTriggerData);

    public void OnPlayerEnter(IThingData data) {
        var dat = (DoorTriggerData) data;
        Player.Main.ChangeRoom(dat.room, new Position(dat.x, dat.y));
    }
}