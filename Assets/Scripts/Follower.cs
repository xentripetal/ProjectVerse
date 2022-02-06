using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
    public class Follower : MonoBehaviour {
        public Transform target;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null) {
                transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            }
        }
    }
}
