using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Verse.Utilities;

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
        var folders = Directory.GetDirectories(Constants.RoomsFolder);
        var rooms = new List<string>();

        for (int i = 0; i < folders.Length; i++) {
            rooms.Add(folders[i].Split('/').Last());
        }

        return rooms;
    }
}