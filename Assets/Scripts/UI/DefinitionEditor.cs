using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using Verse.API.Models;

public class DefinitionEditor : MonoBehaviour
{
    public void LoadDefinition(String definition) {
        var def = getDef(definition);
        SetSprite(def.SpriteInfo);
    }

    private TileDef getDef(string definition) {
        return ObjectAtlas.getDef(definition);
    }

    private void SetSprite(SpriteInfo info) {
        var sprite = Utils.InfoToSprite(info);
        var ratio = CalculateSpriteRatio(sprite);
        SetSpriteContainerRatio(ratio);
        SetRendererSprite(sprite);
    }

    private float CalculateSpriteRatio(Sprite sprite) {
        return sprite.rect.width / sprite.rect.height;
    }

    private void SetSpriteContainerRatio(float ratio) {
        Debug.Log("Ratio: " + ratio);
        var widget = GetSpriteContainerWidget();
        widget.aspectRatio = ratio;
        widget.UpdateAnchors();
    }

    private UIWidget GetSpriteContainerWidget() {
        return GameObject.Find("Ratio Container").GetComponent<UIWidget>();
    }

    private void SetRendererSprite(Sprite sprite) {
        var renderer = GetUi2DSpriteWidget();
        renderer.sprite2D = sprite;
        renderer.UpdateAnchors();
        var grid = GetUI2DGrid();
        grid.mPixelSize = renderer.width / sprite.rect.width;
        grid.MarkAsChanged();
        grid.UpdateAnchors();

    }

    private UI2DSprite GetUI2DGrid() {
        return GameObject.Find("SpriteGrid").GetComponent<UI2DSprite>();
    }
    
    private UI2DSprite GetUi2DSpriteWidget() {
        return GameObject.Find("Rendered Sprite").GetComponent<UI2DSprite>();
    }
}
