using System;
using UnityEngine;
using UnityEngine.Events;

namespace HappyHarvest
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnEnter.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnExit.Invoke();
        }
    }
}
