using UnityEngine;
using Verse.API.Models;

public class UIEditorState : MonoBehaviour {
    public RoomOld CurrentRoomOld { get; protected set; }
    public Tile CurrentSelectedTile { get; protected set; }
    public TileDef CurrentSelectedTileDef { get; protected set; }
    public bool IsRoomLoaded { get; protected set; }
    public bool IsTileSelected { get; protected set; }
    public bool IsTileDefSelected { get; protected set; }
    public bool IsTilesLayerVisible { get; protected set; }
    public bool IsTileObjectsLayerVisible { get; protected set; }
    public bool IsBrushOpen { get; protected set; }

    public static UIEditorState Instance;


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

    public Tile GetTileAt(TilePosition pos) {
        if (CurrentRoomOld == null) {
            return null;
        }

        Tile value = null;
        if (IsTileObjectsLayerVisible) {
            value = CurrentRoomOld.TileProviderOld.GetOptionalScriptableTileObject(pos);
        }

        if (IsTilesLayerVisible && value == null) {
            value = CurrentRoomOld.TileProviderOld.GetTileAtOrDefault(pos);
        }

        return value;
    }

    public void TileDefSelected(TileDef def) {
        CurrentSelectedTileDef = def;
        IsTileDefSelected = true;
    }

    public void TileDefUnselected(TileDef def) {
        CurrentSelectedTileDef = null;
        IsTileDefSelected = false;
    }

    public void RoomLoaded(RoomOld roomOld) {
        IsRoomLoaded = true;
        CurrentRoomOld = roomOld;
    }

    public void RoomUnloaded(RoomOld roomOld) {
        IsRoomLoaded = false;
        CurrentRoomOld = null;
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