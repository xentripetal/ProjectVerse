using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse.API;
using Verse.Systems.Visual;

public class WorldChangeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (string.Equals(RoomController.Instance.CurrentRoom.Name, "main", StringComparison.Ordinal)) {
                RoomController.Instance.ChangeRoom("Test");
            }
            else {
                RoomController.Instance.ChangeRoom("main");
            }
        } 
    }
}
