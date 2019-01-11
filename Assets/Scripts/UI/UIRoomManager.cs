using UnityEngine;
using Verse.Systems.Visual;

public class UIRoomManager : MonoBehaviour {
    public static UIRoomManager Instance;
    public MapEditorToolbarRules mapEditorToolbarRules;
    public GameObject mapSelector;
    public SelectedTileController TileController;

    private RoomController _roomController;

    void Awake() {
        Instance = this;
    }

    void Start() {
        _roomController = RoomController.Instance;
    }

    public void CreateNewRoom() {
        Debug.Log("Create new room");
    }

    public void CloseRoom() {
        if (_roomController.HasActiveRoom) {
            _roomController.DestroyRoom();
            mapEditorToolbarRules.Refresh();
        }
    }

    public void LoadRoom(string room) {
        RoomController.Instance.ChangeRoom(room, null);
        Camera.main.GetComponent<CameraPanAndZoom>().Reset();
        mapEditorToolbarRules.Refresh();
        mapSelector.SetActive(false);
        TileController.LoadRoom(room);
    }
}