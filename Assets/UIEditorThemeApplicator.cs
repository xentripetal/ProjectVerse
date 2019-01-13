using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Verse.Utilities;

public class UIEditorThemeApplicator : MonoBehaviour {
    public string CurrentThemeName;
    public Color TextColor;

    private void OnValidate() {
        if (!Directory.GetDirectories(Constants.EditorThemesFolder)
            .Any(dir => CurrentThemeName == dir.Split('/').Last())) {
            return;
        }

        var themeFolder = Constants.EditorThemesFolder + CurrentThemeName + "/";

        var images = transform.parent.GetComponentsInChildren<Image>();
        foreach (var image in images) {
            Debug.Log(image.gameObject.name);
            if (Directory.GetFiles(Constants.EditorThemesFolder + CurrentThemeName)
                .Any(path => image.sprite.name + ".png" == Path.GetFileName(path))) {
                var newSprite =
                    Resources.Load<Sprite>("UI/EditorThemes/" + CurrentThemeName + "/" + image.sprite.name);
                if (newSprite != null) {
                    Debug.Log("Changin");
                    Debug.Log(newSprite.name);
                    image.overrideSprite = newSprite;
                }
                else {
                    Debug.Log(themeFolder + image.sprite.name);
                }
            }
        }
    }
}