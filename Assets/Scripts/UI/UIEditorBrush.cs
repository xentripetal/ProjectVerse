using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Verse.API.Interfaces;
using Verse.API.Models;

public class UIEditorBrush : MonoBehaviour {
    public BrushToolMode currentToolMode = BrushToolMode.Create;
    public UIEditorState UIEditorState;
    public SelectedTileDefController SelectedTileDefController;

    private EventSystem _eventSystem;
    private Camera _cam;

    private Tile _lastCreatedTile;
    private TilePosition _lastPos;

    public void SetModeCreate(bool value) {
        if (value) {
            currentToolMode = BrushToolMode.Create;
        }
    }

    public void SetModeDestroy(bool value) {
        if (value) {
            currentToolMode = BrushToolMode.Destroy;
        }
    }

    public void SetModeEydropper(bool value) {
        if (value) {
            currentToolMode = BrushToolMode.Eyedropper;
        }
    }

    private void Awake() {
        _eventSystem = EventSystem.current;
        _cam = Camera.main;
        _lastCreatedTile = null;
    }

    void Update() {
        if (!UIEditorState.IsBrushOpen) {
            return;
        }

        if (!Input.GetMouseButton(0)) {
            _lastCreatedTile = null;
            return;
        }

        if (_eventSystem.IsPointerOverGameObject()) {
            return;
        }

        switch (currentToolMode) {
            case BrushToolMode.Create:
                HandleCreateMode();
                break;
            case BrushToolMode.Destroy:
                HandleDestroyMode();
                break;
            case BrushToolMode.Eyedropper:
                HandleEyedropperMode();
                break;
        }
    }

    private void HandleCreateMode() {
        if (!UIEditorState.IsTileDefSelected) {
            return;
        }

        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            return;
        }

        var pos = new TilePosition((int) mousePos.x, (int) mousePos.y);
        var tile = UIEditorState.GetTileAt(pos);
        if (tile != null) {
            if (tile.Definition == UIEditorState.CurrentSelectedTileDef && tile.Position == pos ||
                tile == _lastCreatedTile) {
                return;
            }
        }

        Tile newtile = null;
        switch (UIEditorState.CurrentSelectedTileDef.TileType) {
            case TileType.TileObjectEntity:
                newtile = new TileObjectEntityActual((TileObjectEntityDef) UIEditorState.CurrentSelectedTileDef, pos,
                    UIEditorState.CurrentRoom, new List<IThingData>());
                break;
            case TileType.TileObject:
                newtile = new TileObjectActual((TileObjectDef) UIEditorState.CurrentSelectedTileDef, pos,
                    UIEditorState.CurrentRoom);
                break;
            case TileType.Tile:
                newtile = new TileActual(UIEditorState.CurrentSelectedTileDef, pos, UIEditorState.CurrentRoom);
                break;
        }

        TileOperationsHandler.AddTile(newtile);
        _lastCreatedTile = newtile;
    }

    private void HandleDestroyMode() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            return;
        }

        var pos = new TilePosition((int) mousePos.x, (int) mousePos.y);
        if (pos == _lastPos) {
            return;
        }


        var tile = UIEditorState.GetTileAt(pos);
        if (tile == null) {
            return;
        }

        TileOperationsHandler.DestroyTile(tile);
        _lastPos = pos;
    }

    private void HandleEyedropperMode() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            return;
        }

        var pos = new TilePosition((int) mousePos.x, (int) mousePos.y);
        var tile = UIEditorState.GetTileAt(pos);
        if (tile == null) {
            return;
        }

        SelectedTileDefController.TileDefSelectedInternal(tile.Definition);
    }
}

public enum BrushToolMode {
    Create,
    Destroy,
    Eyedropper
}