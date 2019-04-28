using Barebones.MasterServer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAccessHandler : MonoBehaviour {
    public HelpBox _header = new HelpBox("This script waits for game server access to be received, " +
                                         "and then loads the appropriate scene");

    [Tooltip("If true, and if access contains the scene name - this script will automatically load that scene")]
    public bool LoadSceneIfFound = true;

    private void Awake() {
        Msf.Client.Rooms.AccessReceived += OnAccessReceived;
    }

    private void OnAccessReceived(RoomAccessPacket access) {
        // Set the access
        UnetRoomConnector.RoomAccess = access;

        if (LoadSceneIfFound && access.Properties.ContainsKey(MsfDictKeys.SceneName)) {
            var sceneName = access.Properties[MsfDictKeys.SceneName];
            if (sceneName != SceneManager.GetActiveScene().name)
                SceneManager.LoadScene(sceneName);
            else
                FindObjectOfType<UnetRoomConnector>().ConnectToGame(access);
        }
    }

    private void OnDestroy() {
        Msf.Client.Rooms.AccessReceived -= OnAccessReceived;
    }
}