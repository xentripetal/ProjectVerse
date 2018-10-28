using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SwappableSpriteAnimationController : MonoBehaviour {

    public string bodySpriteSheetName;
    public string hairSpriteSheetName;
    public string shirtSpriteSheetName;
    public string pantsSpriteSheetName;

    public SpriteRenderer bodySpriteRenderer;
    public SpriteRenderer hairSpriteRenderer;
    public SpriteRenderer shirtSpriteRenderer;
    public SpriteRenderer pantsSpriteRenderer;

    private string loadedBodySpriteSheetName;
    private string loadedHairSpriteSheetName;
    private string loadedShirtSpriteSheetName;
    private string loadedPantsSpriteSheetName;

    private Dictionary<string, Sprite> bodySpriteSheet;
    private Dictionary<string, Sprite> hairSpriteSheet;
    private Dictionary<string, Sprite> shirtSpriteSheet;
    private Dictionary<string, Sprite> pantsSpriteSheet;

    private void Start()
    {
        bodySpriteSheet = LoadSpriteSheet(bodySpriteSheetName);
        loadedBodySpriteSheetName = bodySpriteSheetName;

        hairSpriteSheet = LoadSpriteSheet(hairSpriteSheetName);
        loadedHairSpriteSheetName = hairSpriteSheetName;

        shirtSpriteSheet = LoadSpriteSheet(shirtSpriteSheetName);
        loadedShirtSpriteSheetName = shirtSpriteSheetName;

        pantsSpriteSheet = LoadSpriteSheet(pantsSpriteSheetName);
        loadedPantsSpriteSheetName = pantsSpriteSheetName;
    }

    private Dictionary<string, Sprite> LoadSpriteSheet(string spriteSheet)
    {
        var sprites = Resources.LoadAll<Sprite>(spriteSheet);
        return sprites.ToDictionary(x => x.name, x => x);
    }

    private void LateUpdate()
    {

        if (loadedBodySpriteSheetName != bodySpriteSheetName)
        {
            bodySpriteSheet = LoadSpriteSheet(bodySpriteSheetName);
            loadedBodySpriteSheetName = bodySpriteSheetName;
        }


        if (loadedHairSpriteSheetName != hairSpriteSheetName)
        {
            hairSpriteSheet = LoadSpriteSheet(hairSpriteSheetName);
            loadedHairSpriteSheetName = hairSpriteSheetName;
        }


        if (loadedShirtSpriteSheetName != shirtSpriteSheetName)
        {
            shirtSpriteSheet = LoadSpriteSheet(shirtSpriteSheetName);
            loadedShirtSpriteSheetName = shirtSpriteSheetName;
        }


        if (loadedPantsSpriteSheetName != pantsSpriteSheetName)
        {
            pantsSpriteSheet = LoadSpriteSheet(pantsSpriteSheetName);
            loadedPantsSpriteSheetName = pantsSpriteSheetName;
        }

        string currentSpriteName = bodySpriteRenderer.sprite.name;

        bodySpriteRenderer.sprite = bodySpriteSheet[currentSpriteName];
        hairSpriteRenderer.sprite = hairSpriteSheet[currentSpriteName];
        shirtSpriteRenderer.sprite = shirtSpriteSheet[currentSpriteName];
        pantsSpriteRenderer.sprite = pantsSpriteSheet[currentSpriteName];
    }
}
