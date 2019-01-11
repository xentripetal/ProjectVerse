using UnityEngine;
using UnityEngine.UI;
using Verse.Systems.Visual;

public class MapEditorToolbarRules : MonoBehaviour {
    public Button[] OptionsToDisableOnNoActiveRoom;

    private void OnEnable() {
        Refresh();
    }

    public void Refresh() {
        var hasActiveRoom = RoomController.Instance.HasActiveRoom;
        foreach (var button in OptionsToDisableOnNoActiveRoom) {
            button.interactable = hasActiveRoom;
        }
    }
}