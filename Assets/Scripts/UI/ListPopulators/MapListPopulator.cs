using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Verse.API.Models;

public class MapListPopulator : MonoBehaviour {
    public GameObject ItemPrefab;
    public Transform VerticalLayoutParent;

    private SelectedRoomController _selectedRoomController;

    private void Awake() {
        _selectedRoomController = SelectedRoomController.Instance;
    }

    private void OnEnable() {
        SpawnGameobjects();
    }

    private void SpawnGameobjects() {
        var rooms = GetKnownRooms();
        var createNew = Instantiate(ItemPrefab, VerticalLayoutParent);
        createNew.GetComponent<Button>().onClick.AddListener(_selectedRoomController.CreateNewRoom);
        createNew.GetComponentInChildren<TextMeshProUGUI>().text = "Create New Room";

        foreach (var room in rooms) {
            var go = Instantiate(ItemPrefab, VerticalLayoutParent);
            go.GetComponent<Button>().onClick.AddListener(delegate { _selectedRoomController.LoadRoom(room); });
            go.GetComponentInChildren<TextMeshProUGUI>().text = room;
        }
    }

    private void OnDisable() {
        foreach (Transform child in VerticalLayoutParent) {
            Destroy(child.gameObject);
        }
    }

    private List<string> GetKnownRooms() {
        return RoomAtlas.GetRooms().Select(room => room.RoomName).ToList();
    }
}