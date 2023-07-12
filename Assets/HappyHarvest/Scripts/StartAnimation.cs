using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyHarvest
{
    public class StartAnimation : MonoBehaviour
    {
        public Animation Animation;

        public void Trigger()
        {
            Animation.Play();
        }
    }
}