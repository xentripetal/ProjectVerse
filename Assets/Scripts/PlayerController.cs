using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Verse {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : NetworkBehaviour {
        public float speed = 3.0f;

        private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start() {
            rb = GetComponent<Rigidbody2D>();
            if (isLocalPlayer) {
                Camera.main.GetComponent<Follower>().target = transform;
            }
        }

        // Update is called once per frame
        void Update() {
            if (!isLocalPlayer) {
                return;
            }

            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb.velocity = input * speed;
        }
    }
}