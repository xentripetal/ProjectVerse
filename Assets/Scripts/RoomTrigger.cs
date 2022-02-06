using System;
using Mirror;
using Mirror.Examples.MultipleAdditiveScenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse {
    public class RoomTrigger : MonoBehaviour {
        public string targetRoom;

        private void OnTriggerEnter2D(Collider2D col) {
            Debug.Log(col.gameObject.name + " entered trigger");
            if (!NetworkServer.active) {
                return;
            }

            var networkIdentity = col.gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity == null) {
                return;
            }

            var room = Mocks.FromId(targetRoom);

            StartCoroutine(RoomManager.ActivateRoom(room, networkIdentity));
        }
    }
}