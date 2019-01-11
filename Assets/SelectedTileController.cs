using UnityEngine;
using Verse.API.Models;
using Verse.Systems.Visual;

public class SelectedTileController : MonoBehaviour {
    private Room _room;
    private Camera _camera;
    private RoomController _roomController = RoomController.Instance;

    private void Awake() {
        _camera = Camera.main;
    }

    public void LoadRoom(string room) {
        _room = RoomAtlas.GetRoom(room);
    }

    // Update is called once per frame
    void Update() {
        if (_room == null) {
            return;
        }

        if (!Input.GetMouseButtonDown(0)) {
            return;
        }

        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < 0 || mousePos.y < 0) {
            return;
        }

        var pos = new TilePosition((int) mousePos.x, (int) mousePos.y);
        Tile tile = _room.TileProvider.GetScriptableTileObjectAtOrDefault(pos);
        if (tile == null) {
            tile = _room.TileProvider.GetTileObjectAtOrDefault(pos);

            if (tile == null) {
                tile = _room.TileProvider.GetTileAtOrDefault(pos);
                if (tile == null) {
                    return;
                }
            }
        }
    }
}