using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerData : MonoBehaviour {
    public string room;
    
    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<Room>().changeRoom(room);
    }
}
