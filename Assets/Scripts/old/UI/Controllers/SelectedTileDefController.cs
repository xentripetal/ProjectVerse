﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Verse.API.Models;

public class SelectedTileDefController : MonoBehaviour {
    private TileDef _currentlySelectedTile;
    private UIEditorState _uiEditorState;
    public TileDefSelected onTileDefSelected;
    public TileDefUnselected onTileDefUnselected;

    public void TileDefSelectedInternal(TileDef def) {
        if (_currentlySelectedTile != def && _currentlySelectedTile != null)
            onTileDefUnselected.Invoke(_currentlySelectedTile);

        _currentlySelectedTile = def;
        onTileDefSelected.Invoke(def);
    }

    private void Awake() {
        _uiEditorState = GetComponent<UIEditorState>();
    }

    private void Update() {
        if (!_uiEditorState.IsRoomLoaded) return;

        if (!Input.GetMouseButtonDown(1)) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (_currentlySelectedTile == null) return;

        onTileDefUnselected.Invoke(_currentlySelectedTile);
        _currentlySelectedTile = null;
    }

    [Serializable]
    public class TileDefSelected : UnityEvent<TileDef> {
    }

    [Serializable]
    public class TileDefUnselected : UnityEvent<TileDef> {
    }
}