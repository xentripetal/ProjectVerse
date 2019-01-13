using UnityEngine;
using Verse.API.Models;

public class UIEditorState : MonoBehaviour {
    public Room CurrentRoom { get; protected set; }
    public Tile CurrentSelectedTile { get; protected set; }
    public bool IsRoomLoaded { get; protected set; }
    public bool IsTileSelected { get; protected set; }

    public static UIEditorState Instance;

    private void Awake() {
        Instance = this;
    }

    public void RoomLoaded(Room room) {
        IsRoomLoaded = true;
        CurrentRoom = room;
    }

    public void RoomUnloaded(Room room) {
        IsRoomLoaded = false;
        CurrentRoom = null;
    }

    public void TileSelected(Tile tile) {
        CurrentSelectedTile = tile;
        IsTileSelected = true;
    }

    public void TileUnselected(Tile tile) {
        CurrentSelectedTile = null;
        IsTileSelected = false;
    }
}