using UnityEngine;
using Verse.API.Models;

public class SelectedTileOutline : MonoBehaviour {
    private SpriteRenderer _renderer;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    public void TileSelected(Tile tile) {
        /**
        _renderer.enabled = true;
        var width = tile.Definition.SpriteInfo.sprite.rect.width / tile.Definition.SpriteInfo.PixelsPerUnit;
        var height = tile.Definition.SpriteInfo.sprite.rect.height / tile.Definition.SpriteInfo.PixelsPerUnit;
        var pivotLocal = tile.Definition.SpriteInfo.PivotPoint * new Position(width, height);
        var vectorPos = new Vector3(tile.Position.x - pivotLocal.x, tile.Position.y - pivotLocal.y, 0);
        transform.position = vectorPos;
        _renderer.size = new Vector2(width, height);**/
    }

    public void TileUnselected() {
        _renderer.enabled = false;
    }
}