using UnityEngine;
using Verse.API.Models;

public class UIEditorState : MonoBehaviour {
    public static UIEditorState Instance;
    public Room CurrentRoom { get; protected set; }
    public Tile CurrentSelectedTile { get; protected set; }
    public TileDef CurrentSelectedTileDef { get; protected set; }
    public bool IsRoomLoaded { get; protected set; }
    public bool IsTileSelected { get; protected set; }
    public bool IsTileDefSelected { get; protected set; }
    public bool IsTilesLayerVisible { get; protected set; }
    public bool IsTileObjectsLayerVisible { get; protected set; }
    public bool IsBrushOpen { get; protected set; }


    private void Awake() {
        Instance = this;
        IsTilesLayerVisible = true;
        IsTileObjectsLayerVisible = true;
    }

    public void OpenBrush() {
        IsBrushOpen = true;
    }

    public void CloseBrush() {
        IsBrushOpen = false;
    }

    public void SetTilesLayerVisible(bool value) {
        IsTilesLayerVisible = value;
    }

    public void SetTileObjectLayersVisible(bool value) {
        IsTileObjectsLayerVisible = value;
    }

    public Tile GetTileAt(Vector2Int pos) {
        if (CurrentRoom == null) return null;

        return null;
    }

    public void TileDefSelected(TileDef def) {
        CurrentSelectedTileDef = def;
        IsTileDefSelected = true;
    }

    public void TileDefUnselected(TileDef def) {
        CurrentSelectedTileDef = null;
        IsTileDefSelected = false;
    }

    public void RoomLoaded(Room roomOld) {
        IsRoomLoaded = true;
        CurrentRoom = roomOld;
    }

    public void RoomUnloaded(Room roomOld) {
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