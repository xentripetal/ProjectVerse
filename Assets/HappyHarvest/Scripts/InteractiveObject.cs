using UnityEngine;


namespace HappyHarvest
{
    /// <summary>
    /// Base class for object that can be clicked on in the scene. InteractWith will 
    /// </summary>
    public abstract class InteractiveObject : MonoBehaviour
    {
        public abstract void InteractedWith();

        protected void Awake()
        {
            //The Player control raycast in layer 31 to check for interactive object
            gameObject.layer = 31;
        }
    }
}