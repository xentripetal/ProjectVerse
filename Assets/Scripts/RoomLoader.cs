using UnityEngine;

namespace Verse {
	public class RoomLoader : MonoBehaviour {
		private Room room;
		public GameObject triggerPrefab;

		public Room Room {
			get => room;
			set {
				LoadRoom(value);
				room = value;
			}
		}

		private TextMesh textMesh;

		private void Awake() {
			textMesh = GetComponent<TextMesh>();
		}

		private void LoadRoom(Room room) {
			textMesh.text = room.Name;
			if (room.Name == "RoomA") {
				var go = Instantiate(triggerPrefab, this.transform);
				go.transform.localPosition = Vector3.left * 5;
				go.GetComponent<RoomTrigger>().targetRoom = "roomB";
			}
			else if (room.Name == "RoomB") {
				var go = Instantiate(triggerPrefab, this.transform);
				go.transform.localPosition = Vector3.right* 5;
				go.GetComponent<RoomTrigger>().targetRoom = "roomA";
			}
		}
	}
}
