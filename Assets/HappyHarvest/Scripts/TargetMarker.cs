using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyHarvest
{
    public class TargetMarker : MonoBehaviour
    {
        
        [SerializeField]
        private Color _activeColor = Color.white;
        [SerializeField]
        private Color _inactiveColor = Color.gray;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        public void Activate()
        {
            Show();
            _renderer.color = _activeColor;
        }

        public void Deactivate()
        {
            Show();
            _renderer.color = _inactiveColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
}
}
