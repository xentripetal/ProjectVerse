using System.Collections.Generic;
using Barebones.MasterServer;
using UnityEngine;

public class DemoMainScene : MonoBehaviour {
    public List<GameObject> EnableObjectsOnLogIn;

    public LobbyUi LobbyUi;

    private void Awake() {
        Msf.Client.Auth.LoggedIn += OnLoggedIn;

        // In case we're already logged in
        if (Msf.Client.Auth.IsLoggedIn)
            OnLoggedIn();
    }

    private void OnLoggedIn() {
        foreach (var obj in EnableObjectsOnLogIn)
            if (obj != null)
                obj.SetActive(true);
    }

    private void OnDestroy() {
        Msf.Client.Auth.LoggedIn -= OnLoggedIn;
    }
}