using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Verse.API.Models;

public class DefinitionEditorVisualLogicHandler : MonoBehaviour {
    public UIPopupList TypeList;
    public UIInput NameInput;
    public UIInput PPUInput;

    public GameObject UnsavedChangesDialog;

    public UI2DSprite Grid;
    public UI2DSprite SpriteRenderer;
    public UIWidget RatioContainer;
    public GameObject DefinitionEditorWindow;
    
    public GameObject ColliderToggleWidget;
    public GameObject TriggerToggleWidget;
    public GameObject TransparencyToggleWidget;
    public GameObject ColliderWidget;
    public GameObject TransparencyWidget;
    public GameObject EditScriptsWidget;

    private UIToggle ColliderToggle;
    private UIToggle TriggerToggle;
    private UIToggle TransparencyToggle;

    private bool isDirty;
    private bool currentlyChanging;
    private bool HasCollider;
    private bool IsTrigger;
    private bool HasTransparency;
    private TileType _type = TileType.Tile;

    #region Def Loading
    
    public void LoadValues(UITileData data) {
        DefinitionEditorWindow.gameObject.SetActive(true);
        TypeList.value = data.Type.ToString();
        NameInput.value = data.name;
        PPUInput.value = data.PPU.ToString();
        ColliderToggle.value = data.HasCollider;
        TransparencyToggle.value = data.HasTransparency;
        TriggerToggle.value = data.IsTrigger;
        SetSprite(data.Sprite);
        StartCoroutine(ResetDirtyAfterUpdate());
    }

    IEnumerator ResetDirtyAfterUpdate() {
        yield return new WaitForSeconds(.2f);
        isDirty = false;
    }
    
    private void SetSprite(Sprite sprite) {
        var ratio = CalculateSpriteRatio(sprite);
        SetSpriteContainerRatio(ratio);
        SetRendererSprite(sprite);
    }

    private float CalculateSpriteRatio(Sprite sprite) {
        return sprite.rect.width / sprite.rect.height;
    }

    private void SetSpriteContainerRatio(float ratio) {
        var widget = RatioContainer;
        widget.aspectRatio = ratio;
        widget.UpdateAnchors();
    }

    private void SetRendererSprite(Sprite sprite) {
        var renderer = SpriteRenderer;
        renderer.sprite2D = sprite;
        renderer.UpdateAnchors();
        var grid = Grid;
        grid.mPixelSize = renderer.width / sprite.rect.width;
        grid.MarkAsChanged();
        grid.UpdateAnchors();
    }

    #endregion
    

    private void Awake() {
        ColliderToggle = ColliderToggleWidget.GetComponent<UIToggle>();
        TriggerToggle = TriggerToggleWidget.GetComponent<UIToggle>();
        TransparencyToggle = TransparencyToggleWidget.GetComponent<UIToggle>();
    }

    #region Notifiers

    public void OnSave() {
        isDirty = false;
    }

    public void OnSaveAs() {
        isDirty = false;
    }

    public void OnCloseWindow() {
        if (isDirty) {
            UnsavedChangesDialog.SetActive(true);
        }
        else {
            DefinitionEditorWindow.SetActive(false);
        }
    }

    public void ExitWithUnsavedChanges() {
        DefinitionEditorWindow.SetActive(false);
        CloseUnsavedChangesDialog();
    }

    public void CloseUnsavedChangesDialog() {
        UnsavedChangesDialog.SetActive(false);
    }
    
    public void NotifyChange() {
        isDirty = true;
    }
    
    public void TypeChanged() {
        isDirty = true;
        _type = (TileType) Enum.Parse(typeof(TileType), TypeList.value);
        UpdateVisuals();
    }

    public void TriggerToggleChanged() {
        isDirty = true;
        IsTrigger = TriggerToggle.value;
        if (IsTrigger) {
            ColliderToggle.value = true;
            HasCollider = true;
        }

        UpdateVisuals();
    }

    public void ColliderToggleChanged() {
        isDirty = true;
        HasCollider = ColliderToggle.value;
        if (!HasCollider) {
            TriggerToggle.value = false;
            IsTrigger = false;
        }

        UpdateVisuals();
    }

    public void TransparencyToggleChanged() {
        isDirty = true;
        HasTransparency = TransparencyToggle.value;
        UpdateVisuals();
    }
    
    #endregion
    
    #region Visual Logic

    public void UpdateVisuals() {
        if (_type == TileType.ScriptableThing) {
            EditScriptsWidget.SetActive(true);
            TriggerToggleWidget.SetActive(true);
        }
        else {
            EditScriptsWidget.SetActive(false);
            TriggerToggleWidget.SetActive(false);
        }

        if (_type != TileType.Tile) {
            ColliderToggleWidget.SetActive(true);
            TransparencyToggleWidget.SetActive(true);
            TransparencyWidget.SetActive(true);
            ColliderWidget.SetActive(true);
            TransparencyWidget.SetActive(true);
        }
        else {
            ColliderToggleWidget.SetActive(false);
            TransparencyToggleWidget.SetActive(false);
            TransparencyWidget.SetActive(false);
            ColliderWidget.SetActive(false);
            TransparencyWidget.SetActive(false);
        }

        ColliderWidget.SetActive(HasCollider);
        TriggerToggleWidget.SetActive(HasCollider);

        TransparencyWidget.SetActive(HasTransparency);
    }
    
    #endregion
}