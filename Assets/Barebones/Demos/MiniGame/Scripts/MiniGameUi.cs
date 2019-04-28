using UnityEngine;
using UnityEngine.UI;

public class MiniGameUi : MonoBehaviour {
    public static MiniGameUi Instance;
    private Color _defaultWalkBtnColor;

    public Image AutoWalkBg;
    public Text Coins;

    public bool IsAutoWalk { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void ToggleAutoWalk() {
        // Save defualt color
        if (!IsAutoWalk)
            _defaultWalkBtnColor = AutoWalkBg.color;

        IsAutoWalk = !IsAutoWalk;

        AutoWalkBg.color = IsAutoWalk ? Color.red : _defaultWalkBtnColor;
    }

    public void OnPlayerSpawned(MiniPlayerController player) {
        player.CoinsChanged += () => { Coins.text = player.Coins.ToString(); };
    }
}