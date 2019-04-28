using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Verse.API.Models;

public class SelectedTileController : MonoBehaviour, IEventSystemHandler {
    private Camera _camera;

    private Tile _selectedTile;
    private UIEditorState _state;
    public TileSelectedEvent onTileSelected = new TileSelectedEvent();
    public TileUnselectedEvent onTileUnselected = new TileUnselectedEvent();

    private void Awake() {
        _camera = Camera.main;
        _state = GetComponent<UIEditorState>();
    }

    public void UnloadRoom(Room roomOld) {
        TileUnselected();
    }

    public void TileDestroyed(Tile tile) {
        if (_selectedTile == tile) TileUnselected();
    }

    private void TileUnselected() {
        if (_selectedTile == null) return;

        onTileUnselected.Invoke(_selectedTile);
        _selectedTile = null;
    }

    private void TileSelected() {
        if (_selectedTile == null) return;

        onTileSelected.Invoke(_selectedTile);
    }

    // Update is called once per frame
    private void Update() {
        if (!_state.IsRoomLoaded) return;

        if (!Input.GetMouseButtonDown(0)) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            TileUnselected();
            return;
        }

        var pos = new Vector2Int((int) mousePos.x, (int) mousePos.y);

        var tile = _state.GetTileAt(pos);
        if (tile == null) {
            TileUnselected();
            return;
        }

        _selectedTile = tile;
        TileSelected();
    }

    [Serializable]
    public class TileSelectedEvent : UnityEvent<Tile> {
    }

    [Serializable]
    public class TileUnselectedEvent : UnityEvent<Tile> {
    }
}