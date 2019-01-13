using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Verse.API.Models;

public class SelectedTileController : MonoBehaviour, IEventSystemHandler {
    public TileSelectedEvent onTileSelected = new TileSelectedEvent();
    public TileUnselectedEvent onTileUnselected = new TileUnselectedEvent();

    private Room _room;
    private Camera _camera;

    private Tile _selectedTile;

    private void Awake() {
        _camera = Camera.main;
    }

    public void LoadRoom(Room room) {
        _room = room;
    }

    public void UnloadRoom(Room room) {
        _room = null;
        TileUnselected();
    }

    public void TileDestroyed(Tile tile) {
        if (_selectedTile == tile) {
            TileUnselected();
        }
    }

    private Tile GetTileAt(TilePosition pos) {
        Tile tile = _room.TileProvider.GetOptionalScriptableTileObject(pos);
        if (tile == null) {
            tile = _room.TileProvider.GetTileAtOrDefault(pos);
        }

        return tile;
    }

    private void TileUnselected() {
        if (_selectedTile == null) {
            return;
        }

        onTileUnselected.Invoke(_selectedTile);
        _selectedTile = null;
    }

    private void TileSelected() {
        if (_selectedTile == null) {
            return;
        }

        onTileSelected.Invoke(_selectedTile);
    }

    // Update is called once per frame
    void Update() {
        if (_room == null) {
            return;
        }

        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            TileUnselected();
            return;
        }

        var pos = new TilePosition((int) mousePos.x, (int) mousePos.y);

        var tile = GetTileAt(pos);
        if (tile == null) {
            TileUnselected();
            return;
        }

        _selectedTile = tile;
        TileSelected();
    }

    [Serializable]
    public class TileSelectedEvent : UnityEvent<Tile> { }

    [Serializable]
    public class TileUnselectedEvent : UnityEvent<Tile> { }
}