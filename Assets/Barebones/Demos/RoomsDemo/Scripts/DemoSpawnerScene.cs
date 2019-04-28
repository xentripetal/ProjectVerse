using Barebones.MasterServer;
using UnityEngine;
using UnityEngine.UI;

public class DemoSpawnerScene : MonoBehaviour {
    public SpawnerBehaviour Spawner;
    public Button StartSpawnerButton;

    private void Awake() {
        Spawner = Spawner ?? FindObjectOfType<SpawnerBehaviour>();
    }

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        var isSpawnerStarted = Spawner.IsSpawnerStarted;
        var isConnectedToMaster = Msf.Server.Spawners.Connection.IsConnected;
        var isButtonVisible = !isSpawnerStarted && isConnectedToMaster;

        StartSpawnerButton.gameObject.SetActive(isButtonVisible);
    }

    public void OnStartSpawnerClick() {
        Spawner.StartSpawner();
    }
}