using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIController : MonoBehaviour {
    private RoomController roomController;

    public static APIController Instance;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        roomController = RoomController.Instance;
    }

    public void OnPlayerEnter(Player player, GameObject target) {
        roomController
    }
    
    void Update()
    {
        
    }
}
