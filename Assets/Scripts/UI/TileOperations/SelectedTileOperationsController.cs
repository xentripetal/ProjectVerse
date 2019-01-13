using UI;
using UnityEngine;

public class SelectedTileOperationsController : MonoBehaviour {
    private TileOperationsHandler _handler;

    private void Awake() {
        _handler = GetComponent<TileOperationsHandler>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            _handler.Undo();
            return;
        }

        if (!UIEditorState.Instance.IsTileSelected) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Delete)) {
            var tile = UIEditorState.Instance.CurrentSelectedTile;
            _handler.DestroyTile(tile);
        }
    }
}