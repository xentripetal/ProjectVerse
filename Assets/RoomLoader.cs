using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse {
	public class RoomLoader : MonoBehaviour {
		private string room;
		public GameObject triggerPrefab;

		public string Room {
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

		private void LoadRoom(String room) {
			textMesh.text = room;
			if (room == "room1") {
				var go = Instantiate(triggerPrefab, this.transform);
				go.transform.localPosition = Vector3.left * 5;
				go.GetComponent<RoomTrigger>().targetRoom = "room2";
			}
			else if (room == "room2") {
				var go = Instantiate(triggerPrefab, this.transform);
				go.transform.localPosition = Vector3.right* 5;
				go.GetComponent<RoomTrigger>().targetRoom = "room1";
			}
		}
	}
}
