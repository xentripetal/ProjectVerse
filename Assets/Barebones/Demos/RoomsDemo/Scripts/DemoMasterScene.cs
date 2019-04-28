using Barebones.MasterServer;
using UnityEngine;
using UnityEngine.UI;

public class DemoMasterScene : MonoBehaviour {
    public MasterServerBehaviour MasterServer;
    public Button StartMasterBtn;

    private void Awake() {
        MasterServer = MasterServer ?? FindObjectOfType<MasterServerBehaviour>();
    }

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        if (MasterServer.IsRunning == StartMasterBtn.gameObject.activeSelf)
            StartMasterBtn.gameObject.SetActive(!MasterServer.IsRunning);
    }

    public void OnStartMasterClick() {
        MasterServer.StartServer(MasterServer.Port);
    }
}